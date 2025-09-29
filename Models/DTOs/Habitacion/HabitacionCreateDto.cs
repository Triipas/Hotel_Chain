// Models/DTOs/Habitacion/HabitacionCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.DTOs.Habitacion
{
    public class HabitacionCreateDto
    {
        [Required(ErrorMessage = "El hotel es requerido")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "El número de habitación es requerido")]
        [MaxLength(10, ErrorMessage = "El número de habitación no puede exceder 10 caracteres")]
        public string NumeroHabitacion { get; set; } = null!;

        [Required(ErrorMessage = "El tipo es requerido")]
        [RegularExpression("^(simple|doble|suite)$", ErrorMessage = "El tipo debe ser: simple, doble o suite")]
        public string Tipo { get; set; } = null!;

        [Required(ErrorMessage = "La capacidad es requerida")]
        [Range(1, 10, ErrorMessage = "La capacidad debe estar entre 1 y 10 personas")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El precio por noche es requerido")]
        [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioNoche { get; set; }

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
