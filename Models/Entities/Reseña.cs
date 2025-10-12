using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Reseña
    {
        [Key]
        public int ResenaId { get; set; }  // review_id

        // FK
        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; } = null!;

        public int ReservaId { get; set; }
        [ForeignKey("ReservaId")]
        public virtual Reserva Reserva { get; set; } = null!; // Una reseña por reserva

        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        // Contenido
        [Range(1, 5)]
        public int Calificacion { get; set; }  // rating

        [Required]
        public string Comentario { get; set; } = null!;  // comment

        // Interacciones
        public int VecesUtil { get; set; } = 0;  // helpful_count

        // Metadatos
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}