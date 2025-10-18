// Models/DTOs/Hotel/HotelUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.DTOs.Hotel
{
    public class HotelUpdateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es requerida")]
        [MaxLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "La ciudad es requerida")]
        [MaxLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
        public string Ciudad { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [MaxLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? TelefonoContacto { get; set; }

            public string? Pais { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public bool? MascotasPermitidas { get; set; }
        public bool? FumarPermitido { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public string? PoliticaCancelacion { get; set; }

public string? Estado { get; set; }
public string? Moneda { get; set; }
public string? CheckInTime { get; set; }
public string? CheckOutTime { get; set; }
public string? ContactoEmail { get; set; }

    }
}
