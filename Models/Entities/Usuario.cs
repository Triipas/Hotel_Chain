// Models/Entities/Usuario.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.Entities
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Apellido { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string Contra { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Telefono { get; set; } = null!;

        // Navegación
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
