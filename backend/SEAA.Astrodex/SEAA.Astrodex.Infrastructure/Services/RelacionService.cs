using System;
using System.Collections.Generic;
using System.Text;

using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.DTOs;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.Infrastructure.Services
{
    public class RelacionService : IRelacionService
    {
        private readonly ICuerpoCelesteRepository _cuerpoRepository;
        private readonly IRelacionRepository _relacionRepository;
        private readonly IEnumerable<IEstrategiaRelacion> _estrategias;

        public RelacionService(
            ICuerpoCelesteRepository cuerpoRepository,
            IRelacionRepository relacionRepository,
            IEnumerable<IEstrategiaRelacion> estrategias)
        {
            _cuerpoRepository = cuerpoRepository;
            _relacionRepository = relacionRepository;
            _estrategias = estrategias;
        }

        // Operación 5: analiza relaciones entre dos cuerpos celestes
        public async Task<List<RelacionResponseDto>?> AnalizarRelacionAsync(
            string idOrigen, string idDestino, string tipoRelacion)
        {
            // Obtiene los dos cuerpos con Operación 1
            var origen = await _cuerpoRepository.ObtenerCuerpoCelesteAsync(idOrigen);
            var destino = await _cuerpoRepository.ObtenerCuerpoCelesteAsync(idDestino);

            if (origen == null || destino == null)
                return null;

            var resultados = new List<RelacionCeleste>();

            if (tipoRelacion.Equals(TiposRelacion.TODOS,
                StringComparison.OrdinalIgnoreCase))
            {
                // Ejecuta todas las estrategias
                foreach (var estrategia in _estrategias)
                {
                    var relacion = estrategia.Ejecutar(origen, destino);
                    resultados.Add(relacion);
                }
            }
            else
            {
                // Ejecuta solo la estrategia seleccionada
                var estrategia = _estrategias.FirstOrDefault(e =>
                    e.Tipo.Equals(tipoRelacion, StringComparison.OrdinalIgnoreCase));

                if (estrategia == null)
                    return null;

                var relacion = estrategia.Ejecutar(origen, destino);
                resultados.Add(relacion);
            }

            // Guarda todas las relaciones en BD
            await _relacionRepository.GuardarRelacionesAsync(resultados);

            // Registra en historial
            await _cuerpoRepository.RegistrarHistorialEnBdAsync(
                origen.Id, TiposConsulta.ANALISIS_RELACION);

            // Mapea a DTOs de respuesta
            return resultados.Select(r => new RelacionResponseDto
            {
                TipoRelacion = r.TipoRelacion,
                ValorCalculado = r.ValorCalculado,
                UnidadMedida = r.UnidadMedida,
                Descripcion = r.Descripcion,
                TieneRelacion = r.ValorCalculado != 0,
                CuerpoOrigenId = r.CuerpoOrigenId,
                CuerpoDestinoId = r.CuerpoDestinoId,
                FechaConsulta = r.FechaConsulta
            }).ToList();
        }
    }
}
