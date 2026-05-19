using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.Interfaces
{
    public interface IEstrategiaRelacion
    {
        string Tipo { get; }
        RelacionCeleste Ejecutar(CuerpoCeleste origen, CuerpoCeleste destino);
    }
}
