using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEAA.Astrodex.Core.Entities
{
    [Table("HistorialConsultas")]
    public class HistorialConsulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TipoConsulta { get; set; }
        public DateTime FechaConsulta { get; set; }

        // Relacion
        [Required]
        [MaxLength(100)]
        public string CuerpoCelesteId { get; set; } = string.Empty;

        [ForeignKey("CuerpoCelesteId")]
        public CuerpoCeleste CuerpoCeleste { get; set; } = null!;
    }
}