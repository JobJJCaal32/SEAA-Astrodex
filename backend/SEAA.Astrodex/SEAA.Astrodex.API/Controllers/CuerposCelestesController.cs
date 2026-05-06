// API/Controllers/CuerposCelestesController.cs
using Microsoft.AspNetCore.Mvc;
using SEAA.Astrodex.Core.DTOs;
using SEAA.Astrodex.Core.Interfaces;
using SEAA.Astrodex.Infrastructure.External;

namespace SEAA.Astrodex.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuerposCelestesController : ControllerBase
    {
        private readonly SolarSystemApiService _apiService;
        private readonly ICuerpoCelesteRepository _repository;

        public CuerposCelestesController(
            SolarSystemApiService apiService,
            ICuerpoCelesteRepository repository)
        {
            _apiService = apiService;
            _repository = repository;
        }

        // Trae todos los cuerpos de la API,
        // los guarda en BD y los carga en memoria
        [HttpPost("cargar")]
        public async Task<IActionResult> CargarDesdApi()
        {
            var dtos = await _apiService.GetTodosLosCuerposAsync();

            if (dtos == null || !dtos.Any())
                return NotFound("No se obtuvieron datos de la API.");

            var entidades = CuerpoCelesteMapper.ToEntityList(dtos);

            await _repository.GuardarLoteEnBaseDatosAsync(entidades);

            foreach (var entidad in entidades)
                _repository.CargarEnMemoria(entidad);

            return Ok(new
            {
                mensaje = "Datos cargados correctamente.",
                totalApi = dtos.Count,
                totalGuardados = entidades.Count
            });
        }

        // Busca un cuerpo por id siguiendo Cache-Aside
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPorId(string id)
        {
            // 1. Busca en memoria
            var enMemoria = _repository.BuscarEnMemoriaPorId(id);
            if (enMemoria != null)
                return Ok(new { fuente = "memoria", data = enMemoria });

            // 2. Busca en base de datos
            var enBd = await _repository.BuscarEnBaseDatosPorIdAsync(id);
            if (enBd != null)
            {
                _repository.CargarEnMemoria(enBd);
                return Ok(new { fuente = "base de datos", data = enBd });
            }

            // 3. Busca en la API
            var dto = await _apiService.GetCuerpoPorIdAsync(id);
            if (dto == null)
                return NotFound($"No se encontró el cuerpo con id: {id}");

            var entidad = CuerpoCelesteMapper.ToEntity(dto);
            await _repository.GuardarEnBaseDatosAsync(entidad);
            _repository.CargarEnMemoria(entidad);

            return Ok(new { fuente = "api", data = entidad });
        }

        // Retorna todos los cuerpos que están en memoria
        [HttpGet("memoria")]
        public IActionResult GetEnMemoria()
        {
            var enMemoria = _repository.ObtenerTodosEnMemoria();

            if (!enMemoria.Any())
                return Ok(new
                {
                    mensaje = "No hay datos en memoria. Llama primero a /cargar",
                    total = 0
                });

            return Ok(new
            {
                mensaje = "Datos obtenidos desde memoria.",
                total = enMemoria.Count,
                data = enMemoria
            });
        }

        // Trae solo planetas
        [HttpGet("planetas")]
        public async Task<IActionResult> GetPlanetas()
        {
            var enMemoria = _repository.ObtenerTodosEnMemoria()
                .Where(c => c.EsPlaneta)
                .ToList();

            if (enMemoria.Any())
                return Ok(new
                {
                    fuente = "memoria",
                    total = enMemoria.Count,
                    data = enMemoria
                });

            var dtos = await _apiService.GetPlanetasAsync();
            var entidades = CuerpoCelesteMapper.ToEntityList(dtos);

            await _repository.GuardarLoteEnBaseDatosAsync(entidades);
            foreach (var entidad in entidades)
                _repository.CargarEnMemoria(entidad);

            return Ok(new
            {
                fuente = "api",
                total = entidades.Count,
                data = entidades
            });
        }
    }
}
