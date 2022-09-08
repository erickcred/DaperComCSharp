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
            
        }

        public int Id { get; set; }
        public string Nome { get; set; }

        
    }
}
