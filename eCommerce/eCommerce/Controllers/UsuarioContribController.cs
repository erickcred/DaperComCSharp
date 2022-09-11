using eCommerce.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using eCommerce.Models;
using eCommerce.Repository;
using Microsoft.Identity.Client;
using eCommerce.Repositories;

namespace eCommecer.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsuarioContribController : ControllerBase
    {
        private IRepository<Usuario> _usuarioRepository;
        public UsuarioContribController()
        {
            _usuarioRepository = new UsuarioContribRepository();
        }

        [HttpGet()]
        public IActionResult Get()
        {
            var usuario = _usuarioRepository.Get();
            return Ok(usuario);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_usuarioRepository.GetById(id));
        }

        [HttpGet("lixeira")]
        public IActionResult Lixeira()
        {
            return Ok(_usuarioRepository.Lixeira());
        }

        [HttpPost()]
        public IActionResult Create([FromBody]Usuario usuario)
        {
            if (usuario != null)
                _usuarioRepository.Create(usuario);
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody]Usuario usuario)
        {
            if (usuario !=  null)
                _usuarioRepository.Update(usuario);

            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public IActionResult Lixeira(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario != null)
                _usuarioRepository.Lixeira(usuario.Id);

            return Ok(usuario);
        }

        [HttpDelete("lixeira/{id}")]
        public IActionResult DeletePermanente(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario != null)
                _usuarioRepository.Delete(usuario.Id);
            return Ok(usuario);
        }
    }
}