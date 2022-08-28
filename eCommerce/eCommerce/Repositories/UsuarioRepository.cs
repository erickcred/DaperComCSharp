using System;
using System.Linq;
using eCommerce.Repositories.Interfaces;
using eCommerce.Models;
using System.Net.WebSockets;

namespace eCommerce.Repository
{
    public class UsuarioRepository : IRepository<Usuario>
    {
        private static List<Usuario> _dbUsuarios = new List<Usuario>()
            {
                new Usuario() { Id = 1, Nome = "Erick Rick" },
                new Usuario() { Id = 2, Nome = "Jessica Pereira" },
                new Usuario() { Id = 3, Nome = "Maily Pereira" }
            };


        public List<Usuario> Get()
        {
            return _dbUsuarios.ToList();
        }

        public Usuario GetById(int id)
        {
            Usuario usuario = _dbUsuarios.FirstOrDefault(u => u.Id == id);
            return usuario;
            
        }

        public void Create(Usuario usuario)
        {
            int id = 0;
            foreach (var u in Get())
            {
                if (u.Id > 0)
                {
                    id = u.Id + 1;
                    Console.WriteLine($"{id}");
                } else
                {
                    id = 1;
                }
            }

            usuario.Id = id;
            _dbUsuarios.Add(usuario);
        }

        public void Update(Usuario usuario)
        {
            Usuario update = _dbUsuarios.FirstOrDefault(u => u.Id == usuario.Id);
            update.Nome = usuario.Nome;
        }

        public void Delete(int id)
        {
            var usuario = _dbUsuarios.FirstOrDefault(u => u.Id == id);
            _dbUsuarios.Remove(usuario);
            if (usuario.Id == 0 || usuario == null)
                throw new Exception("Unsuario não encontrado na base de dados");
        }

    }
}