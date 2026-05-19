// Core/Interfaces/ICuerpoCelesteRepository.cs
using SEAA.Astrodex.Core.Entities;

namespace SEAA.Astrodex.Core.Interfaces
{
    public interface ICuerpoCelesteRepository
    {
        // Cache-Aside completo
        Task<CuerpoCeleste?> ObtenerCuerpoCelesteAsync(string idONombre);

        // Métodos individuales del Cache-Aside
        CuerpoCeleste? BuscarEnMemoria(string idONombre);
        Task<CuerpoCeleste?> BuscarEnBaseDatosAsync(string idONombre);
        Task<CuerpoCeleste?> BuscarEnApiAsync(string idONombre);

        // Persistencia
        Task GuardarEnBaseDatosAsync(CuerpoCeleste cuerpo);
        Task GuardarLoteEnBaseDatosAsync(List<CuerpoCeleste> cuerpos);
        void CargarEnMemoria(CuerpoCeleste cuerpo);

        // Historial
        Task RegistrarHistorialEnBdAsync(string cuerpoId, string tipoConsulta);
        // Búsqueda por planeta padre (Operación 3)
        Task<List<CuerpoCeleste>> BuscarPorPlanetaPadreAsync(string idPlaneta);
        // Operación 4
        Task<List<CuerpoCeleste>?> ObtenerPaginaCargadaAsync(
            string tipo, int pagina, int tamanio);
        Task<List<CuerpoCeleste>> BuscarPaginaEnApiAsync(
            string tipo, int pagina, int tamanio);
        Task RegistrarPaginaCargadaAsync(
            string tipo, int pagina, int tamanio, List<CuerpoCeleste> cuerpos);
    }
}