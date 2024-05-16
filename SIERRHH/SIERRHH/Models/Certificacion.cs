using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class Certificacion
    {
        [Key]
        public int IdCertificado { get; set; }

        public int IdEmpleado { get; set; }

        public string NombreCertificacion { get; set; }

        public string Entidad { get; set; }

        public int IdSector { get; set; }

        public string nombreSector = "";
    }
}
