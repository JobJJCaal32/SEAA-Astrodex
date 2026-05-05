using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SEAA.Astrodex.Core.Entities
{
    [Table("CuerposCelestes")]
    public class CuerpoCeleste
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string NombreIngles { get; set; }
        public string TipoCuerpo { get; set; }
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
        public string DescubridoPor { get; set; }
        public string FechaDescubrimiento { get; set; }
        public string NombreAlternativo { get; set; }
        public string Dimension { get; set; }
        public string UrlApi { get; set; }
        public DateTime FechaCarga { get; set; }

        // Auto-referencia planeta padre
        [MaxLength(100)]
        public string? PlanetaPadreId { get; set; }

        [ForeignKey("PlanetaPadreId")]
        public CuerpoCeleste? PlanetaPadre { get; set; }

        // Propiedades de navegacion
        public ICollection<CuerpoCeleste> Lunas { get; set; }
            = new List<CuerpoCeleste>();

        public ICollection<LunaRef> LunasRef { get; set; }
            = new List<LunaRef>();

        public ICollection<RelacionCeleste> RelacionesOrigen { get; set; }
            = new List<RelacionCeleste>();

        public ICollection<RelacionCeleste> RelacionesDestino { get; set; }
            = new List<RelacionCeleste>();

        public ICollection<HistorialConsulta> Historial { get; set; }
            = new List<HistorialConsulta>();
    }
}
