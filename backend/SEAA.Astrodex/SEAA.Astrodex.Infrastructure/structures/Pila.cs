using System;
using System.Collections.Generic;
using System.Text;

namespace SEAA.Astrodex.Infrastructure.structures
{
    // DataStructures/Pila.cs

public class Pila<T>
{
    // Nodo interno de la pila
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

    private Nodo? _tope;
    private int _tamaño;

    public Pila()
    {
        _tope = null;
        _tamaño = 0;
    }

    // Agrega un elemento al tope de la pila
    public void Push(T valor)
    {
        Nodo nuevoNodo = new Nodo(valor);
        nuevoNodo.Siguiente = _tope;
        _tope = nuevoNodo;
        _tamaño++;
    }

    // Saca y retorna el elemento del tope
    public T Pop()
    {
        if (EstaVacia())
            throw new InvalidOperationException("La pila está vacía.");

        T valor = _tope!.Valor;
        _tope = _tope.Siguiente;
        _tamaño--;
        return valor;
    }

    // Retorna el elemento del tope sin sacarlo
    public T Peek()
    {
        if (EstaVacia())
            throw new InvalidOperationException("La pila está vacía.");

        return _tope!.Valor;
    }

    // Retorna true si la pila no tiene elementos
    public bool EstaVacia()
    {
        return _tope == null;
    }

    // Retorna cuántos elementos tiene la pila
    public int Tamaño()
    {
        return _tamaño;
    }

    // Retorna todos los elementos sin modificar la pila
    // El primero de la lista es el tope de la pila
    public List<T> ObtenerTodo()
    {
        List<T> elementos = new List<T>();
        Nodo? actual = _tope;

        while (actual != null)
        {
            elementos.Add(actual.Valor);
            actual = actual.Siguiente;
        }

        return elementos;
    }

    // Vacía la pila completamente
    public void Limpiar()
    {
        _tope = null;
        _tamaño = 0;
    }

    // Busca si un elemento ya existe en la pila
    // usando una condición personalizada
    public bool BuscarEnHistorial(Func<T, bool> condicion)
    {
        Nodo? actual = _tope;

        while (actual != null)
        {
            if (condicion(actual.Valor))
                return true;

            actual = actual.Siguiente;
        }

        return false;
    }
}
}
