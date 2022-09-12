using Dapper;
using eCommerce.Models;
using eCommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eCommerce.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TricksController : ControllerBase
    {
        private readonly string connectionString = @"Data Source=localhost\SQLEXPRESS;Database=eCommerce;User ID=sa;Password=123;TrustServerCertificate=True";
        private IDbConnection _connection;

        public TricksController()
        {
            _connection = new SqlConnection(connectionString);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string sql = @"SELECT * FROM [Usuario] WHERE [Id] = @Id
                        SELECT * FROM [Contato] WHERE [UsuarioId] = @Id
                        SELECT * FROM [EnderecoEntrega] WHERE [UsuarioId] = @Id
                        SELECT 
                            [Departamento].* 
                        FROM [UsuarioDepartamento]
                            INNER JOIN [Departamento] ON [UsuarioDepartamento].[DepartamentoId] = [Departamento].[Id] 
                        WHERE [UsuarioDepartamento].[UsuarioId] = @Id";

            using (var queryMultiple = _connection.QueryMultiple(sql, new { Id = id }))
            {
                var usuario = queryMultiple.Read<Usuario>().FirstOrDefault();
                var contato = queryMultiple.Read<Contato>().FirstOrDefault();
                var enderecoEntregas = (List<EnderecoEntrega>)queryMultiple.Read<EnderecoEntrega>();
                var departamentos = (List<Departamento>)queryMultiple.Read<Departamento>();

                if (usuario != null)
                {
                    usuario.Contato = contato;
                    usuario.EnderecoEntregas = enderecoEntregas;
                    usuario.Departamentos = departamentos;
                }
                return Ok(usuario);
            }
        }
    }
}
