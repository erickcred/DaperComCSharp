using System;
using System.Linq;
using Dapper;
using eCommerce.Repositories.Interfaces;
using eCommerce.Models;
using System.Net.WebSockets;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Routing;

namespace eCommerce.Repository
{
    public class UsuarioRepository : IRepository<Usuario>
    {
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
                    [Usuario] AS [Usuario]
                    LEFT JOIN [Contato] AS [Contato] ON [Contato].[UsuarioId] = [Usuario].[Id]
                    LEFT JOIN [EnderecoEntrega] AS [EnderecoEntrega] ON [EnderecoEntrega].[UsuarioId] = [Usuario].[Id]
                WHERE
                    [Usuario].[SituacaoCadastro] = 1
                ORDER BY [Usuario].[Id] DESC";

            var listaUsuarios = new List<Usuario>();

            _connection.Query<Usuario, Contato, EnderecoEntrega, Usuario>(
                sqlSelect,
                (usuario, contato, enderecoEntrega) =>
                {
                    var us = listaUsuarios.FirstOrDefault(x => x.Id == usuario.Id);
                    if (us == null)
                    {
                        us = usuario;
                        if (enderecoEntrega != null)
                            us.EnderecoEntregas.Add(enderecoEntrega);

                        us.Contato = contato;
                        listaUsuarios.Add(us);
                    } else
                    {
                        us.EnderecoEntregas.Add(enderecoEntrega);
                    }

                    return usuario;
                }, splitOn: "Id");
            return listaUsuarios;
        }

        public Usuario GetById(int id)
        {
            string querySql = @"SELECT
                    *
                FROM
                    [Usuario] AS [Usuario]
                    LEFT JOIN [Contato] AS [Contato] ON [Contato].[UsuarioId] = [Usuario].[Id]
                    LEFT JOIN [EnderecoEntrega] AS [EnderecoEntrega] ON [EnderecoEntrega].[UsuarioId] = [Usuario].[Id]
                WHERE [Usuario].[Id] = @Id";

            var lista = new List<Usuario>();

            var resultado = _connection.Query<Usuario, Contato, EnderecoEntrega, Usuario>(
                querySql,
                (usuario, contato, enderecoEntrega) => 
                {
                    var us = lista.FirstOrDefault(x => x.Id == id);
                    if (us == null)
                    {
                        us = usuario;
                        if (enderecoEntrega != null)
                            us.EnderecoEntregas.Add(enderecoEntrega);

                        us.Contato = contato;
                        lista.Add(us);
                    } else
                    {
                        us.EnderecoEntregas.Add(enderecoEntrega);
                    }
                        
                    return usuario;
                },
                new { Id = id }, splitOn: "Id"
                ).FirstOrDefault();
            return resultado;
        }

        public void Create(Usuario usuario)
        {
            var validaCpf = _connection.QuerySingleOrDefault<Usuario>("SELECT [CPF] FROM [Usuario] WHERE [CPF] = @CPF", new { CPF = usuario.CPF });
            var validaEmail = _connection.QuerySingleOrDefault<Usuario>("SELECT [Email] FROM [Usuario] WHERE [Email] = @Email", new { Email = usuario.Email });
            if (validaCpf != null)
                throw new Exception("CPF já cadastrado.");
            if (validaEmail != null)
                throw new Exception("E-mail já cadastrado.");

            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                usuario.Id = _connection.QuerySingleOrDefault<int>(@"
                    INSERT INTO
                        [Usuario]  ([Nome], [Email], [Sexo], [RG], [CPF], [NomeMae], [SituacaoCadastro], [DataCadastro])
                        --OUTPUT INSERTED.[Id]
                    VALUES
                        (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                    usuario, transaction);

                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;
                    usuario.Contato.Id = _connection.QuerySingleOrDefault<int>(@"
                        INSERT INTO
                            [Contato] ([UsuarioId], [Telefone], [Celular])
                        VALUES
                            (@UsuarioId, @Telefone, @Celular); SELECT CAST(SCOPE_IDENTITY() AS INT);",
                        usuario.Contato, transaction);
                }

                transaction.Commit();
                
            } catch (Exception erro)
            {
                transaction.Rollback(erro.Message);
            } finally
            {
                _connection.Close();
            }
        }

        public void Update(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();

            try
            {
                string sqlUsuario = @" UPDATE
                        [Usuario]
                    SET
                        [Nome] = @Nome,  [Email] = @Email, [Sexo] = @Sexo, [RG] = @RG, [CPF] = @CPF, [NomeMae] = @NomeMae, [SituacaoCadastro] = @SituacaoCadastro
                    WHERE [Id] = @Id;";

                string sqlContato = @" UPDATE
                        [Contato]
                    SET
                        [UsuarioId] = @Id, [Telefone] = @Telefone, [Celular] = @Celular 
                    WHERE [UsuarioId] = @Id";

                _connection.Execute(sqlUsuario, usuario, transaction);

                if (usuario.Contato != null)
                    _connection.Execute(sqlContato, usuario.Contato, transaction);

                transaction.Commit();
            } catch (Exception erro)
            {
                transaction.Rollback();
                throw erro;
            } finally
            {
                _connection.Close();
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

        public void Lixeira(int id)
        {
            _connection.Execute(@"
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

        public void Delete(int id)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();
            try
            {
                _connection.Execute(
                    @"DELETE FROM [Contato] WHERE [UsuarioId] = @Id;
                        DELETE FROM [UsuarioDepartamento] WHERE [UsuarioId] = @Id;
                        DELETE FROM [EnderecoEntrega] WHERE [UsuarioId] = @Id;
                        DELETE FROM [Usuario] WHERE [Id] = @Id;",
                    new { Id = id }, transaction);

                transaction.Commit();
            } catch (Exception erro)
            {
                transaction.Rollback();
                throw new Exception(erro.Message);
            } finally
            {
                _connection.Close();
            }
        }
    }
}