using System;
using System.Collections.Generic;
using System.Text;

using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.Infrastructure.Strategies
{
    // Calcula la distancia entre dos cuerpos celestes usando semimajorAxis
    public class EstrategiaDistancia : IEstrategiaRelacion
    {
        public string Tipo => TiposRelacion.DISTANCIA;

        public RelacionCeleste Ejecutar(
            CuerpoCeleste origen, CuerpoCeleste destino)
        {
            var relacion = new RelacionCeleste
            {
                TipoRelacion = Tipo,
                UnidadMedida = "km",
                CuerpoOrigenId = origen.Id,
                CuerpoDestinoId = destino.Id,
                FechaConsulta = DateTime.Now
            };

            // Verifica que ambos tengan semimajorAxis válido
            if (origen.SemimajorAxis <= 0 || destino.SemimajorAxis <= 0)
            {
                relacion.ValorCalculado = 0;
                relacion.Descripcion = "No hay relación de distancia disponible " +
                    "para esta combinación";
                return relacion;
            }

            // Cálculos de distancia
            var distanciaPromedio = Math.Abs(
                origen.SemimajorAxis - destino.SemimajorAxis);

            var distanciaMinima = Math.Abs(
                origen.Perihelio - destino.Afelio);

            var distanciaMaxima = Math.Abs(
                origen.Afelio - destino.Perihelio);

            relacion.ValorCalculado = Math.Round(distanciaPromedio, 2);

            relacion.Descripcion =
                $"Distancia promedio: {distanciaPromedio:N0} km. " +
                $"Mínima: {distanciaMinima:N0} km. " +
                $"Máxima: {distanciaMaxima:N0} km";

            return relacion;
        }
    }
}
