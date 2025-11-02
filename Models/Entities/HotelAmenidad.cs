using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class HotelAmenidad
    {
        [Key]
        public int AmenidadHotelId { get; set; }  // Id único

        [Required]
        public int HotelId { get; set; }          // FK al hotel

        [ForeignKey("HotelId")]
        public virtual Hotel? Hotel { get; set; }

        [Required, MaxLength(100)]
        public string Amenidad { get; set; } = null!; // Ej: WiFi, Piscina
    }
}