namespace Hotel_chain.Models.DTOs.Reserva
{
    public class ReservaResponseDto
    {
        public int ReservaId { get; set; }
        public string NumeroReserva { get; set; } = null!;

        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = null!;
        public string UsuarioEmail { get; set; } = null!;

        public int HabitacionId { get; set; }
        public string HabitacionNumero { get; set; } = null!;
        public int HotelId { get; set; }
        public string HotelNombre { get; set; } = null!;

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumeroHuespedes { get; set; }
        public int NumeroNoches { get; set; }
        public decimal PrecioTotal { get; set; }

        public string Estado { get; set; } = null!;
        public string EstadoPago { get; set; } = null!;

        public string? SolicitudesEspeciales { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // 🔹 Campos nuevos
        public int? GuestsAdults { get; set; }
        public int? GuestsChildren { get; set; }

        public string? GuestFirstName { get; set; }
        public string? GuestLastName { get; set; }
        public string? GuestEmail { get; set; }
        public string? GuestPhone { get; set; }

        public decimal? RoomRate { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Taxes { get; set; }
        public string? Currency { get; set; }

        public string? PaymentMethod { get; set; }
    }
}