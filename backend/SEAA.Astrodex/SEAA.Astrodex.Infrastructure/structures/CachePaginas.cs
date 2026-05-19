// Infrastructure/structures/CachePaginas.cs
using SEAA.Astrodex.Core.Entities;

namespace SEAA.Astrodex.Infrastructure.Structures
{
    public class CachePaginas
    {
        private const int MAXIMO_PAGINAS = 3;

        private readonly Dictionary<string, List<CuerpoCeleste>> _paginas
            = new Dictionary<string, List<CuerpoCeleste>>();

        // Lleva el orden de las últimas claves usadas
        private readonly LinkedList<string> _ordenUso = new LinkedList<string>();

        // Construye la clave estándar
        public static string GenerarClave(string tipo, int pagina, int tamanio)
            => $"{tipo}_pagina_{pagina}_tamanio_{tamanio}".ToLower();

        // Obtiene una página del caché, retorna null si no está
        public List<CuerpoCeleste>? Obtener(string clave)
        {
            if (!_paginas.ContainsKey(clave))
                return null;

            // Marca como usada recientemente
            _ordenUso.Remove(clave);
            _ordenUso.AddLast(clave);

            return _paginas[clave];
        }

        // Agrega una página al caché aplicando política LRU
        public void Agregar(string clave, List<CuerpoCeleste> cuerpos)
        {
            // Si ya existe, solo actualiza orden y datos
            if (_paginas.ContainsKey(clave))
            {
                _paginas[clave] = cuerpos;
                _ordenUso.Remove(clave);
                _ordenUso.AddLast(clave);
                return;
            }

            // Si el caché está lleno, elimina la página más vieja
            if (_paginas.Count >= MAXIMO_PAGINAS)
            {
                var masVieja = _ordenUso.First!.Value;
                _ordenUso.RemoveFirst();
                _paginas.Remove(masVieja);
            }

            _paginas[clave] = cuerpos;
            _ordenUso.AddLast(clave);
        }

        public void Limpiar()
        {
            _paginas.Clear();
            _ordenUso.Clear();
        }
    }
}
