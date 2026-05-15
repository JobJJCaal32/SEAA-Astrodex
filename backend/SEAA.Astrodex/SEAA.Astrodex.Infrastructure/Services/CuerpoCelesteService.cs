// Infrastructure/Services/CuerpoCelesteService.cs
using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.DTOs;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;
using SEAA.Astrodex.Infrastructure.Formatters;
using SEAA.Astrodex.Infrastructure.Structures;

namespace SEAA.Astrodex.Infrastructure.Services
{
    public class CuerpoCelesteService : ICuerpoCelesteService
    {
        private readonly ICuerpoCelesteRepository _repository;
        private readonly HistorialMemoriaService _historial;
        private readonly ICaracteristicasFisicasFormatter _formatter;

        public CuerpoCelesteService(
            ICuerpoCelesteRepository repository,
            HistorialMemoriaService historial,
            ICaracteristicasFisicasFormatter formatter)
        {
            _repository = repository;
            _historial = historial;
            _formatter = formatter;
        }

        // Método privado reutilizable
        private async Task<(CuerpoCeleste? cuerpo, string fuente)>
            ObtenerCuerpoConFuenteAsync(string id)
        {
            string fuente;

            if (_repository.BuscarEnMemoria(id) != null)
                fuente = "memoria";
            else if (await _repository.BuscarEnBaseDatosAsync(id) != null)
                fuente = "base de datos";
            else
                fuente = "api";

            var cuerpo = await _repository.ObtenerCuerpoCelesteAsync(id);

            return (cuerpo, fuente);
        }

        // ─── Operación 1 ─────────────────────────────────────────

        public async Task<CuerpoCelesteResponseDto?> ObtenerInformacionAsync(string id)
        {
            var (cuerpo, fuente) = await ObtenerCuerpoConFuenteAsync(id);

            if (cuerpo == null)
                return null;

            _historial.Apilar(cuerpo);
            await _repository.RegistrarHistorialEnBdAsync(
                cuerpo.Id, TiposConsulta.CONSULTA_INFO);

            return CuerpoCelesteResponseMapper.ToResponseDto(cuerpo, fuente);
        }

        // ─── Operación 2 ─────────────────────────────────────────

        public async Task<CaracteristicasFisicasDto?>
            ObtenerCaracteristicasFisicasAsync(string id)
        {
            var (cuerpo, fuente) = await ObtenerCuerpoConFuenteAsync(id);

            if (cuerpo == null)
                return null;

            _historial.Apilar(cuerpo);
            await _repository.RegistrarHistorialEnBdAsync(
                cuerpo.Id, TiposConsulta.CARACTERISTICAS_FISICAS);

            var resultado = _formatter.Formatear(cuerpo);
            resultado.Fuente = fuente;

            return resultado;
        }
    }
}