using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Hotel_chain.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Apellido { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string Contra { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Telefono { get; set; } = null!;

        // Navegación
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }

    public class Rol
    {
        [Key]
        public int RolId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Apellido { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string Contraseña { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Telefono { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Puesto { get; set; } = null!;
    }

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
    }

    public class Habitacion
{
    [Key]
    public int HabitacionId { get; set; }

    [Required, MaxLength(10)]
    [Column("numero_habitacion")]
    public string NumeroHabitacion { get; set; } = null!;

    [Required]
    public string Tipo { get; set; } = null!;

    [Required]
    public int Capacidad { get; set; }

    [Required, Column("precio_noche",TypeName = "decimal(10,2)")]
    public decimal PrecioNoche { get; set; }

    public string? Descripcion { get; set; }

    // FK
    public int HotelId { get; set; }
    public virtual Hotel Hotel { get; set; } = null!;

    public bool Disponible { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();
}

    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTotal { get; set; }

        [Required]
        public string Estado { get; set; } = "pendiente";

        // FK (nullable en SQL → int?)
        public int? UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        public int HabitacionId { get; set; }
        public virtual Habitacion Habitacion { get; set; } = null!;
    }

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