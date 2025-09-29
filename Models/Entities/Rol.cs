// Models/Entities/Rol.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.Entities
{
    public class Rol
    {
        [Key]
        public int RolId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Apellido { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string Contraseña { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Telefono { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Puesto { get; set; } = null!;
    }
}