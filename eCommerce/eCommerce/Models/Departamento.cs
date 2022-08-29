using System;
using System.Collections;

namespace eCommerce.Models
{
    public class Departamento
    {
        public Departamento()
        {
            Usuarios = new List<Usuario>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
