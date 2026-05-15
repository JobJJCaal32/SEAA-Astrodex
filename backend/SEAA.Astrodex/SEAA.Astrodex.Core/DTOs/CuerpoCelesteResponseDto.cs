using System;
using System.Collections.Generic;
using System.Text;

// Core/DTOs/CuerpoCelesteResponseDto.cs
namespace SEAA.Astrodex.Core.DTOs
{
    public class CuerpoCelesteResponseDto
    {
        public string Fuente { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string NombreIngles { get; set; } = string.Empty;
        public string TipoCuerpo { get; set; } = string.Empty;
        public bool EsPlaneta { get; set; }

        public double SemimajorAxis { get; set; }
        public double Perihelio { get; set; }
        public double Afelio { get; set; }
        public double Excentricidad { get; set; }
        public double Inclinacion { get; set; }

        public double MasaValor { get; set; }
        public int MasaExponente { get; set; }
        public double VolumenValor { get; set; }
        public int VolumenExponente { get; set; }

        public double Densidad { get; set; }
        public double Gravedad { get; set; }
        public double VelocidadEscape { get; set; }

        public double RadioMedio { get; set; }
        public double RadioEcuatorial { get; set; }
        public double RadioPolar { get; set; }

        public double Orbita { get; set; }
        public double Rotacion { get; set; }
        public double InclinacionAxial { get; set; }
        public double TempPromedio { get; set; }

        public string DescubridoPor { get; set; } = string.Empty;
        public string FechaDescubrimiento { get; set; } = string.Empty;
        public string NombreAlternativo { get; set; } = string.Empty;
        public string Dimension { get; set; } = string.Empty;
        public string UrlApi { get; set; } = string.Empty;

        public string? PlanetaPadreId { get; set; }
        public DateTime FechaCarga { get; set; }
    }
}
