namespace DentalApp.Models.View
{
    public class ClientHomeViewModel
    {
        public Usuario? Usuario { get; set; }
        public List<Usuario?>? listUsuarios { get; set; }
        public List<SolicitudCita>? SolicitudesCita { get; set; }
        public List<Notificaciones?>? notificaciones { get; set; }
        public List<TipoCita?>? tipoCitas { get; set; }
        public HistorialClinico? historialClinico { get; set; }
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }
    }

}
