using Microsoft.AspNetCore.Mvc;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelacionesController : ControllerBase
    {
        private readonly IRelacionService _service;

        public RelacionesController(IRelacionService service)
        {
            _service = service;
        }

        // Operación 5: analiza relaciones entre dos cuerpos
        // tipo puede ser: ORBITAL, FISICA, DISTANCIA, TEMPORAL, FAMILIAR, TODOS
        [HttpGet("{origen}/{destino}/{tipo}")]
        public async Task<IActionResult> AnalizarRelacion(
            string origen, string destino, string tipo)
        {
            try
            {
                var resultado = await _service.AnalizarRelacionAsync(
                    origen, destino, tipo);

                if (resultado == null)
                    return NotFound(
                        $"No se pudo analizar la relación entre {origen} y {destino}");

                return Ok(new
                {
                    cuerpoOrigen = origen,
                    cuerpoDestino = destino,
                    tipoSolicitado = tipo,
                    cantidadAnalisis = resultado.Count,
                    relaciones = resultado
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
