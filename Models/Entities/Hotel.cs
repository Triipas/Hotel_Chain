using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Direccion { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Ciudad { get; set; } = null!;

        public string? Descripcion { get; set; }

        [MaxLength(20)]
        public string? TelefonoContacto { get; set; }

        // Navegación
        public virtual ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
        public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();

        [MaxLength(100)]
        public string? Estado { get; set; }

        [MaxLength(100)]
        public string? Pais { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Latitud { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal? Longitud { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Calificacion { get; set; }

        public int? CantidadResenas { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioMin { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioMax { get; set; }

        [MaxLength(3)]
        public string? Moneda { get; set; }

        [MaxLength(20)]
        public string? CheckInTime { get; set; }

        [MaxLength(20)]
        public string? CheckOutTime { get; set; }

        public string? PoliticaCancelacion { get; set; }

        public bool? MascotasPermitidas { get; set; }

        public bool? FumarPermitido { get; set; }

        [MaxLength(255)]
        public string? ContactoEmail { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime? FechaActualizacion { get; set; }
    }
}