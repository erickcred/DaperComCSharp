using eCommerce.Repositories.Interfaces;
using eCommerce.Models;

namespace eCommerce.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private List<Usuario> _dbUsuarios = new List<Usuario>()
            {
                new Usuario() { Id = 1, Nome = "Erick Rick" },
                new Usuario() { Id = 2, Nome = "Jessica Pereira" },
                new Usuario() { Id = 3, Nome = "Maily Pereira" }
            };


        public List<Usuario> Get()
        {
            throw new NotImplementedException();
        }

        public Usuario GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario Create(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public Usuario Update(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {

        }

    }
}