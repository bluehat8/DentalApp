namespace DentalApp.Models
{
    public class HistorialClinico
    {
        public int Id { get; set; }

        public int Paciente { get; set; }

        public string Observaciones { get; set; } = null!;

        public string EnfermedadPadre { get; set; } = null!;

        public string EnfermedadMadre { get; set; } = null!;

        public byte Deporte { get; set; }

        public byte MalestarDeporte { get; set; }

        public byte Diabetico { get; set; }

        public byte ProblemaCardiaco { get; set; }

        public byte ProblemaRenales { get; set; }

        public byte PresionAlta { get; set; }

        public byte Epileptico { get; set; }

        public byte Operado { get; set; }

        public string DetalleOperacion { get; set; } = null!;

        public byte Caries { get; set; }

        public string EstadoBucal { get; set; } = null!;

        public string OtrasEnfermedades { get; set; } = null!;

        public string SangraEncimas { get; set; } = null!;

        public string DientesFracturados { get; set; } = null!;

        public byte Embarazado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

    }
}
