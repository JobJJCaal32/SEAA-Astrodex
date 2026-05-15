using SEAA.Astrodex.Core.DTOs;
using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.Interfaces
{
    public interface ICaracteristicasFisicasFormatter
    {
        CaracteristicasFisicasDto Formatear(CuerpoCeleste cuerpo);
    }
}
