using Microsoft.AspNetCore.Mvc;
using eCommerce.Models;
using eCommerce.Repository;
using eCommerce.Repositories.Interfaces;

namespace eCommerce.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IRepository<Usuario> _usuarioRepository;

        public UsuarioController()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_usuarioRepository.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario != null)
                return Ok(_usuarioRepository.GetById(id));
            return NotFound(new
            {
                title = "Usuario não encontrado!",
                status = 404
            });
        }

        [HttpGet("lixeira")]
        public IActionResult Lixeira()
        {
            return Ok(_usuarioRepository.Lixeira());
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            _usuarioRepository.Create(usuario);

            return Ok(usuario);
        }

        [HttpPut]
        public IActionResult Update(Usuario usuario)
        {
            _usuarioRepository.Update(usuario);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario != null)
            {
                _usuarioRepository.Delete(usuario.Id);
                return Ok(usuario);
            } else
            {
                return NotFound(new
                {
                    title = "Usuario não encontrado!",
                    status = 404
                });
            }
        }
    }
}