using SEAA.Astrodex.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Core.Interfaces
{
    public interface IRelacionService
    {
        Task<List<RelacionResponseDto>?> AnalizarRelacionAsync(
            string idOrigen, string idDestino, string tipoRelacion);
    }
}