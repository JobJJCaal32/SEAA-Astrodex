// API/Controllers/CuerposCelestesController.cs
using Microsoft.AspNetCore.Mvc;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuerposCelestesController : ControllerBase
    {
        private readonly ICuerpoCelesteService _service;

        public CuerposCelestesController(ICuerpoCelesteService service)
        {
            _service = service;
        }

        // Operación 1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerInformacion(string id)
        {
            try
            {
                var resultado = await _service.ObtenerInformacionAsync(id);

                if (resultado == null)
                    return NotFound($"No se encontró el cuerpo con id: {id}");

                return Ok(resultado);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "No se pudo conectar con la API externa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // Operación 2
        [HttpGet("{id}/caracteristicas-fisicas")]
        public async Task<IActionResult> ObtenerCaracteristicasFisicas(string id)
        {
            try
            {
                var resultado = await _service
                    .ObtenerCaracteristicasFisicasAsync(id);

                if (resultado == null)
                    return NotFound($"No se encontró el cuerpo con id: {id}");

                return Ok(resultado);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "No se pudo conectar con la API externa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // Operación 3: obtiene el sistema planetario completo
        [HttpGet("{id}/sistema-planetario")]
        public async Task<IActionResult> ObtenerSistemaPlanetario(string id)
        {
            try
            {
                var resultado = await _service.ObtenerSistemaPlanetarioAsync(id);

                if (resultado == null)
                    return NotFound($"No se encontró un planeta con id: {id}");

                return Ok(resultado);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "No se pudo conectar con la API externa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // Operación 4: busca cuerpos por tipo con paginación
        [HttpGet("tipo/{tipo}")]
        public async Task<IActionResult> BuscarPorTipo(
            string tipo,
            [FromQuery] int pagina,
            [FromQuery] int tamanio)
        {
            if (pagina < 1 || tamanio < 1)
                return BadRequest("pagina y tamanio deben ser mayores a 0");

            try
            {
                var (cuerpos, fuente) = await _service
                    .BuscarPorTipoConFuenteAsync(tipo, pagina, tamanio);

                if (cuerpos == null || !cuerpos.Any())
                    return NotFound($"Página no encontrada para el tipo: {tipo}");

                return Ok(new
                {
                    fuente,
                    tipo,
                    pagina,
                    tamanio,
                    cuerpos
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "No se pudo conectar con la API externa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}