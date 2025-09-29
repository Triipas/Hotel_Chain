// Models/DTOs/Habitacion/HabitacionResponseDto.cs
namespace Hotel_chain.Models.DTOs.Habitacion
{
    public class HabitacionResponseDto
    {
        public int HabitacionId { get; set; }
        public int HotelId { get; set; }
        public string HotelNombre { get; set; } = null!;
        public string NumeroHabitacion { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public int Capacidad { get; set; }
        public decimal PrecioNoche { get; set; }
        public string? Descripcion { get; set; }
        public bool Disponible { get; set; }
        public List<string> Imagenes { get; set; } = new();
    }
}