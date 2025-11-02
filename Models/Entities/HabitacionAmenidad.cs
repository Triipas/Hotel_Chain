using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class HabitacionAmenidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HabitacionId { get; set; }
        public virtual Habitacion? Habitacion { get; set; }

        [Required, MaxLength(100)]
        public string Amenidad { get; set; } = null!;
    }
}