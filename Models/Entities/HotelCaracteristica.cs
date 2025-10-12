using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class HotelCaracteristica
    {
        [Key]
        public int Id { get; set; }

        // FK hacia el hotel
        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Caracteristica { get; set; } = null!;
    }
}