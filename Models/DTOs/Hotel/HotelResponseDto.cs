// Models/DTOs/Hotel/HotelResponseDto.cs
namespace Hotel_chain.Models.DTOs.Hotel
{
    public class HotelResponseDto
    {
        public int HotelId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string? Pais { get; set; }       // <-- agregado
        public string? Descripcion { get; set; }
        public string? TelefonoContacto { get; set; }

        public decimal? Latitud { get; set; }   // <-- agregado
        public decimal? Longitud { get; set; }  // <-- agregado
        public decimal? Calificacion { get; set; } // <-- agregado
        public bool? MascotasPermitidas { get; set; } // <-- agregado
        public bool? FumarPermitido { get; set; }    // <-- agregado

        public int TotalHabitaciones { get; set; }
        public List<string> Imagenes { get; set; } = new();
        public string? Estado { get; set; }
        public string? Moneda { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string? ContactoEmail { get; set; }

public string? PoliticaCancelacion { get; set; }
    }
}