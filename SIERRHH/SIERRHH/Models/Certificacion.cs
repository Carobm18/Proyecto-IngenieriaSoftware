namespace SIERRHH.Models
{
    public class Certificacion
    {
        public int IdCertificado { get; set; }

        public int IdEmpleado { get; set; }

        public string NombreCertificacion { get; set; }

        public string Entidad { get; set; }

        public int IdSector { get; set; }
    }
}
