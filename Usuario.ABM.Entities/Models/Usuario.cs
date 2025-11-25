using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuario.ABM.Entities.Models
{
    public class Usuario
    {
        public int Id { get; set; } = 0;
        public string Descripcion { get; set; }      
        public string Tipo { get; set; }              // "Administrador" | "Cliente" | "Agente"
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }         
        public bool Activo { get; set; }           

        public Usuario() { }

        public Usuario(int id, string descripcion, string tipo, string correoElectronico, string telefono, bool activo)
        {
            Id = id;
            Descripcion = descripcion;
            Tipo = tipo;
            CorreoElectronico = correoElectronico;
            Telefono = telefono;
            Activo = activo;
        }

        public void MostrarInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Descripción: {Descripcion}");
            Console.WriteLine($"Tipo: {Tipo}");
            Console.WriteLine($"Correo Electrónico: {CorreoElectronico}");
            Console.WriteLine($"Teléfono: {Telefono}");
            Console.WriteLine($"Activo: {(Activo ? "Sí" : "No")}");
        }
    }
}
