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

       public int HotelId { get; set; }               // FK
       [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; } = null!; 

         [MaxLength(100)]
        public string? Departamento { get; set; }   // department

         public DateTime? Fechadeingreso { get; set; }    // last_login
    }
}