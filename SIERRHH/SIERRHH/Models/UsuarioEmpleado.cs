using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class UsuarioEmpleado
    {
        [Key]
        public int IdEmpleado { get; set; }
        public string Correo { get; set; }

        public string Password { get; set; }

        public string Rol { get; set; }

        public string Estado { get; set; }

    }
}
