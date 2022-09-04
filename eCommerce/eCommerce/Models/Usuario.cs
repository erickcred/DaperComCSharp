using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models
{
    [Table("[Usuario]")]
    public class Usuario
    {
        public Usuario()
        {
            EnderecoEntregas = new List<EnderecoEntrega>();
            Departamentos = new List<Departamento>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public string Sexo { get; set; }

        public string RG { get; set; }
        public string CPF { get; set; }
        public string NomeMae { get; set; }
        public byte SituacaoCadastro { get; set; }
        public DateTimeOffset DataCadastro { get; set; }

        public Contato? Contato { get; set; }

        public List<EnderecoEntrega> EnderecoEntregas { get; set; }
        public List<Departamento> Departamentos { get; set; }


    }
}
