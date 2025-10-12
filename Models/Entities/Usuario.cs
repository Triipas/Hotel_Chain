using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_chain.Models.Entities
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        // Información básica
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = null!;  // first_name

        [Required, MaxLength(100)]
        public string Apellido { get; set; } = null!; // last_name

        [Required, MaxLength(255)]
        public string Email { get; set; } = null!;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        [MaxLength(500)]
        public string? Avatar { get; set; } // foto de perfil

        [MaxLength(50)]
        public string? Documento { get; set; }

        [Required]
        public string Contraseña { get; set; } = null!;

        // Rol y permisos
        [Required, MaxLength(50)]
        public string Rol { get; set; } = "huesped"; // admin, manager, reception, guest

        // Estado
        [MaxLength(20)]
        public string Estado { get; set; } = "activo"; // active/inactive/suspended

        // Dirección
        [MaxLength(255)]
        public string? DireccionCalle { get; set; }    // address_street
        [MaxLength(100)]
        public string? DireccionCiudad { get; set; }   // address_city
        [MaxLength(100)]
        public string? DireccionEstado { get; set; }   // address_state
        [MaxLength(20)]
        public string? CodigoPostal { get; set; }      // address_zip_code
        [MaxLength(100)]
        public string? DireccionPais { get; set; }     // address_country

        // Contacto de emergencia
        [MaxLength(255)]
        public string? ContactoEmergenciaNombre { get; set; }
        [MaxLength(20)]
        public string? ContactoEmergenciaTelefono { get; set; }
        [MaxLength(100)]
        public string? ContactoEmergenciaRelacion { get; set; }

        // Auditoría
        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoAcceso { get; set; }    // last_login


        // Relaciones
        public virtual Huesped? Huesped { get; set; }
        public virtual Staff? Staff { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}