using SEAA.Astrodex.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

// Infrastructure/structures/MemoriaService.cs
namespace SEAA.Astrodex.Infrastructure.Structures
{
    public class MemoriaService
    {
        private readonly Dictionary<string, CuerpoCeleste> _tablaHash
            = new Dictionary<string, CuerpoCeleste>();

        private readonly LinkedList<CuerpoCeleste> _listaEnlazada
            = new LinkedList<CuerpoCeleste>();

        public void Agregar(CuerpoCeleste cuerpo)
        {
            if (!_tablaHash.ContainsKey(cuerpo.Id))
            {
                _tablaHash[cuerpo.Id] = cuerpo;
                _listaEnlazada.AddLast(cuerpo);
            }
        }

        public CuerpoCeleste? BuscarPorId(string id)
        {
            _tablaHash.TryGetValue(id, out var cuerpo);
            return cuerpo;
        }
        public CuerpoCeleste? BuscarPorNombre(string nombre)
        {
            return _listaEnlazada.FirstOrDefault(c =>
                c.NombreIngles.Equals(nombre,
                    StringComparison.OrdinalIgnoreCase) ||
                c.Nombre.Equals(nombre,
                    StringComparison.OrdinalIgnoreCase) ||
                c.Id.Equals(nombre,
                    StringComparison.OrdinalIgnoreCase));
        }

        public List<CuerpoCeleste> ObtenerTodos()
        {
            return _listaEnlazada.ToList();
        }

        public bool Contiene(string id)
        {
            return _tablaHash.ContainsKey(id);
        }

        public void Limpiar()
        {
            _tablaHash.Clear();
            _listaEnlazada.Clear();
        }
    }
}
