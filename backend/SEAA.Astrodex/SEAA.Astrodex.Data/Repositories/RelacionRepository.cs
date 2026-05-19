using System;
using System.Collections.Generic;
using System.Text;

using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;
using SEAA.Astrodex.Data.Context;

namespace SEAA.Astrodex.Data.Repositories
{
    public class RelacionRepository : IRelacionRepository
    {
        private readonly AstronomiaContext _context;

        public RelacionRepository(AstronomiaContext context)
        {
            _context = context;
        }

        // Guarda una relación nueva en la tabla
        public async Task GuardarRelacionAsync(RelacionCeleste relacion)
        {
            await _context.RelacionesCelestes.AddAsync(relacion);
            await _context.SaveChangesAsync();
        }

        // Guarda múltiples relaciones de una sola vez
        public async Task GuardarRelacionesAsync(List<RelacionCeleste> relaciones)
        {
            if (!relaciones.Any())
                return;

            await _context.RelacionesCelestes.AddRangeAsync(relaciones);
            await _context.SaveChangesAsync();
        }
    }
}
