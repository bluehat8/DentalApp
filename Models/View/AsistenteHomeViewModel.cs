namespace DentalApp.Models.View
{
    public class AsistenteHomeViewModel
    {
        public Asistente? _Asistente { get; set; }
        public Usuario? Usuario { get; set; }
        public List<Notificaciones>? notificaciones { get; set; }

        public List<SolicitudCita>? solicitudes { get; set; }

    }
}
