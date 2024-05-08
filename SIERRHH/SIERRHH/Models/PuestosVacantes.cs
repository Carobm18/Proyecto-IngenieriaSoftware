using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class PuestosVacantes
    {
        [Key]
        public int IdPuesto { get; set; }

        public string Descripcion { get; set; }

        public int IdSector { get; set; }

        public string Estdado { get; set; }

        public DateTime FechaPublicacion { get; set; }

        public string Modalidad { get; set; }

  

    }
}
