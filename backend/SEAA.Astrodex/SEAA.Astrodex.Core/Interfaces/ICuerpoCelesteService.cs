using SEAA.Astrodex.Core.DTOs;
// Core/Interfaces/ICuerpoCelesteService.cs
using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.Interfaces
{
    public interface ICuerpoCelesteService
    {
        Task<CuerpoCelesteResponseDto?> ObtenerInformacionAsync(string id);
        Task<CaracteristicasFisicasDto?> ObtenerCaracteristicasFisicasAsync(string id);
        //Task<List<CuerpoCelesteResponseDto>?> BuscarPorTipoAsync(string tipo, int pagina, int tamanio);
        Task<(List<CuerpoCelesteResponseDto>? cuerpos, string fuente)>BuscarPorTipoConFuenteAsync(string tipo, int pagina, int tamanio);
        Task<SistemaPlanetarioDto?> ObtenerSistemaPlanetarioAsync(string idPlaneta);
    }
}
