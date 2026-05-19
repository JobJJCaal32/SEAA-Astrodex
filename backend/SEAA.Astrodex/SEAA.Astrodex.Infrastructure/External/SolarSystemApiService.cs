// Infrastructure/External/SolarSystemApiService.cs
using SEAA.Astrodex.Core.DTOs;
using System.Text.Json;

namespace SEAA.Astrodex.Infrastructure.External
{
    public class SolarSystemApiService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public SolarSystemApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Trae todos los cuerpos celestes sin filtro
        public async Task<List<CuerpoCelesteDto>> GetTodosLosCuerposAsync()
        {
            var response = await _httpClient
                .GetAsync("rest/bodies/");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiResponseDto>(json, _jsonOptions);

            return result?.bodies ?? new List<CuerpoCelesteDto>();
        }

        // Trae solo planetas
        public async Task<List<CuerpoCelesteDto>> GetPlanetasAsync()
        {
            var todos = await GetTodosLosCuerposAsync();
            return todos.Where(x => x.isPlanet).ToList();
        }

        // Trae un cuerpo por su id
        public async Task<CuerpoCelesteDto?> GetCuerpoPorIdAsync(string id)
        {
            var response = await _httpClient
                .GetAsync($"rest/bodies/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CuerpoCelesteDto>(json, _jsonOptions);
        }

        // Trae cuerpos por filtro con orden y paginación opcionales
        // campo, operador, valor → filtro de la API
        // orden     → ejemplo: "englishName,asc"
        // pagina    → número de página
        // tamanio   → tamaño de página
        public async Task<List<CuerpoCelesteDto>> GetCuerposPorFiltroAsync(
            string campo, string operador, string valor,
            string? orden = null, int? pagina = null, int? tamanio = null)
        {
            var url = $"rest/bodies/?filter[]={campo},{operador},{valor}";

            if (!string.IsNullOrEmpty(orden))
                url += $"&order={orden}";

            if (pagina.HasValue && tamanio.HasValue)
                url += $"&page={pagina},{tamanio}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<CuerpoCelesteDto>();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponseDto>(json, _jsonOptions);

            return result?.bodies ?? new List<CuerpoCelesteDto>();
        }

    }
}