using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class PerfilAptitudes
    {
        [Required]
        public int IdEmpleado { get; set; }

        public int IdAptitudes { get; set; }

        public string descripcion = "";
    }
}
