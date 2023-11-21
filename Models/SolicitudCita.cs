namespace DentalApp.Models
{
    public class SolicitudCita
    {
        public int Userid { get; set; }

        public int Id { get; set; }
        public int PacienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int TipoCita { get; set; }
        public string? MotivoCita { get; set; }
        public byte Estado { get; set; }
        public bool Activo { get; set; }
    }
}
