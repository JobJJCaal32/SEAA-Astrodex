using System;
using System.Collections.Generic;
using System.Text;

using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.Infrastructure.Strategies
{
    // Verifica si dos cuerpos comparten planeta padre
    public class EstrategiaFamiliar : IEstrategiaRelacion
    {
        public string Tipo => TiposRelacion.FAMILIAR;

        public RelacionCeleste Ejecutar(
            CuerpoCeleste origen, CuerpoCeleste destino)
        {
            var relacion = new RelacionCeleste
            {
                TipoRelacion = Tipo,
                UnidadMedida = "booleano",
                CuerpoOrigenId = origen.Id,
                CuerpoDestinoId = destino.Id,
                FechaConsulta = DateTime.Now
            };

            // Caso 1: ambos son cuerpos sin padre (orbitan al Sol)
            if (string.IsNullOrEmpty(origen.PlanetaPadreId) &&
                string.IsNullOrEmpty(destino.PlanetaPadreId))
            {
                relacion.ValorCalculado = 1;
                relacion.Descripcion = "Ambos cuerpos orbitan directamente al Sol";
                return relacion;
            }

            // Caso 2: uno tiene padre y el otro no
            if (string.IsNullOrEmpty(origen.PlanetaPadreId) ||
                string.IsNullOrEmpty(destino.PlanetaPadreId))
            {
                relacion.ValorCalculado = 0;
                relacion.Descripcion = "No hay relación familiar entre estos cuerpos";
                return relacion;
            }

            // Caso 3: comparten padre
            if (origen.PlanetaPadreId.Equals(destino.PlanetaPadreId,
                    StringComparison.OrdinalIgnoreCase))
            {
                relacion.ValorCalculado = 1;
                relacion.Descripcion =
                    $"Ambos cuerpos orbitan a {origen.PlanetaPadreId}";
                return relacion;
            }

            // Caso 4: diferentes padres
            relacion.ValorCalculado = 0;
            relacion.Descripcion =
                $"{origen.NombreIngles} orbita a {origen.PlanetaPadreId}, " +
                $"{destino.NombreIngles} orbita a {destino.PlanetaPadreId}";

            return relacion;
        }
    }
}
