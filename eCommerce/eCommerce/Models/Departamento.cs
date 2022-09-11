using System;
using System.Collections;
using Dapper.Contrib.Extensions;

namespace eCommerce.Models
{
    [Table("[Departamento]")]
    public class Departamento
    {
        public Departamento()
        {
            
        }

        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }

        
    }
}
