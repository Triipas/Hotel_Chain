// Models/Entities/Reserva.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTotal { get; set; }

        [Required]
        public string Estado { get; set; } = "pendiente";

        // FK
        public int? UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        public int HabitacionId { get; set; }
        public virtual Habitacion Habitacion { get; set; } = null!;
    }
}
