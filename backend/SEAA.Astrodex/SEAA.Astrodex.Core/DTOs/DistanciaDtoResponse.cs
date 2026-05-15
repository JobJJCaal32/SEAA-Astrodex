using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.DTOs
{
    public class DistanciaDtoResponse
    {
        public string SemimajorAxis { get; set; } = string.Empty;
        public string Perihelio { get; set; } = string.Empty;
        public string Afelio { get; set; } = string.Empty;
        public string Unidad { get; set; } = "km";
        public bool Disponible { get; set; }
    }
}
