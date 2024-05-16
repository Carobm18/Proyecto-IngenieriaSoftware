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

        public List<Aptitudes> listaAptitudes = new List<Aptitudes>();

        public List<Titulos> listaTitulos = new List<Titulos>();

        public List<Certificacion> listaCertificacion = new List<Certificacion>();

        public List<Capacitacion> listaCapacitaciones = new List<Capacitacion>();

        public List<PuestosDesempenados> listaPuestosDesemp = new List<PuestosDesempenados>();


    }
}
