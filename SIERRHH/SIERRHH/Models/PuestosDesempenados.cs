namespace SIERRHH.Models
{
    public class PuestosDesempenados
    {
        public int IdPuestoDesempenado { get; set; }

        public int IdEmpleado { get; set; }

        public string Descripcion { get; set; }

        public string Empresa { get; set; }

        public int Tiempo { get; set; }
    }
}
