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
        private readonly CachePaginas _cachePaginas;

        public CuerpoCelesteService(
            ICuerpoCelesteRepository repository,
            HistorialMemoriaService historial,
            ICaracteristicasFisicasFormatter formatter,
            CachePaginas cachePaginas)
        {
            _repository = repository;
            _historial = historial;
            _formatter = formatter;
            _cachePaginas = cachePaginas;
        }

        // Método privado reutilizable
        private async Task<(CuerpoCeleste? cuerpo, string fuente)>
            ObtenerCuerpoConFuenteAsync(string idONombre)
        {
            string fuente;

            if (_repository.BuscarEnMemoria(idONombre) != null)
                fuente = "memoria";
            else if (await _repository.BuscarEnBaseDatosAsync(idONombre) != null)
                fuente = "base de datos";
            else
                fuente = "api";

            var cuerpo = await _repository.ObtenerCuerpoCelesteAsync(idONombre);

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
        // Operación 4: busca cuerpos por tipo con paginación
        // Niveles: caché en memoria → BD con tabla intermedia → API
        public async Task<(List<CuerpoCelesteResponseDto>? cuerpos, string fuente)>
            BuscarPorTipoConFuenteAsync(string tipo, int pagina, int tamanio)
        {
            if (string.IsNullOrWhiteSpace(tipo) || pagina < 1 || tamanio < 1)
                return (null, "");

            string fuente;
            List<CuerpoCeleste> cuerpos;

            // Nivel 3: caché en memoria
            var clave = CachePaginas.GenerarClave(tipo, pagina, tamanio);
            var enCache = _cachePaginas.Obtener(clave);

            if (enCache != null)
            {
                cuerpos = enCache;
                fuente = "memoria";
            }
            else
            {
                // Nivel 2: BD con tabla intermedia
                var enBd = await _repository.ObtenerPaginaCargadaAsync(tipo, pagina, tamanio);

                if (enBd != null && enBd.Any())
                {
                    cuerpos = enBd;
                    fuente = "base de datos";
                    _cachePaginas.Agregar(clave, cuerpos);
                }
                else
                {
                    // Nivel 1: API externa
                    cuerpos = await _repository.BuscarPaginaEnApiAsync(tipo, pagina, tamanio);

                    if (!cuerpos.Any())
                        return (null, "");

                    await _repository.GuardarLoteEnBaseDatosAsync(cuerpos);
                    await _repository.RegistrarPaginaCargadaAsync(tipo, pagina, tamanio, cuerpos);

                    foreach (var c in cuerpos)
                        _repository.CargarEnMemoria(c);

                    _cachePaginas.Agregar(clave, cuerpos);
                    fuente = "api";
                }
            }

            var respuesta = cuerpos
                .Select(c => CuerpoCelesteResponseMapper.ToResponseDto(c, fuente))
                .ToList();

            return (respuesta, fuente);
        }
    }
}