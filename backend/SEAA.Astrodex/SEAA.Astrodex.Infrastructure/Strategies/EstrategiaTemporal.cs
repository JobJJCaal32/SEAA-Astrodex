using System;
using System.Collections.Generic;
using System.Text;

using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.Infrastructure.Strategies
{
    // Compara los tiempos de órbita y rotación entre dos cuerpos
    public class EstrategiaTemporal : IEstrategiaRelacion
    {
        public string Tipo => TiposRelacion.TEMPORAL;

        public RelacionCeleste Ejecutar(
            CuerpoCeleste origen, CuerpoCeleste destino)
        {
            var relacion = new RelacionCeleste
            {
                TipoRelacion = Tipo,
                UnidadMedida = "ratio",
                CuerpoOrigenId = origen.Id,
                CuerpoDestinoId = destino.Id,
                FechaConsulta = DateTime.Now
            };

            // Verifica que ambos tengan datos temporales
            if (origen.Orbita <= 0 || destino.Orbita <= 0 ||
                origen.Rotacion == 0 || destino.Rotacion == 0)
            {
                relacion.ValorCalculado = 0;
                relacion.Descripcion = "No hay relación temporal disponible " +
                    "para esta combinación";
                return relacion;
            }

            var razonOrbita = origen.Orbita / destino.Orbita;
            var razonRotacion = Math.Abs(origen.Rotacion / destino.Rotacion);

            relacion.ValorCalculado = Math.Round(razonOrbita, 4);

            relacion.Descripcion =
                $"El año de {origen.NombreIngles} es {razonOrbita:F2}x el de " +
                $"{destino.NombreIngles}. " +
                $"El día de {origen.NombreIngles} es {razonRotacion:F2}x el de " +
                $"{destino.NombreIngles}";

            return relacion;
        }
    }
}