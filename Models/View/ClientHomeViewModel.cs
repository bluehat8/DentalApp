namespace DentalApp.Models.View
{
    public class ClientHomeViewModel
    {
        public Usuario Usuario { get; set; }
        public List<SolicitudCita> SolicitudesCita { get; set; }
        public int PageNumber { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }
    }

}
