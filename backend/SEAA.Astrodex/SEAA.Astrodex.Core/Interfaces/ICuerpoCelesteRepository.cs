using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
namespace SEAA.Astrodex.Core.Interfaces
{
    public interface ICuerpoCelesteRepository
    {
        Task<CuerpoCeleste?> BuscarEnBaseDatosPorIdAsync(string id);
        Task GuardarEnBaseDatosAsync(CuerpoCeleste cuerpo);
        Task GuardarLoteEnBaseDatosAsync(List<CuerpoCeleste> cuerpos);
        void CargarEnMemoria(CuerpoCeleste cuerpo);
        CuerpoCeleste? BuscarEnMemoriaPorId(string id);
        List<CuerpoCeleste> ObtenerTodosEnMemoria();
    }
}
