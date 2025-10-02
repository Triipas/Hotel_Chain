// Models/Entities/Reserva.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        [Required, MaxLength(50)]
        public string NumeroReserva { get; set; } = null!; // ej: BK-2024-001

        // FK Usuario (huésped que realiza la reserva)
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        // FK Habitación
        public int HabitacionId { get; set; }
        [ForeignKey("HabitacionId")]
        public virtual Habitacion Habitacion { get; set; } = null!;

        // Fechas
        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        // Número de huéspedes
        [Required]
        public int NumeroHuespedes { get; set; } = 1;

        // Número de noches (calculado)
        [Required]
        public int NumeroNoches { get; set; }

        // Precio
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTotal { get; set; }

        // Estados
        [Required, MaxLength(50)]
        public string Estado { get; set; } = "pendiente"; // pendiente, confirmada, cancelada, completada

        [Required, MaxLength(50)]
        public string EstadoPago { get; set; } = "pendiente"; // pendiente, pagado, reembolsado

        // Solicitudes especiales
        public string? SolicitudesEspeciales { get; set; }

        // Fechas de control
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaModificacion { get; set; }
    }
}