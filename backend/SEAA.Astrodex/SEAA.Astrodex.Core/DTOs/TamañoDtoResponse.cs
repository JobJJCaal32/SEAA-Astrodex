using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.DTOs
{
    public class TamañoDtoResponse
    {
        public string RadioMedio { get; set; } = string.Empty;
        public string RadioEcuatorial { get; set; } = string.Empty;
        public string RadioPolar { get; set; } = string.Empty;
        public string Unidad { get; set; } = "km";
        public bool Disponible { get; set; }
    }
}
