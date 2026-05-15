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
    }
}
