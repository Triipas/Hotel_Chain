using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class PermisoUsuario
    {
        [Key]
        public int PermisoUsuarioId { get; set; }  // id

        // FK a Usuario
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Permiso { get; set; } = null!; // permission
    }
}