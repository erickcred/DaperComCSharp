using Microsoft.AspNetCore.Mvc;
using eCommerce.Models;
using eCommerce.Repository;

namespace eCommerce.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            UsuarioRepository usuarios = new UsuarioRepository();

            return Ok(usuarios.Get());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var usuarios = new UsuarioRepository();

            return Ok(usuarios.GetById(id));
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            UsuarioRepository usuarios = new UsuarioRepository();
            usuarios.Create(usuario);

            return Ok(usuario);
        }

        [HttpPut]
        public IActionResult Update(Usuario usuario)
        {
            UsuarioRepository usuarios = new UsuarioRepository();
            usuarios.Update(usuario);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UsuarioRepository usuarios = new UsuarioRepository();
            var usuario = usuarios.GetById(id);
            usuarios.Delete(usuario.Id);
            
            return Ok(usuario);
        }
    }
}