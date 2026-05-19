using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SEAA.Astrodex.Core.Entities
{
    [Table("PaginasCargadas")]
    public class PaginaCargada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoCuerpo { get; set; } = string.Empty;

        public int NumeroPagina { get; set; }
        public int TamanioPagina { get; set; }

        public DateTime FechaCarga { get; set; } = DateTime.Now;

        public ICollection<PaginaCargadaCuerpo> Cuerpos { get; set; }
            = new List<PaginaCargadaCuerpo>();
    }
}
