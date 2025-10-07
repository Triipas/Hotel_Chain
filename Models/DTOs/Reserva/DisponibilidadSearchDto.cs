// Models/DTOs/Reserva/DisponibilidadSearchDto.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.DTOs.Reserva
{
    public class DisponibilidadSearchDto
    {
        [Required(ErrorMessage = "El hotel es requerido")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "La fecha de check-in es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de check-out es requerida")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El número de huéspedes es requerido")]
        [Range(1, 10, ErrorMessage = "El número de huéspedes debe estar entre 1 y 10")]
        public int NumeroHuespedes { get; set; }
    }
}