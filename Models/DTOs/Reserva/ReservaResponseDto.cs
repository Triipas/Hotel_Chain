// Models/DTOs/Reserva/ReservaResponseDto.cs
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
    }
}
