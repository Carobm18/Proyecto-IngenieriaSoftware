using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class Grado
    {
        [Key]
        public int IdGrado { get; set; }

        public string Descripcion { get; set; }

        public int Puntaje { get; set; }

        public string Estado { get; set; }
    }
}
