using System;
using System.Collections.Generic;
using System.Text;

using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.Infrastructure.Strategies
{
    // Compara masa, radio y gravedad entre dos cuerpos celestes
    public class EstrategiaFisica : IEstrategiaRelacion
    {
        public string Tipo => TiposRelacion.FISICA;

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

            // Verifica que ambos tengan datos físicos válidos
            if (origen.MasaValor <= 0 || destino.MasaValor <= 0 ||
                origen.RadioMedio <= 0 || destino.RadioMedio <= 0 ||
                origen.Gravedad <= 0 || destino.Gravedad <= 0)
            {
                relacion.ValorCalculado = 0;
                relacion.Descripcion = "No hay relación física disponible " +
                    "para esta combinación";
                return relacion;
            }

            // Calcula la masa real considerando el exponente
            var masaOrigen = origen.MasaValor *
                Math.Pow(10, origen.MasaExponente);
            var masaDestino = destino.MasaValor *
                Math.Pow(10, destino.MasaExponente);

            var razonMasa = masaOrigen / masaDestino;
            var razonRadio = origen.RadioMedio / destino.RadioMedio;
            var razonGravedad = origen.Gravedad / destino.Gravedad;

            relacion.ValorCalculado = Math.Round(razonMasa, 4);

            relacion.Descripcion =
                $"{origen.NombreIngles} tiene {razonMasa:F2}x la masa, " +
                $"{razonRadio:F2}x el radio y {razonGravedad:F2}x la gravedad " +
                $"de {destino.NombreIngles}";

            return relacion;
        }
    }
}
