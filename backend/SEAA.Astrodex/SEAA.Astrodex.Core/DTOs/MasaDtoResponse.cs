using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.DTOs
{
    public class MasaDtoResponse
    {
        public string Valor { get; set; } = string.Empty;
        public string Unidad { get; set; } = "kg";
        public bool Disponible { get; set; }
    }
}
