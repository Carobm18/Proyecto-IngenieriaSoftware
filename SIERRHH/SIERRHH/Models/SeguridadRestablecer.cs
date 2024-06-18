using System.ComponentModel.DataAnnotations;

namespace SIERRHH.Models
{
    public class SeguridadRestablecer
    {
        public string Correo { get; set; }

       
        public string Password { get; set; }

       
        public string NuevoPassword { get; set; }

        public string Confirmar { get; set; }
    }
}
