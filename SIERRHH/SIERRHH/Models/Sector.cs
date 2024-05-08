using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class Sector
    {
        [Key]
        public int IdSector { get; set; }

        public string NombreSector { get; set; }

        public string Estado { get; set; }
    }
}
