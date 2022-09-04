using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models
{
    [Table("[Contato]")]
    public class Contato
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }

    }
}