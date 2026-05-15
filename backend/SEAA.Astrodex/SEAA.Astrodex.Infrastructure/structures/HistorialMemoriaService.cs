// Infrastructure/structures/HistorialMemoriaService.cs
using SEAA.Astrodex.Core.Entities;
using SEAA.Astrodex.Infrastructure.structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Infrastructure.Structures
{
    public class HistorialMemoriaService
    {
        private readonly Pila<CuerpoCeleste> _pilaHistorial;

        public HistorialMemoriaService()
        {
            _pilaHistorial = new Pila<CuerpoCeleste>();
        }

        public void Apilar(CuerpoCeleste cuerpo)
        {
            // Evita apilar el mismo cuerpo dos veces consecutivas
            if (!_pilaHistorial.EstaVacia() &&
                _pilaHistorial.Peek().Id == cuerpo.Id)
                return;

            _pilaHistorial.Push(cuerpo);
        }

        public CuerpoCeleste? VerUltima()
        {
            return _pilaHistorial.EstaVacia() ? null : _pilaHistorial.Peek();
        }

        public List<CuerpoCeleste> ObtenerHistorialCompleto()
        {
            return _pilaHistorial.ObtenerTodo();
        }

        public int TamañoHistorial()
        {
            return _pilaHistorial.Tamaño();
        }

        public void LimpiarHistorial()
        {
            _pilaHistorial.Limpiar();
        }
    }
}
