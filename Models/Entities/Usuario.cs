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

        [Required, MaxLength(20)]
        public string Telefono { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Documento { get; set; } = null!; // DNI/Pasaporte

        [Required]
        public string Contraseña { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Rol { get; set; } = "huesped"; // admin, recepcionista, huesped, dueño, etc.

        [Required, MaxLength(20)]
        public string Estado { get; set; } = "activo"; // activo/inactivo

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime? UltimoAcceso { get; set; }

        // Navegación
        public virtual Huesped? Huesped { get; set; }
        public virtual Staff? Staff { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}