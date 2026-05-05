using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Infrastructure.structures
{
    // DataStructures/Cola.cs

    public class Cola<T>
    {
        // Nodo interno de la cola
        private class Nodo
        {
            public T Valor { get; set; }
            public Nodo? Siguiente { get; set; }

            public Nodo(T valor)
            {
                Valor = valor;
                Siguiente = null;
            }
        }

        private Nodo? _frente;
        private Nodo? _final;
        private int _tamaño;

        public Cola()
        {
            _frente = null;
            _final = null;
            _tamaño = 0;
        }

        // Agrega un elemento al final de la cola
        public void Encolar(T valor)
        {
            Nodo nuevoNodo = new Nodo(valor);

            if (EstaVacia())
            {
                _frente = nuevoNodo;
                _final = nuevoNodo;
            }
            else
            {
                _final!.Siguiente = nuevoNodo;
                _final = nuevoNodo;
            }

            _tamaño++;
        }

        // Saca y retorna el primer elemento de la cola
        public T Desencolar()
        {
            if (EstaVacia())
                throw new InvalidOperationException("La cola está vacía.");

            T valor = _frente!.Valor;
            _frente = _frente.Siguiente;

            if (_frente == null)
                _final = null;

            _tamaño--;
            return valor;
        }

        // Retorna el primer elemento sin sacarlo
        public T VerPrimero()
        {
            if (EstaVacia())
                throw new InvalidOperationException("La cola está vacía.");

            return _frente!.Valor;
        }

        // Retorna true si la cola no tiene elementos
        public bool EstaVacia()
        {
            return _frente == null;
        }

        // Retorna cuántos elementos están pendientes
        public int Tamaño()
        {
            return _tamaño;
        }

        // Agrega una lista completa de elementos a la cola
        public void EnColarLote(List<T> elementos)
        {
            foreach (T elemento in elementos)
            {
                Encolar(elemento);
            }
        }

        // Verifica si un elemento ya está en la cola
        // usando una condición personalizada
        public bool ContieneCuerpo(Func<T, bool> condicion)
        {
            Nodo? actual = _frente;

            while (actual != null)
            {
                if (condicion(actual.Valor))
                    return true;

                actual = actual.Siguiente;
            }

            return false;
        }

        // Desencola todos los elementos y ejecuta una acción
        // por cada uno, retorna cuántos procesó
        public int ProcesarTodo(Action<T> accion)
        {
            int procesados = 0;

            while (!EstaVacia())
            {
                T valor = Desencolar();
                accion(valor);
                procesados++;
            }

            return procesados;
        }

        // Vacía la cola completamente
        public void Limpiar()
        {
            _frente = null;
            _final = null;
            _tamaño = 0;
        }
    }
}
