// Models/Entities/Habitacion.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Habitacion
    {
        [Key]
        public int HabitacionId { get; set; }

        [Required, MaxLength(10)]
        public string NumeroHabitacion { get; set; } = null!;

        // Mantengo Tipo como string para compatibilidad con la BD actual.
        [Required, MaxLength(100)]
        public string Tipo { get; set; } = null!;

        // Nombre descriptivo corto (ej: "Suite Vista Mar")
        [MaxLength(255)]
        public string? Nombre { get; set; }

        // Capacidad general (legacy)
        [Required]
        public int Capacidad { get; set; }

        // Capacidad separada (nuevos)
        public int? CapacidadAdultos { get; set; }
        public int? CapacidadNinos { get; set; }
        public int? CantidadCamas { get; set; }
        [MaxLength(100)]
        public string? TipoCama { get; set; }  // King, Queen, Twin, etc.

        // Tamaño
        public int? TamanoM2 { get; set; } // metros cuadrados

        // Precios: base, impuestos, total (decimal 10,2)
        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioBase { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioImpuestos { get; set; }

        // PrecioNoche legacy (lo mantenemos por compatibilidad)
        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioNoche { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioTotal { get; set; }

        [MaxLength(3)]
        public string? Moneda { get; set; } // "USD", "PEN", etc.

        // Disponibilidad
        public bool Disponible { get; set; } = true;
        public int? HabitacionesDisponibles { get; set; } // cantidad disponible de este tipo

        public string? Descripcion { get; set; }

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // FK hacia Hotel (igual que antes)
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;

        // Navegación (igual que antes)
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();
    }
}