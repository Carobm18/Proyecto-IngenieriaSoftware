using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class PerfilProfesional
    {
        [Key]
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Descripcion { get; set; }

        public string Foto { get; set; }
    }
}
