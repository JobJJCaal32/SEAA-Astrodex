using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
// Core/Interfaces/ICuerpoCelesteRepository.cs
using SEAA.Astrodex.Core.Entities;

namespace SEAA.Astrodex.Core.Interfaces
{
    public interface ICuerpoCelesteRepository
    {
        // Cache-Aside completo - método central
        Task<CuerpoCeleste?> ObtenerCuerpoCelesteAsync(string id);

        // Métodos individuales del Cache-Aside
        CuerpoCeleste? BuscarEnMemoria(string id);
        Task<CuerpoCeleste?> BuscarEnBaseDatosAsync(string id);
        Task<CuerpoCeleste?> BuscarEnApiAsync(string id);

        // Persistencia
        Task GuardarEnBaseDatosAsync(CuerpoCeleste cuerpo);
        void CargarEnMemoria(CuerpoCeleste cuerpo);

        // Historial en BD
        Task RegistrarHistorialEnBdAsync(string cuerpoId, string tipoConsulta);
    }
}
