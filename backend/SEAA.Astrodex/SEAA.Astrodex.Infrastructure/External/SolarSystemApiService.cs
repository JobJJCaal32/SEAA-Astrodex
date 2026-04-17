using SEAA.Astrodex.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SEAA.Astrodex.Infrastructure.External
{
    public class SolarSystemApiService
    {
        private readonly HttpClient _httpClient;

        public SolarSystemApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PlanetDto>> GetPlanetsAsync()
        {
            var response = await _httpClient.GetAsync("https://api.le-systeme-solaire.net/rest/bodies/");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApiResponse>(json);

            return result.bodies.Where(x => x.isPlanet).ToList();
        }

    }
    public class ApiResponse
    {
        public List<PlanetDto> bodies { get; set; }
    }
}
