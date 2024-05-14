using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class PuestosDesempenados
    {
        [Key]
        public int IdPuestoDesempenado { get; set; }

        public int IdEmpleado { get; set; }

        public string Descripcion { get; set; }

        public string Empresa { get; set; }

        public string Tiempo { get; set; }
    }
}
