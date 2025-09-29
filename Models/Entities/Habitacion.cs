// Models/Entities/Habitacion.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Habitacion
    {
        [Key]
        public int HabitacionId { get; set; }

        [Required, MaxLength(10)]
        public string NumeroHabitacion { get; set; } = null!;

        [Required]
        public string Tipo { get; set; } = null!;

        [Required]
        public int Capacidad { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioNoche { get; set; }

        public string? Descripcion { get; set; }

        public bool Disponible { get; set; } = true;

        // FK
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;

        // Navegación
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();
    }
}