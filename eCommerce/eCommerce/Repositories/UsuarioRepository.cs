using System;
using System.Linq;
using Dapper;
using eCommerce.Repositories.Interfaces;
using eCommerce.Models;
using System.Net.WebSockets;
using Microsoft.Data.SqlClient;

namespace eCommerce.Repository
{
    public class UsuarioRepository : IRepository<Usuario>
    {
        private static List<Usuario> _dbUsuarios = new List<Usuario>()
            {
                new Usuario() { Id = 1, Nome = "Erick Rick" },
                new Usuario() { Id = 2, Nome = "Jessica Pereira" },
                new Usuario() { Id = 3, Nome = "Maily Pereira" }
            };

        private SqlConnection _connection;
        private readonly string connectionString = @"Data Source=localhost\SQLEXPRESS;Database=eCommerce;User ID=sa;Password=123;TrustServerCertificate=True;";

        public UsuarioRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<Usuario> Get()
        {
            string sqlSelect = @"
                SELECT
                    *
                FROM
                    [Usuario]
                WHERE
                    [SituacaoCadastro] = 1";

            return (List<Usuario>)_connection.Query<Usuario>(sqlSelect);
        }

        public Usuario GetById(int id)
        {
            Usuario usuario = _connection.QueryFirstOrDefault<Usuario>("SELECT * FROM [Usuario] WHERE [Id] = @Id",
                new
                {
                    Id = id
                });
            return usuario;
            
        }

        public void Create(Usuario usuario)
        {
            _connection.Execute(@"
                INSERT INTO
                    [Usuario]
                    ([Nome], [Email], [Sexo], [RG], [CPF], [NomeMae], [SituacaoCadastro], [DataCadastro])
                    OUTPUT INSERTED.[Id]
                VALUES
                    (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, GETDATE())",
            new
            {
                usuario.Nome,
                usuario.Email,
                Sexo = usuario.Sexo.ToUpper(),
                usuario.RG,
                usuario.CPF,
                usuario.NomeMae,
                usuario.SituacaoCadastro
            });
        }

        public void Update(Usuario usuario)
        {
            string sqlUpdate = @"
                UPDATE
                    [Usuario]
                SET
                    [Nome] = @Nome, [Email] = @Email, [Sexo] = @Sexo, [RG] = @RG, [CPF] = @CPF, [NomeMae] = @NomeMae, [SituacaoCadastro] = @SituacaoCadastro
                WHERE [Id] = @Id";

            _connection.Query<Usuario>(sqlUpdate, new
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Sexo = usuario.Sexo.ToUpper(),
                RG = usuario.RG,
                CPF = usuario.CPF,
                NomeMae = usuario.NomeMae,
                SituacaoCadastro = usuario.SituacaoCadastro,
            });
        }

        public List<Usuario> Lixeira()
        {
            return (List<Usuario>)_connection.Query<Usuario>(@"
                SELECT
                    *
                FROM
                    [Usuario]
                WHERE
                    [SituacaoCadastro] = 0
            ");
        }

        public void Delete(int id)
        {
            _connection.Query(@"
                UPDATE
                    [Usuario]
                SET 
                    [SituacaoCadastro] = 0
                WHERE [Id] = @Id",
            new
            {
                Id = id
            });
        }

    }
}