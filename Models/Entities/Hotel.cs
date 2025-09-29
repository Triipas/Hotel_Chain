// Models/Entities/Hotel.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.Entities
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Direccion { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Ciudad { get; set; } = null!;

        public string? Descripcion { get; set; }

        [MaxLength(20)]
        public string? TelefonoContacto { get; set; }

        // Navegación
        public virtual ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
        public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();
    }
}
