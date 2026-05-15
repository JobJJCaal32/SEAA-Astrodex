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
    }
}