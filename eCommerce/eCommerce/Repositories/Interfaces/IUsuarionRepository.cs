using System;
using System.Collections.Generic;
using System.Linq;
using eCommerce.Models;

namespace eCommerce.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        public List<Usuario> Get();
        public Usuario GetById(int id);
        public Usuario Create(Usuario usuario);
        public Usuario Update(Usuario id);
        public void Delete(int id);
    }
}
