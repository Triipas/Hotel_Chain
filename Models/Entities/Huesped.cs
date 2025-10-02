// Models/Entities/Huesped.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Huesped
    {
        [Key]
        public int HuespedId { get; set; }

        // FK a Usuario
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        // JSON o texto para preferencias (ej: "habitación fumador", "piso alto", etc.)
        public string? Preferencias { get; set; }

        // Notas internas del hotel sobre el huésped
        public string? NotasInternas { get; set; }
    }
}