using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.DTOs
{
    public class PlanetDto
    {
        public string id { get; set; }
        public string englishName { get; set; }
        public bool isPlanet { get; set; }
        public double gravity { get; set; }
    }
}
