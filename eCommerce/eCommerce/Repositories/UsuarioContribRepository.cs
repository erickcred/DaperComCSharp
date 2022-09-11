using Dapper.Contrib.Extensions;
using eCommerce.Repositories.Interfaces;
using eCommerce.Models;
using Microsoft.Data.SqlClient;

namespace eCommerce.Repositories
{
    public class UsuarioContribRepository : IRepository<Usuario>
    {
        private SqlConnection _connection;
        private readonly string connectionString = 
            @"Data Source=localhost\SQLEXPRESS;Database=eCommerce;User ID=sa;Password=123;TrustServerCertificate=True;";

        public UsuarioContribRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<Usuario> Get()
        {
            return (List<Usuario>)_connection.GetAll<Usuario>();
        }

        public Usuario GetById(int id)
        {
            return _connection.Get<Usuario>(id);
        }

        public List<Usuario> Lixeira()
        {
            var usuarios = new List<Usuario>();
            foreach (var usuario in _connection.GetAll<Usuario>())
            {
                if (usuario.SituacaoCadastro == 0)
                    usuarios.Add(usuario);
            }
            return usuarios;
        }

        public void Lixeira(int id)
        {
            var usuario = _connection.Get<Usuario>(id);
            usuario.SituacaoCadastro = 0;
            _connection.Update<Usuario>(usuario);
        }

        public void Update(Usuario id)
        {
            var usuario = _connection.Get<Usuario>(id);
            _connection.Update<Usuario>(usuario);
        }

        public void Create(Usuario usuario)
        {
            usuario.Id = Convert.ToInt32(_connection.Insert(usuario));
        }

        public void Delete(int id)
        {
            var usuario = _connection.Get<Usuario>(id);
            var contato = _connection.GetAll<Contato>().FirstOrDefault(x => x.UsuarioId == usuario.Id);
            var enderecoEntrega = _connection.GetAll<EnderecoEntrega>().FirstOrDefault(x => x.UsuarioId == usuario.Id);
            if (usuario != null)
            {
                if (contato != null)
                    _connection.Delete(contato);

                if (enderecoEntrega != null)
                    _connection.Delete(enderecoEntrega);

                usuario.Id = Convert.ToInt32(_connection.Delete<Usuario>(usuario));
            }
        }
    }
}