using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.DTOs
{
    public class SistemaPlanetarioDto
    {
        public string Fuente { get; set; } = string.Empty;
        public CuerpoCelesteResponseDto PlanetaCentral { get; set; } = null!;
        public int CantidadSatelites { get; set; }
        public List<CuerpoCelesteResponseDto> Satelites { get; set; } = new();
    }
}
