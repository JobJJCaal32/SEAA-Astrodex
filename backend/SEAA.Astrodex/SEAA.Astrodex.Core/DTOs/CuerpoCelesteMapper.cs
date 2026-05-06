// Core/DTOs/CuerpoCelesteMapper.cs
using SEAA.Astrodex.Core.Entities;

namespace SEAA.Astrodex.Core.DTOs
{
    public static class CuerpoCelesteMapper
    {
        public static CuerpoCeleste ToEntity(CuerpoCelesteDto dto)
        {
            return new CuerpoCeleste
            {
                Id = dto.id,
                Nombre = dto.name,
                NombreIngles = dto.englishName,
                TipoCuerpo = dto.bodyType,
                EsPlaneta = dto.isPlanet,
                SemimajorAxis = dto.semimajorAxis,
                Perihelio = dto.perihelion,
                Afelio = dto.aphelion,
                Excentricidad = dto.eccentricity,
                Inclinacion = dto.inclination,
                MasaValor = dto.mass?.massValue ?? 0,
                MasaExponente = dto.mass?.massExponent ?? 0,
                VolumenValor = dto.vol?.volValue ?? 0,
                VolumenExponente = dto.vol?.volExponent ?? 0,
                Densidad = dto.density,
                Gravedad = dto.gravity,
                VelocidadEscape = dto.escape,
                RadioMedio = dto.meanRadius,
                RadioEcuatorial = dto.equaRadius,
                RadioPolar = dto.polarRadius,
                Orbita = dto.sideralOrbit,
                Rotacion = dto.sideralRotation,
                InclinacionAxial = dto.axialTilt,
                TempPromedio = dto.avgTemp,
                DescubridoPor = dto.discoveredBy,
                FechaDescubrimiento = dto.discoveryDate,
                NombreAlternativo = dto.alternativeName,
                Dimension = dto.dimension,
                UrlApi = dto.rel,
                PlanetaPadreId = dto.aroundPlanet?.planet,
                FechaCarga = DateTime.Now
            };
        }

        public static List<CuerpoCeleste> ToEntityList(
            List<CuerpoCelesteDto> dtos)
        {
            return dtos.Select(ToEntity).ToList();
        }
    }
}