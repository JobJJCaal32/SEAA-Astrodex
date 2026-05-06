// Core/DTOs/CuerpoCelesteDto.cs
namespace SEAA.Astrodex.Core.DTOs
{
    public class CuerpoCelesteDto
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string englishName { get; set; } = string.Empty;
        public string bodyType { get; set; } = string.Empty;
        public bool isPlanet { get; set; }
        public double semimajorAxis { get; set; }
        public double perihelion { get; set; }
        public double aphelion { get; set; }
        public double eccentricity { get; set; }
        public double inclination { get; set; }
        public MasaDto? mass { get; set; }
        public VolumenDto? vol { get; set; }
        public double density { get; set; }
        public double gravity { get; set; }
        public double escape { get; set; }
        public double meanRadius { get; set; }
        public double equaRadius { get; set; }
        public double polarRadius { get; set; }
        public double flattening { get; set; }
        public double sideralOrbit { get; set; }
        public double sideralRotation { get; set; }
        public double axialTilt { get; set; }
        public double avgTemp { get; set; }
        public string discoveredBy { get; set; } = string.Empty;
        public string discoveryDate { get; set; } = string.Empty;
        public string alternativeName { get; set; } = string.Empty;
        public string dimension { get; set; } = string.Empty;
        public string rel { get; set; } = string.Empty;
        public AroundPlanetDto? aroundPlanet { get; set; }
        public List<LunaRefDto>? moons { get; set; }
    }

    public class MasaDto
    {
        public double massValue { get; set; }
        public int massExponent { get; set; }
    }

    public class VolumenDto
    {
        public double volValue { get; set; }
        public int volExponent { get; set; }
    }

    public class AroundPlanetDto
    {
        public string planet { get; set; } = string.Empty;
        public string rel { get; set; } = string.Empty;
    }

    public class LunaRefDto
    {
        public string moon { get; set; } = string.Empty;
        public string rel { get; set; } = string.Empty;
    }

    // Wrapper de la respuesta de la API
    public class ApiResponseDto
    {
        public List<CuerpoCelesteDto> bodies { get; set; }
            = new List<CuerpoCelesteDto>();
    }
}