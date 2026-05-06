using Microsoft.AspNetCore.Mvc;
using SEAA.Astrodex.Infrastructure.External;

namespace SEAA.Astrodex.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PlanetsController : Controller
    {
        private readonly SolarSystemApiService _service;

        public PlanetsController(SolarSystemApiService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetPlanetasAsync();
            return Ok(data);
        }
    }
}
