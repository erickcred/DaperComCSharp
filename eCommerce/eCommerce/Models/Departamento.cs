using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models
{
    [Table("[Departamento]")]
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
