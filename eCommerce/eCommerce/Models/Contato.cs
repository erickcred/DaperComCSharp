using System;
using Dapper.Contrib.Extensions;

namespace eCommerce.Models
{
    [Table("[Contato]")]
    public class Contato
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }

    }
}