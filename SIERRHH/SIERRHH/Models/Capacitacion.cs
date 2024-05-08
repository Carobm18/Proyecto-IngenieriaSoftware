using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class Capacitacion
    {
        [Key]
        public int IdCapacitacion { get; set; }

        public int IdeEmpleado { get; set; }

        public string NombreCapacitacion { get; set; }

        public string Lugar { get; set; }

        public int Year { get; set; }

        public int Estado { get; set; }


    }
}
