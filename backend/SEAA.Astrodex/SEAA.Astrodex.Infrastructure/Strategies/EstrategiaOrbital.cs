using System;
using System.Collections.Generic;
using System.Text;

using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.Infrastructure.Strategies
{
    // Compara la relación orbital entre dos cuerpos celestes
    public class EstrategiaOrbital : IEstrategiaRelacion
    {
        public string Tipo => TiposRelacion.ORBITAL;

        public RelacionCeleste Ejecutar(
            CuerpoCeleste origen, CuerpoCeleste destino)
        {
            var relacion = new RelacionCeleste
            {
                TipoRelacion = Tipo,
                UnidadMedida = "días",
                CuerpoOrigenId = origen.Id,
                CuerpoDestinoId = destino.Id,
                FechaConsulta = DateTime.Now
            };

            // Verifica si tienen datos válidos
            if (origen.Orbita <= 0 || destino.Orbita <= 0)
            {
                relacion.ValorCalculado = 0;
                relacion.Descripcion = "No hay relación orbital disponible " +
                    "para esta combinación";
                return relacion;
            }

            // Compara los períodos orbitales
            var razon = origen.Orbita / destino.Orbita;
            relacion.ValorCalculado = Math.Round(razon, 4);

            // Si comparten planeta padre
            if (!string.IsNullOrEmpty(origen.PlanetaPadreId) &&
                origen.PlanetaPadreId == destino.PlanetaPadreId)
            {
                relacion.Descripcion =
                    $"Ambos orbitan al mismo cuerpo. " +
                    $"{origen.NombreIngles} tarda {origen.Orbita} días, " +
                    $"{destino.NombreIngles} tarda {destino.Orbita} días";
            }
            else
            {
                relacion.Descripcion =
                    $"El período orbital de {origen.NombreIngles} es {razon:F2} " +
                    $"veces el de {destino.NombreIngles}";
            }

            return relacion;
        }
    }
}
