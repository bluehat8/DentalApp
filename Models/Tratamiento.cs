using System.ComponentModel.DataAnnotations;

namespace DentalApp.Models
{
    public class Tratamiento
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Duracion { get; set; }

        public decimal Precio { get; set; }

        public string Descripcion { get; set; } = null!;

        public string Restricciones { get; set; } = null!;

        [MaxLength(100)]
        public byte[] Imagen { get; set; } = null!;

        public int TipoTratamiento { get; set; }

        public byte Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }
    }
}
