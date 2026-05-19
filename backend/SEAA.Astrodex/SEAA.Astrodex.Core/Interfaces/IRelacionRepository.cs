using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.Interfaces
{
    public interface IRelacionRepository
    {
        Task GuardarRelacionAsync(RelacionCeleste relacion);
        Task GuardarRelacionesAsync(List<RelacionCeleste> relaciones);
    }
}
