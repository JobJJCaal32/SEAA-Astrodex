using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.DTOs
{
    public class RelacionResponseDto
    {
        public string TipoRelacion { get; set; } = string.Empty;
        public double ValorCalculado { get; set; }
        public string UnidadMedida { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool TieneRelacion { get; set; }

        public string CuerpoOrigenId { get; set; } = string.Empty;
        public string CuerpoDestinoId { get; set; } = string.Empty;
        public DateTime FechaConsulta { get; set; }
    }
}
