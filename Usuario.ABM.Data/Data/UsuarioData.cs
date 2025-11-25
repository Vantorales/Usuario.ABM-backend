using EntUsuario = Usuario.ABM.Entities.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Usuario.ABM.Data.Data
{
    public static class UsuarioData
    {
        // Ajustá el datasource a tu instancia
        private static readonly string connectionString =
            @"Server=localhost\SQLEXPRESS;Database=UsuariosDB;Trusted_Connection=true;TrustServerCertificate=True;";

        // ---------- SELECT ALL ----------
        public static List<EntUsuario> ListarUsuarios()
        {
            var lista = new List<EntUsuario>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                "select Id, Descripcion, Tipo, CorreoElectronico, Telefono, Activo from dbo.Usuario order by Id desc",
                connection))
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(MapUsuario(reader));
                }
            }
            return lista;
        }

        // ---------- SELECT BY ID ----------
        public static EntUsuario ObtenerUsuario(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                "select Id, Descripcion, Tipo, CorreoElectronico, Telefono, Activo from dbo.Usuario where Id = @Id",
                connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                connection.Open();
                using var reader = command.ExecuteReader();
                return reader.Read() ? MapUsuario(reader) : null;
            }
        }

        // ---------- INSERT ----------
        public static void CrearUsuario(EntUsuario usuario)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                @"insert into dbo.Usuario (Descripcion, Tipo, CorreoElectronico, Telefono, Activo)
                  values (@Descripcion, @Tipo, @Correo, @Telefono, @Activo);", connection))
            {
                command.Parameters.Add("@Descripcion", SqlDbType.VarChar, 100).Value = (object)usuario.Descripcion ?? DBNull.Value;
                command.Parameters.Add("@Tipo", SqlDbType.VarChar, 20).Value = (object)usuario.Tipo ?? DBNull.Value; // 'Administrador' | 'Cliente' | 'Agente'
                command.Parameters.Add("@Correo", SqlDbType.VarChar, 120).Value = (object)usuario.CorreoElectronico ?? DBNull.Value;
                command.Parameters.Add("@Telefono", SqlDbType.VarChar, 30).Value = (object?)usuario.Telefono ?? DBNull.Value;
                command.Parameters.Add("@Activo", SqlDbType.Bit).Value = usuario.Activo;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Usuario creado correctamente.");
                }
                catch (SqlException ex)
                {
                    // Índice único de CorreoElectronico
                    Console.WriteLine("Error: ya existe un usuario con ese correo electrónico.");
                    throw new Exception($"ya existe un usuario con ese correo electrónico");
                }
            }
        }

        // ---------- UPDATE ----------
        public static void ModificarUsuario(EntUsuario usuario)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                @"update dbo.Usuario
                  set Descripcion = @Descripcion,
                      Tipo = @Tipo,
                      CorreoElectronico = @Correo,
                      Telefono = @Telefono,
                      Activo = @Activo
                  where Id = @Id", connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = usuario.Id;
                command.Parameters.Add("@Descripcion", SqlDbType.VarChar, 100).Value = (object)usuario.Descripcion ?? DBNull.Value;
                command.Parameters.Add("@Tipo", SqlDbType.VarChar, 20).Value = (object)usuario.Tipo ?? DBNull.Value;
                command.Parameters.Add("@Correo", SqlDbType.VarChar, 120).Value = (object)usuario.CorreoElectronico ?? DBNull.Value;
                command.Parameters.Add("@Telefono", SqlDbType.VarChar, 30).Value = (object?)usuario.Telefono ?? DBNull.Value;
                command.Parameters.Add("@Activo", SqlDbType.Bit).Value = usuario.Activo;

                try
                {
                    connection.Open();
                    var rows = command.ExecuteNonQuery();
                    Console.WriteLine(rows == 0
                        ? "No se encontró el usuario a modificar."
                        : "Usuario modificado correctamente.");
                }
                catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
                {
                    Console.WriteLine("Error: ya existe un usuario con ese correo electrónico.");
                    throw;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error al modificar el usuario: {ex.Message}");
                    throw;
                }
            }
        }

        // ---------- DELETE ----------
        public static void EliminarUsuario(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                "delete from dbo.Usuario where Id = @Id", connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                try
                {
                    connection.Open();
                    var rows = command.ExecuteNonQuery();
                    Console.WriteLine(rows == 0
                        ? "No se encontró el usuario a eliminar."
                        : "Usuario eliminado correctamente.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error al eliminar al usuario: {ex.Message}");
                    throw;
                }
            }
        }

        // ---------- HELPERS ----------
        private static EntUsuario MapUsuario(SqlDataReader reader)
        {
            return new EntUsuario
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Descripcion = reader["Descripcion"]?.ToString(),
                Tipo = reader["Tipo"]?.ToString(),
                CorreoElectronico = reader["CorreoElectronico"]?.ToString(),
                Telefono = reader["Telefono"] == DBNull.Value ? null : reader["Telefono"].ToString(),
                Activo = reader["Activo"] != DBNull.Value && Convert.ToBoolean(reader["Activo"])
            };
        }

        public static bool ExisteCorreo(string correo, int? excluirId = null)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(
                @"select count(1)
                  from dbo.Usuario
                  where CorreoElectronico = @Correo
                    and (@Excl is null or Id <> @Excl)", connection))
            {
                command.Parameters.Add("@Correo", SqlDbType.VarChar, 120).Value = correo;
                command.Parameters.Add("@Excl", SqlDbType.Int).Value = (object?)excluirId ?? DBNull.Value;

                connection.Open();
                var count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
