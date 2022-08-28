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
                    [Usuario].*,
                    [Contato].*
                FROM
                    [Usuario]
                    LEFT JOIN [Contato] ON [Contato].[UsuarioId] = [Usuario].[Id]
                WHERE
                    [Usuario].[SituacaoCadastro] = 1
                ORDER BY [Usuario].[Id] DESC";

            return (List<Usuario>)_connection.Query<Usuario, Contato, Usuario>(
                sqlSelect,
                (usuario, contato) =>
                {
                    if (contato == null)
                    {
                        usuario.Contato = null;
                    } else
                    {
                        usuario.Contato = contato;
                    }
                    return usuario;
                }, splitOn: "Id");
        }

        public Usuario GetById(int id)
        {
            string querySql = @"SELECT
                    [Usuario].*,
                    [Contato].*
                FROM
                    [Usuario]
                    LEFT JOIN [Contato] ON [Contato].[UsuarioId] = [Usuario].[Id]
                WHERE [Usuario].[Id] = @Id";

            var usuario = new Usuario();

            var resultado = _connection.Query<Usuario, Contato, Usuario>(
                querySql,
                (resUsuario, contato) => 
                {
                    usuario = resUsuario;
                    if (contato == null)
                        resUsuario.Contato = null;
                    resUsuario.Contato = contato; 
                    return resUsuario; 
                },
                new { Id = id },
                splitOn: "Id"
                );
            return usuario;
        }

        public void Create(Usuario usuario)
        {
            var id = _connection.QuerySingle<int>(@"
                INSERT INTO
                    [Usuario]  ([Nome], [Email], [Sexo], [RG], [CPF], [NomeMae], [SituacaoCadastro], [DataCadastro])
                    OUTPUT INSERTED.[Id]
                VALUES
                    (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, GETDATE());",
                new
                {
                    usuario.Nome,
                    usuario.Email,
                    Sexo = usuario.Sexo.ToUpper(),
                    usuario.RG,
                    usuario.CPF,
                    usuario.NomeMae,
                    usuario.SituacaoCadastro,
                });

            _connection.Query<Contato>(@"
                INSERT INTO
                    [Contato] ([UsuarioId], [Telefone], [Celular])
                VALUES
                    (@UsuarioId, @Telefone, @Celular)",
                new
                {
                    UsuarioId = id,
                    Telefone = usuario.Contato.Telefone,
                    Celular = usuario.Contato.Celular
                });
        }

        public void Update(Usuario usuario)
        {
            string sqlUpdate = @"
                UPDATE
                    [Usuario]
                SET
                    [Nome] = @Nome,
                    [Email] = @Email,
                    [Sexo] = @Sexo,
                    [RG] = @RG,
                    [CPF] = @CPF,
                    [NomeMae] = @NomeMae, 
                    [SituacaoCadastro] = @SituacaoCadastro
                WHERE [Id] = @Id;
                UPDATE
                    [Contato]
                SET
                    [UsuarioId] = @Id,
                    [Telefone] = @Telefone,
                    [Celular] = @Celular
                WHERE [Id] = @ContatoId";

            try
            {
                _connection.QueryAsync<Usuario, Contato, Usuario>(
                    sqlUpdate, 
                    (resUsuario, contato) =>
                    {
                        resUsuario = usuario;
                        resUsuario.Contato = contato;
                        return resUsuario;
                    },
                    new
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Sexo = usuario.Sexo.ToUpper(),
                        RG = usuario.RG,
                        CPF = usuario.CPF,
                        NomeMae = usuario.NomeMae,
                        SituacaoCadastro = usuario.SituacaoCadastro,
                        ContatoId = usuario.Contato.Id,
                        UsuarioId = usuario.Contato.UsuarioId,
                        Telefone = usuario.Contato.Telefone,
                        Celular = usuario.Contato.Celular
                    });
            } catch (Exception erro)
            {
                throw new Exception($"Erro: {erro.Message} \n----\nPilha: {erro.StackTrace} \n----\nTipo: {erro.GetType()} \n----\nInnerException: {erro.InnerException}");
            }
        }

        public List<Usuario> Lixeira()
        {
            return (List<Usuario>)_connection.Query<Usuario, Contato, Usuario>(
                @" SELECT
                        [Usuario].*,
                        [Contato].*
                    FROM
                        [Usuario]
                        LEFT JOIN [Contato] ON [Contato].[UsuarioId] = [Usuario].[Id]
                    WHERE
                        [SituacaoCadastro] = 0
                    ORDER BY [Usuario].[Id] DESC",
                (usuario, contato) =>
                {
                    usuario.Contato = contato;
                    return usuario;
                }, splitOn: "Id");
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