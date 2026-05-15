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

        public async Task<CuerpoCeleste?> ObtenerCuerpoCelesteAsync(string id)
        {
            // 1. Memoria
            var enMemoria = BuscarEnMemoria(id);
            if (enMemoria != null)
                return enMemoria;

            // 2. Base de datos
            var enBd = await BuscarEnBaseDatosAsync(id);
            if (enBd != null)
            {
                CargarEnMemoria(enBd);
                return enBd;
            }

            // 3. API externa
            var deApi = await BuscarEnApiAsync(id);
            if (deApi != null)
            {
                await GuardarEnBaseDatosAsync(deApi);
                CargarEnMemoria(deApi);
                return deApi;
            }

            return null;
        }

        // ─── Métodos individuales ─────────────────────────────────

        public CuerpoCeleste? BuscarEnMemoria(string id)
        {
            return _memoria.BuscarPorId(id);
        }

        public async Task<CuerpoCeleste?> BuscarEnBaseDatosAsync(string id)
        {
            return await _context.CuerposCelestes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CuerpoCeleste?> BuscarEnApiAsync(string id)
        {
            var dto = await _apiService.GetCuerpoPorIdAsync(id);
            if (dto == null)
                return null;

            return CuerpoCelesteMapper.ToEntity(dto);
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