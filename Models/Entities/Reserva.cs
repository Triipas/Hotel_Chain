using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        [Required, MaxLength(50)]
        public string NumeroReserva { get; set; } = null!; // BK-2024-001

        // FK Usuario (huésped)
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        // FK Habitación
        public int HabitacionId { get; set; }
        [ForeignKey("HabitacionId")]
        public virtual Habitacion Habitacion { get; set; } = null!;

        // FK Hotel (nuevo, opcional)
        public int? HotelId { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel? Hotel { get; set; }

        // Fechas
        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        // Número de huéspedes
        [Required]
        public int NumeroHuespedes { get; set; } = 1;

        public int? GuestsAdults { get; set; }    // nuevo
        public int? GuestsChildren { get; set; }  // nuevo

        // Datos huésped principal
        [MaxLength(100)]
        public string? GuestFirstName { get; set; } // nuevo
        [MaxLength(100)]
        public string? GuestLastName { get; set; }  // nuevo
        [MaxLength(255)]
        public string? GuestEmail { get; set; }     // nuevo
        [MaxLength(20)]
        public string? GuestPhone { get; set; }     // nuevo

        // Número de noches (legacy)
        [Required]
        public int NumeroNoches { get; set; }

        // Precios
        [Column(TypeName = "decimal(10,2)")]
        public decimal? RoomRate { get; set; }      // tarifa por noche

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTotal { get; set; }    // legacy

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Subtotal { get; set; }      // nuevo
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Taxes { get; set; }         // nuevo
        [MaxLength(3)]
        public string? Currency { get; set; }       // nuevo

        // Estados
        [Required, MaxLength(50)]
        public string Estado { get; set; } = "pendiente";

        [Required, MaxLength(50)]
        public string EstadoPago { get; set; } = "pendiente";

        [MaxLength(50)]
        public string? PaymentMethod { get; set; }  // nuevo

        // Solicitudes especiales
        public string? SolicitudesEspeciales { get; set; }

        // Fechas de control
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaModificacion { get; set; }
    }
}