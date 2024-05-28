using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class Concurso
    {

        public int IdConcurso = 0;

        [Key]
        
        public int IdPuesto { get; set; }

        [Key]
       
        public int IdEmpleado { get; set; }

        public string ExperienciaPuesto { get; set; }

        public int Puntaje { get; set; }

        public string nombrePuesto = "";

        public string nombreEmpleado = "";
    }
}
