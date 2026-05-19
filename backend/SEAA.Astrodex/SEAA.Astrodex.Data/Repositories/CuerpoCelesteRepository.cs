// Data/Repositories/CuerpoCelesteRepository.cs
using Microsoft.EntityFrameworkCore;
using SEAA.Astrodex.Core.DTOs;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;
using SEAA.Astrodex.Data.Context;
using SEAA.Astrodex.Infrastructure.Structures;
using SEAA.Astrodex.Infrastructure.External;


namespace SEAA.Astrodex.Data.Repositories
{
    public class CuerpoCelesteRepository : ICuerpoCelesteRepository
    {
        private readonly AstronomiaContext _context;
        private readonly MemoriaService _memoria;
        private readonly SolarSystemApiService _apiService;

        public CuerpoCelesteRepository(
            AstronomiaContext context,
            MemoriaService memoria,
            SolarSystemApiService apiService)
        {
            _context = context;
            _memoria = memoria;
            _apiService = apiService;
        }

        // ─── Cache-Aside central ──────────────────────────────────

        public async Task<CuerpoCeleste?> ObtenerCuerpoCelesteAsync(string idONombre)
        {
            // 1. Memoria
            var enMemoria = BuscarEnMemoria(idONombre);
            if (enMemoria != null)
                return enMemoria;

            // 2. Base de datos
            var enBd = await BuscarEnBaseDatosAsync(idONombre);
            if (enBd != null)
            {
                CargarEnMemoria(enBd);
                return enBd;
            }

            // 3. API externa
            var deApi = await BuscarEnApiAsync(idONombre);
            if (deApi != null)
            {
                await GuardarEnBaseDatosAsync(deApi);
                CargarEnMemoria(deApi);
                return deApi;
            }

            return null;
        }

        // ─── Métodos individuales ─────────────────────────────────

        public CuerpoCeleste? BuscarEnMemoria(string idONombre)
        {
            // Primero busca por id (O(1) con Dictionary)
            var porId = _memoria.BuscarPorId(idONombre);
            if (porId != null)
                return porId;

            // Si no encuentra, busca por nombre flexible
            return _memoria.BuscarPorNombre(idONombre);
        }

        public async Task<CuerpoCeleste?> BuscarEnBaseDatosAsync(string idONombre)
        {
            // Una sola consulta SQL con OR para los 3 campos
            return await _context.CuerposCelestes
                .FirstOrDefaultAsync(c =>
                    c.Id.ToLower() == idONombre.ToLower() ||
                    c.NombreIngles.ToLower() == idONombre.ToLower() ||
                    c.Nombre.ToLower() == idONombre.ToLower());
        }

        public async Task<CuerpoCeleste?> BuscarEnApiAsync(string idONombre)
        {
            // Primero intenta directamente por id
            // (es la forma más rápida y eficiente)
            var porId = await _apiService.GetCuerpoPorIdAsync(idONombre);
            if (porId != null)
                return CuerpoCelesteMapper.ToEntity(porId);

            // Si no encuentra por id, usa el filtro de la API
            // por nombre en inglés (case-insensitive)
            var porNombre = await _apiService.GetCuerposPorFiltroAsync(
                "englishName", "eq", idONombre);

            if (porNombre.Any())
                return CuerpoCelesteMapper.ToEntity(porNombre.First());

            return null;
        }

        // ─── Persistencia ─────────────────────────────────────────

        public async Task GuardarEnBaseDatosAsync(CuerpoCeleste cuerpo)
        {
            var existe = await _context.CuerposCelestes
                .AnyAsync(c => c.Id == cuerpo.Id);

            if (!existe)
            {
                cuerpo.FechaCarga = DateTime.Now;
                await _context.CuerposCelestes.AddAsync(cuerpo);
                await _context.SaveChangesAsync();
            }
        }

        public void CargarEnMemoria(CuerpoCeleste cuerpo)
        {
            _memoria.Agregar(cuerpo);
        }

        // ─── Historial en BD ──────────────────────────────────────

        public async Task RegistrarHistorialEnBdAsync(
            string cuerpoId, string tipoConsulta)
        {
            var historial = new HistorialConsulta
            {
                CuerpoCelesteId = cuerpoId,
                TipoConsulta = tipoConsulta,
                FechaConsulta = DateTime.Now
            };

            await _context.HistorialConsultas.AddAsync(historial);
            await _context.SaveChangesAsync();
        }

        // Trae una página de cuerpos desde BD usando JOIN con la intermedia
        // Retorna null si la página no fue cargada antes
        public async Task<List<CuerpoCeleste>?> ObtenerPaginaCargadaAsync(
            string tipo, int pagina, int tamanio)
        {
            var paginaRegistrada = await _context.PaginasCargadas
                .FirstOrDefaultAsync(p =>
                    p.TipoCuerpo.ToLower() == tipo.ToLower() &&
                    p.NumeroPagina == pagina &&
                    p.TamanioPagina == tamanio);

            if (paginaRegistrada == null)
                return null;

            return await _context.PaginasCargadasCuerpos
                .Where(pcc => pcc.PaginaCargadaId == paginaRegistrada.Id)
                .Join(_context.CuerposCelestes,
                    pcc => pcc.CuerpoCelesteId,
                    cc => cc.Id,
                    (pcc, cc) => cc)
                .OrderBy(cc => cc.NombreIngles)
                .ToListAsync();
        }

        // Trae una página directamente desde la API
        public async Task<List<CuerpoCeleste>> BuscarPaginaEnApiAsync(
            string tipo, int pagina, int tamanio)
        {
            var dtos = await _apiService.GetCuerposPorFiltroAsync(
                "bodyType", "eq", tipo,
                orden: "englishName,asc",
                pagina: pagina,
                tamanio: tamanio);

            return dtos.Select(CuerpoCelesteMapper.ToEntity).ToList();
        }

        // Registra una página cargada y sus cuerpos en la tabla intermedia
        public async Task RegistrarPaginaCargadaAsync(
            string tipo, int pagina, int tamanio, List<CuerpoCeleste> cuerpos)
        {
            var registro = new PaginaCargada
            {
                TipoCuerpo = tipo,
                NumeroPagina = pagina,
                TamanioPagina = tamanio,
                FechaCarga = DateTime.Now
            };

            await _context.PaginasCargadas.AddAsync(registro);
            await _context.SaveChangesAsync();

            // Crea registros en la tabla intermedia
            var registros = cuerpos.Select(c => new PaginaCargadaCuerpo
            {
                PaginaCargadaId = registro.Id,
                CuerpoCelesteId = c.Id
            }).ToList();

            await _context.PaginasCargadasCuerpos.AddRangeAsync(registros);
            await _context.SaveChangesAsync();
        }

        // Guarda en lote sin duplicar los que ya existen
        public async Task GuardarLoteEnBaseDatosAsync(List<CuerpoCeleste> cuerpos)
        {
            if (!cuerpos.Any())
                return;

            var idsEntrada = cuerpos.Select(c => c.Id).ToList();

            var idsExistentes = await _context.CuerposCelestes
                .Where(c => idsEntrada.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            var nuevos = cuerpos
                .Where(c => !idsExistentes.Contains(c.Id))
                .ToList();

            if (nuevos.Any())
            {
                foreach (var c in nuevos)
                    c.FechaCarga = DateTime.Now;

                await _context.CuerposCelestes.AddRangeAsync(nuevos);
                await _context.SaveChangesAsync();
            }
        }
    }
}