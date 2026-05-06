// Data/Repositories/CuerpoCelesteRepository.cs
using Microsoft.EntityFrameworkCore;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;
using SEAA.Astrodex.Data.Context;

namespace SEAA.Astrodex.Data.Repositories
{
    public class CuerpoCelesteRepository : ICuerpoCelesteRepository
    {
        private readonly AstronomiaContext _context;

        // Estructuras en memoria
        private static readonly Dictionary<string, CuerpoCeleste> _tablaHash
            = new Dictionary<string, CuerpoCeleste>();

        private static readonly LinkedList<CuerpoCeleste> _listaEnlazada
            = new LinkedList<CuerpoCeleste>();

        public CuerpoCelesteRepository(AstronomiaContext context)
        {
            _context = context;
        }

        // ─── Memoria ────────────────────────────────────────────

        public void CargarEnMemoria(CuerpoCeleste cuerpo)
        {
            // Solo carga si no existe ya en memoria
            if (!_tablaHash.ContainsKey(cuerpo.Id))
            {
                _tablaHash[cuerpo.Id] = cuerpo;
                _listaEnlazada.AddLast(cuerpo);
            }
        }

        public CuerpoCeleste? BuscarEnMemoriaPorId(string id)
        {
            _tablaHash.TryGetValue(id, out var cuerpo);
            return cuerpo;
        }

        public List<CuerpoCeleste> ObtenerTodosEnMemoria()
        {
            return _listaEnlazada.ToList();
        }

        // ─── Base de datos ───────────────────────────────────────

        public async Task<CuerpoCeleste?> BuscarEnBaseDatosPorIdAsync(string id)
        {
            return await _context.CuerposCelestes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task GuardarEnBaseDatosAsync(CuerpoCeleste cuerpo)
        {
            // Verifica si ya existe antes de insertar
            var existe = await _context.CuerposCelestes
                .AnyAsync(c => c.Id == cuerpo.Id);

            if (!existe)
            {
                cuerpo.FechaCarga = DateTime.Now;
                await _context.CuerposCelestes.AddAsync(cuerpo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task GuardarLoteEnBaseDatosAsync(List<CuerpoCeleste> cuerpos)
        {
            foreach (var cuerpo in cuerpos)
            {
                var existe = await _context.CuerposCelestes
                    .AnyAsync(c => c.Id == cuerpo.Id);

                if (!existe)
                {
                    cuerpo.FechaCarga = DateTime.Now;
                    await _context.CuerposCelestes.AddAsync(cuerpo);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}