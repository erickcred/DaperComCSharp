﻿using System;
using Dapper.Contrib.Extensions;

namespace eCommerce.Models
{
    [Table("[EnderecoEntrega]")]
    public class EnderecoEntrega
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeEndereco { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }

    }
}
