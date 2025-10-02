// Models/Entities/Staff.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        // FK a Usuario
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [Required, MaxLength(100)]
        public string RolDetallado { get; set; } = null!; // admin, recepcionista, financiero, limpieza, etc.

        // Permisos adicionales en formato JSON o texto
        public string? PermisosExtra { get; set; }
    }
}