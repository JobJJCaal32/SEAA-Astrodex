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
    }
}