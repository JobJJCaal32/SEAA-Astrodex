using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.DTOs
{
    public class CaracteristicasFisicasDto
    {
        public string Fuente { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string TipoCuerpo { get; set; } = string.Empty;

        public MasaDtoResponse Masa { get; set; } = new();
        public TamañoDtoResponse Tamaño { get; set; } = new();
        public DistanciaDtoResponse Distancia { get; set; } = new();
        public TemperaturaDtoResponse Temperatura { get; set; } = new();
    }
}
