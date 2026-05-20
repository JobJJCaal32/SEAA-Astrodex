using System;
using System.Collections.Generic;
using System.Text;

// Infrastructure/Strategies/EstrategiaDistancia.cs
using SEAA.Astrodex.Core.Constants;
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Core.Interfaces;

namespace SEAA.Astrodex.Infrastructure.Strategies
{
    // Calcula la distancia entre dos cuerpos celestes considerando
    // la jerarquía orbital (padre-hijo, hermanos, no relacionados)
    public class EstrategiaDistancia : IEstrategiaRelacion
    {
        private readonly ICuerpoCelesteRepository _repository;

        public EstrategiaDistancia(ICuerpoCelesteRepository repository)
        {
            _repository = repository;
        }

        public string Tipo => TiposRelacion.DISTANCIA;

        public RelacionCeleste Ejecutar(
            CuerpoCeleste origen, CuerpoCeleste destino)
        {
            var relacion = new RelacionCeleste
            {
                TipoRelacion = Tipo,
                UnidadMedida = "km",
                CuerpoOrigenId = origen.Id,
                CuerpoDestinoId = destino.Id,
                FechaConsulta = DateTime.Now
            };

            var origenTienePadre = !string.IsNullOrEmpty(origen.PlanetaPadreId);
            var destinoTienePadre = !string.IsNullOrEmpty(destino.PlanetaPadreId);

            // Caso 1: ambos orbitan al Sol (planeta vs planeta)
            if (!origenTienePadre && !destinoTienePadre)
            {
                return CalcularEntrePlanetas(origen, destino, relacion);
            }

            // Caso 2: origen es planeta, destino es satélite
            if (!origenTienePadre && destinoTienePadre)
            {
                // Subcaso: el satélite orbita al planeta consultado
                if (destino.PlanetaPadreId!.Equals(origen.Id,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return CalcularPadreHijo(origen, destino, relacion);
                }

                // Subcaso: el satélite orbita a otro planeta
                var padreDestino = _repository
                    .BuscarEnBaseDatosAsync(destino.PlanetaPadreId).Result;

                if (padreDestino == null)
                {
                    relacion.ValorCalculado = 0;
                    relacion.Descripcion =
                        $"No se pudo calcular: planeta padre de " +
                        $"{destino.NombreIngles} no encontrado";
                    return relacion;
                }

                return CalcularPlanetaConSatelite(origen, destino, padreDestino, relacion);
            }

            // Caso 3: origen es satélite, destino es planeta
            if (origenTienePadre && !destinoTienePadre)
            {
                // Subcaso: el satélite orbita al planeta consultado
                if (origen.PlanetaPadreId!.Equals(destino.Id,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return CalcularPadreHijo(destino, origen, relacion);
                }

                // Subcaso: el satélite orbita a otro planeta
                var padreOrigen = _repository
                    .BuscarEnBaseDatosAsync(origen.PlanetaPadreId).Result;

                if (padreOrigen == null)
                {
                    relacion.ValorCalculado = 0;
                    relacion.Descripcion =
                        $"No se pudo calcular: planeta padre de " +
                        $"{origen.NombreIngles} no encontrado";
                    return relacion;
                }

                return CalcularPlanetaConSatelite(destino, origen, padreOrigen, relacion);
            }

            // Caso 4: ambos son satélites
            if (origenTienePadre && destinoTienePadre)
            {
                // Subcaso: comparten el mismo padre (hermanos)
                if (origen.PlanetaPadreId!.Equals(destino.PlanetaPadreId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return CalcularHermanos(origen, destino, relacion);
                }

                // Subcaso: padres distintos
                var padreOrigen = _repository
                    .BuscarEnBaseDatosAsync(origen.PlanetaPadreId).Result;
                var padreDestino = _repository
                    .BuscarEnBaseDatosAsync(destino.PlanetaPadreId!).Result;

                if (padreOrigen == null || padreDestino == null)
                {
                    relacion.ValorCalculado = 0;
                    relacion.Descripcion =
                        "No se pudo calcular: planetas padres no encontrados";
                    return relacion;
                }

                return CalcularSatelitesDePlanetasDistintos(
                    origen, destino, padreOrigen, padreDestino, relacion);
            }

            relacion.ValorCalculado = 0;
            relacion.Descripcion = "No se pudo determinar la relación";
            return relacion;
        }

        // ─── Caso 1: dos planetas orbitando al Sol ───────────────

        private RelacionCeleste CalcularEntrePlanetas(
            CuerpoCeleste a, CuerpoCeleste b, RelacionCeleste rel)
        {
            if (a.SemimajorAxis <= 0 || b.SemimajorAxis <= 0)
            {
                rel.ValorCalculado = 0;
                rel.Descripcion = "Datos insuficientes para calcular distancia";
                return rel;
            }

            // Lados opuestos del Sol → máxima
            var max = a.Afelio + b.Afelio;

            // Mismo lado del Sol → mínima
            var min = Math.Abs(a.Perihelio - b.Perihelio);

            var promedio = (min + max) / 2;

            rel.ValorCalculado = Math.Round(promedio, 2);
            rel.Descripcion =
                $"{a.NombreIngles} y {b.NombreIngles} orbitan al Sol. " +
                $"Distancia promedio: {promedio:N0} km. " +
                $"Mínima: {min:N0} km. " +
                $"Máxima: {max:N0} km";
            return rel;
        }

        // ─── Caso 2: planeta y su satélite directo ───────────────

        private RelacionCeleste CalcularPadreHijo(
            CuerpoCeleste padre, CuerpoCeleste hijo, RelacionCeleste rel)
        {
            if (hijo.SemimajorAxis <= 0)
            {
                rel.ValorCalculado = 0;
                rel.Descripcion = "Datos insuficientes para calcular distancia";
                return rel;
            }

            rel.ValorCalculado = Math.Round(hijo.SemimajorAxis, 2);
            rel.Descripcion =
                $"{hijo.NombreIngles} orbita a {padre.NombreIngles} " +
                $"a una distancia promedio de {hijo.SemimajorAxis:N0} km. " +
                $"Mínima: {hijo.Perihelio:N0} km. " +
                $"Máxima: {hijo.Afelio:N0} km";
            return rel;
        }

        // ─── Caso 3: hermanos (mismo padre) ──────────────────────

        private RelacionCeleste CalcularHermanos(
            CuerpoCeleste a, CuerpoCeleste b, RelacionCeleste rel)
        {
            if (a.SemimajorAxis <= 0 || b.SemimajorAxis <= 0)
            {
                rel.ValorCalculado = 0;
                rel.Descripcion = "Datos insuficientes para calcular distancia";
                return rel;
            }

            var max = a.Afelio + b.Afelio;
            var min = Math.Abs(a.Perihelio - b.Perihelio);
            var promedio = (min + max) / 2;

            rel.ValorCalculado = Math.Round(promedio, 2);
            rel.Descripcion =
                $"{a.NombreIngles} y {b.NombreIngles} orbitan a {a.PlanetaPadreId}. " +
                $"Distancia promedio: {promedio:N0} km. " +
                $"Mínima: {min:N0} km. " +
                $"Máxima: {max:N0} km";
            return rel;
        }

        // ─── Caso 4: planeta con satélite de otro planeta ────────

        private RelacionCeleste CalcularPlanetaConSatelite(
            CuerpoCeleste planeta, CuerpoCeleste satelite,
            CuerpoCeleste padreSatelite, RelacionCeleste rel)
        {
            if (planeta.SemimajorAxis <= 0 ||
                padreSatelite.SemimajorAxis <= 0 ||
                satelite.SemimajorAxis <= 0)
            {
                rel.ValorCalculado = 0;
                rel.Descripcion = "Datos insuficientes para calcular distancia";
                return rel;
            }

            // Máximo: planetas en lados opuestos del Sol
            // + satélite en su punto más lejano de su planeta
            var max = planeta.Afelio + padreSatelite.Afelio + satelite.Afelio;

            // Mínimo: planetas alineados mismo lado del Sol
            // - satélite acerca su distancia al planeta consultado
            var min = Math.Abs(planeta.Perihelio - padreSatelite.Perihelio)
                    - satelite.Perihelio;

            if (min < 0) min = 0;

            var promedio = (min + max) / 2;

            rel.ValorCalculado = Math.Round(promedio, 2);
            rel.Descripcion =
                $"{satelite.NombreIngles} orbita a {padreSatelite.NombreIngles}. " +
                $"Distancia aproximada con {planeta.NombreIngles}: " +
                $"promedio {promedio:N0} km, " +
                $"mínima {min:N0} km, " +
                $"máxima {max:N0} km";
            return rel;
        }

        // ─── Caso 5: satélites de planetas distintos ─────────────

        private RelacionCeleste CalcularSatelitesDePlanetasDistintos(
            CuerpoCeleste a, CuerpoCeleste b,
            CuerpoCeleste padreA, CuerpoCeleste padreB,
            RelacionCeleste rel)
        {
            if (a.SemimajorAxis <= 0 || b.SemimajorAxis <= 0 ||
                padreA.SemimajorAxis <= 0 || padreB.SemimajorAxis <= 0)
            {
                rel.ValorCalculado = 0;
                rel.Descripcion = "Datos insuficientes para calcular distancia";
                return rel;
            }

            // Máximo: ambos planetas en lados opuestos del Sol
            // + ambos satélites en sus puntos más lejanos
            var max = padreA.Afelio + padreB.Afelio + a.Afelio + b.Afelio;

            // Mínimo: planetas alineados al mismo lado
            // - ambos satélites se "acercan" entre sí
            var min = Math.Abs(padreA.Perihelio - padreB.Perihelio)
                    - a.Perihelio - b.Perihelio;

            if (min < 0) min = 0;

            var promedio = (min + max) / 2;

            rel.ValorCalculado = Math.Round(promedio, 2);
            rel.Descripcion =
                $"{a.NombreIngles} orbita a {padreA.NombreIngles} y " +
                $"{b.NombreIngles} orbita a {padreB.NombreIngles}. " +
                $"Distancia aproximada: " +
                $"promedio {promedio:N0} km, " +
                $"mínima {min:N0} km, " +
                $"máxima {max:N0} km";
            return rel;
        }
    }
}