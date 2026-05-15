using SEAA.Astrodex.Core.DTOs;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Infrastructure.Formatters
{
    public class CaracteristicasFisicasFormatter : ICaracteristicasFisicasFormatter
    {
        public CaracteristicasFisicasDto Formatear(CuerpoCeleste cuerpo)
        {
            return new CaracteristicasFisicasDto
            {
                Id = cuerpo.Id,
                Nombre = cuerpo.Nombre,
                TipoCuerpo = cuerpo.TipoCuerpo,
                Masa = ExtraerMasa(cuerpo),
                Tamaño = ExtraerTamaño(cuerpo),
                Distancia = ExtraerDistancia(cuerpo),
                Temperatura = ExtraerTemperatura(cuerpo)
            };
        }

        private MasaDtoResponse ExtraerMasa(CuerpoCeleste cuerpo)
        {
            if (cuerpo.MasaValor == 0)
                return new MasaDtoResponse
                {
                    Valor = "Dato no disponible",
                    Disponible = false
                };

            return new MasaDtoResponse
            {
                Valor = $"{cuerpo.MasaValor} × 10^{cuerpo.MasaExponente}",
                Disponible = true
            };
        }

        private TamañoDtoResponse ExtraerTamaño(CuerpoCeleste cuerpo)
        {
            if (cuerpo.RadioMedio == 0 &&
                cuerpo.RadioEcuatorial == 0 &&
                cuerpo.RadioPolar == 0)
            {
                return new TamañoDtoResponse
                {
                    RadioMedio = "Dato no disponible",
                    RadioEcuatorial = "Dato no disponible",
                    RadioPolar = "Dato no disponible",
                    Disponible = false
                };
            }

            return new TamañoDtoResponse
            {
                RadioMedio = cuerpo.RadioMedio == 0
                    ? "Dato no disponible"
                    : cuerpo.RadioMedio.ToString(),
                RadioEcuatorial = cuerpo.RadioEcuatorial == 0
                    ? "Dato no disponible"
                    : cuerpo.RadioEcuatorial.ToString(),
                RadioPolar = cuerpo.RadioPolar == 0
                    ? "Dato no disponible"
                    : cuerpo.RadioPolar.ToString(),
                Disponible = true
            };
        }

        private DistanciaDtoResponse ExtraerDistancia(CuerpoCeleste cuerpo)
        {
            if (cuerpo.SemimajorAxis == 0 &&
                cuerpo.Perihelio == 0 &&
                cuerpo.Afelio == 0)
            {
                return new DistanciaDtoResponse
                {
                    SemimajorAxis = "Dato no disponible",
                    Perihelio = "Dato no disponible",
                    Afelio = "Dato no disponible",
                    Disponible = false
                };
            }

            return new DistanciaDtoResponse
            {
                SemimajorAxis = cuerpo.SemimajorAxis == 0
                    ? "Dato no disponible"
                    : cuerpo.SemimajorAxis.ToString(),
                Perihelio = cuerpo.Perihelio == 0
                    ? "Dato no disponible"
                    : cuerpo.Perihelio.ToString(),
                Afelio = cuerpo.Afelio == 0
                    ? "Dato no disponible"
                    : cuerpo.Afelio.ToString(),
                Disponible = true
            };
        }

        private TemperaturaDtoResponse ExtraerTemperatura(CuerpoCeleste cuerpo)
        {
            if (cuerpo.TempPromedio == 0)
                return new TemperaturaDtoResponse
                {
                    Valor = "Dato no disponible",
                    Disponible = false
                };

            return new TemperaturaDtoResponse
            {
                Valor = cuerpo.TempPromedio.ToString(),
                Disponible = true
            };
        }
    }
}
