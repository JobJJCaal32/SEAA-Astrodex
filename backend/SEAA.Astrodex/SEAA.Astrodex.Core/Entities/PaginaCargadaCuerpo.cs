using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SEAA.Astrodex.Core.Entities
{
    [Table("PaginasCargadasCuerpos")]
    public class PaginaCargadaCuerpo
    {
        public int PaginaCargadaId { get; set; }
        public PaginaCargada PaginaCargada { get; set; } = null!;

        [MaxLength(100)]
        public string CuerpoCelesteId { get; set; } = string.Empty;
        public CuerpoCeleste CuerpoCeleste { get; set; } = null!;
    }
}
