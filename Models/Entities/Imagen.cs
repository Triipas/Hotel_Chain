// Models/Entities/Imagen.cs - Versión Final
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.Entities
{
    public class Imagen
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string? NombreArchivo { get; set; }

        [Required, MaxLength(500)]
        public string UrlS3 { get; set; } = null!;

        [MaxLength(50)]
        public string? TipoMime { get; set; }

        public long? Tamano { get; set; }

        public int? HotelId { get; set; }
        public virtual Hotel? Hotel { get; set; }

        public int? HabitacionId { get; set; }
        public virtual Habitacion? Habitacion { get; set; }

        public DateTime? FechaSubida { get; set; }

        public int? SubidoPor { get; set; } // ID del usuario que subió la imagen
    }
}