using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class Titulos
    {
        [Key]
        public int IdTitulo { get; set; }

        public int IdEmpleado { get; set; }

        public string Descripcion { get; set; }

        public string CentroEstudio { get; set; }

        public int IdSector { get; set; }

        public int IdGrado { get; set; }

        public string nombreSector = "";
        public string nombreGrado = "";


    }
}
