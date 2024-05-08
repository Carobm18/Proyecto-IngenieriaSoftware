using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class Aptitudes
    {

        [Key]
        public int IdAptitud { get; set; }

        public string Descripcion { get; set; }

        public int Puntaje { get; set; }

        public string Estado { get; set; }

        
    }
}
