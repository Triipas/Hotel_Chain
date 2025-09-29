// Models/DTOs/Hotel/HotelResponseDto.cs
namespace Hotel_chain.Models.DTOs.Hotel
{
    public class HotelResponseDto
    {
        public int HotelId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? TelefonoContacto { get; set; }
        public int TotalHabitaciones { get; set; }
        public List<string> Imagenes { get; set; } = new();
    }
}