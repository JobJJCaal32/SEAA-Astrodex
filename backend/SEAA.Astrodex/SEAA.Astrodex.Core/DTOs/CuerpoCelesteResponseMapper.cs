using System;
using System.Collections.Generic;
using System.Text;

// Core/DTOs/CuerpoCelesteResponseMapper.cs
using SEAA.Astrodex.Core.Entities;

namespace SEAA.Astrodex.Core.DTOs
{
    public static class CuerpoCelesteResponseMapper
    {
        public static CuerpoCelesteResponseDto ToResponseDto(
            CuerpoCeleste cuerpo, string fuente)
        {
            return new CuerpoCelesteResponseDto
            {
                Fuente = fuente,
                Id = cuerpo.Id,
                Nombre = cuerpo.Nombre,
                NombreIngles = cuerpo.NombreIngles,
                TipoCuerpo = cuerpo.TipoCuerpo,
                EsPlaneta = cuerpo.EsPlaneta,
                SemimajorAxis = cuerpo.SemimajorAxis,
                Perihelio = cuerpo.Perihelio,
                Afelio = cuerpo.Afelio,
                Excentricidad = cuerpo.Excentricidad,
                Inclinacion = cuerpo.Inclinacion,
                MasaValor = cuerpo.MasaValor,
                MasaExponente = cuerpo.MasaExponente,
                VolumenValor = cuerpo.VolumenValor,
                VolumenExponente = cuerpo.VolumenExponente,
                Densidad = cuerpo.Densidad,
                Gravedad = cuerpo.Gravedad,
                VelocidadEscape = cuerpo.VelocidadEscape,
                RadioMedio = cuerpo.RadioMedio,
                RadioEcuatorial = cuerpo.RadioEcuatorial,
                RadioPolar = cuerpo.RadioPolar,
                Orbita = cuerpo.Orbita,
                Rotacion = cuerpo.Rotacion,
                InclinacionAxial = cuerpo.InclinacionAxial,
                TempPromedio = cuerpo.TempPromedio,
                DescubridoPor = cuerpo.DescubridoPor,
                FechaDescubrimiento = cuerpo.FechaDescubrimiento,
                NombreAlternativo = cuerpo.NombreAlternativo,
                Dimension = cuerpo.Dimension,
                UrlApi = cuerpo.UrlApi,
                PlanetaPadreId = cuerpo.PlanetaPadreId,
                FechaCarga = cuerpo.FechaCarga
            };
        }
    }
}
