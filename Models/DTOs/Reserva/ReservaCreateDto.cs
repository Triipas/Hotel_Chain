// Models/DTOs/Reserva/ReservaCreateDto.cs
using System.ComponentModel.DataAnnotations;

using System;


namespace Hotel_chain.Models.DTOs.Reserva
{
    public class ReservaCreateDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int HabitacionId { get; set; }

        public int? HotelId { get; set; } // opcional

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        [Range(1, 10)]
        public int NumeroHuespedes { get; set; }

        public int? GuestsAdults { get; set; }
        public int? GuestsChildren { get; set; }

        public string? GuestFirstName { get; set; }
        public string? GuestLastName { get; set; }
        public string? GuestEmail { get; set; }
        public string? GuestPhone { get; set; }

        [Required]
        public int NumeroNoches { get; set; }

        public decimal? RoomRate { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Taxes { get; set; }
        public string? Currency { get; set; }

        public decimal PrecioTotal { get; set; } // obligatorio

        public string? PaymentMethod { get; set; }
        public string? SolicitudesEspeciales { get; set; }
    }
}