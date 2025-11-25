using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntUsuario = Usuario.ABM.Entities.Models.Usuario;
using Usuario.ABM.Data.Data;


namespace Usuario.ABM.Bussiness.Services
{
    public static class UsuarioBussiness
    {
        public static List<EntUsuario> ObtenerUsuarios()
        {
            return UsuarioData.ListarUsuarios();
        }
        public static EntUsuario ObtenerUsuario(int id) => UsuarioData.ObtenerUsuario(id);

        public static string CrearUsuario(EntUsuario usuario)
        {
            try {
                UsuarioData.CrearUsuario(usuario);
                return "Se creó correctamente el Usuario";
            }
            catch (Exception ex)
            {
               return ex.Message;
            }
            
        }

        public static void ModificarUsuario(EntUsuario usuario)
        {
            UsuarioData.ModificarUsuario(usuario);
        }

        public static void EliminarUsuario(int id)
        {
            UsuarioData.EliminarUsuario(id);
        }
    }
}
