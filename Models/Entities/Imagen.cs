// Models/Entities/Imagen.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.Entities
{
    public class Imagen
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string NombreArchivo { get; set; } = null!;

        public int? HotelId { get; set; }
        public virtual Hotel? Hotel { get; set; }

        public int? HabitacionId { get; set; }
        public virtual Habitacion? Habitacion { get; set; }
    }
}