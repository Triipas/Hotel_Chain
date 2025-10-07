// Models/DTOs/Reserva/ReservaUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.DTOs.Reserva
{
    public class ReservaUpdateDto
    {
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El número de huéspedes es requerido")]
        [Range(1, 10, ErrorMessage = "El número de huéspedes debe estar entre 1 y 10")]
        public int NumeroHuespedes { get; set; }

        public string? SolicitudesEspeciales { get; set; }
    }
}
