using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEAA.Astrodex.Core.Entities
{
    [Table("RelacionesCelestes")]
    public class RelacionCeleste
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TipoRelacion { get; set; }  // ORBITAL | FISICA | DISTANCIA | TEMPORAL | FAMILIAR
        public double ValorCalculado { get; set; }
        public string UnidadMedida { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaConsulta { get; set; }

        // Relaciones
        [Required]
        [MaxLength(100)]
        public string CuerpoOrigenId { get; set; } = string.Empty;

        [ForeignKey("CuerpoOrigenId")]
        public CuerpoCeleste CuerpoOrigen { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string CuerpoDestinoId { get; set; } = string.Empty;

        [ForeignKey("CuerpoDestinoId")]
        public CuerpoCeleste CuerpoDestino { get; set; } = null!;
    }
}