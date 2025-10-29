using Microsoft.AspNetCore.Mvc;
using Usuario.ABM.Bussiness.Services;
using EntUsuario = Usuario.ABM.Entities.Models.Usuario;

namespace CatppuccinoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        [HttpGet(Name = "ObtenerUsuarios")]
        public ActionResult<IEnumerable<EntUsuario>> GetUsuarios()
        {
            return Ok(UsuarioBussiness.ObtenerUsuarios().ToArray());
        }

        [HttpGet("{id}", Name = "ObtenerUsuario")]

        public ActionResult<EntUsuario> GetUsuario(int id)
        {
            var resultado = UsuarioBussiness.ObtenerUsuario(id);

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpPost(Name = "CrearUsuario")]

        public ActionResult Post([FromBody] EntUsuario usuario)
        {
           var message = UsuarioBussiness.CrearUsuario(usuario);
            return Ok(message);
        }

        [HttpPut(Name = "ModificarUsuario")]

        public ActionResult Put([FromBody] EntUsuario usuario)
        {
            var resultado = UsuarioBussiness.ObtenerUsuario(usuario.Id);
            if (resultado != null)
            {
                UsuarioBussiness.ModificarUsuario(usuario);
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete(Name = "EliminarUsuario")]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var resultado = UsuarioBussiness.ObtenerUsuario(id);
            if (resultado != null)
            {
                UsuarioBussiness.EliminarUsuario(id);
            }
            else
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
