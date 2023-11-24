namespace DentalApp.Models.View
{
    public class DoctorHomeViewModel
    {
        public Doctor? _Doctor { get; set; }  
        public Usuario? Usuario { get; set; }
        public List<Especialidades>? Especialidades { get; set; }
        public List<Notificaciones>? notificaciones { get; set; }

        //public List<HistorialClinico> HistorialClinico { get; set; }
        //public List<Tratamiento> Tratamientos { get; set; }
        //public List<SeguimientoTratamiento> SeguimientoTratamiento { get; set; }
    }
}
