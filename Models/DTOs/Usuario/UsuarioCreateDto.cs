using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.DTOs.Usuario
{
    public class UsuarioCreateDto
    {
        // Información básica
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [MaxLength(255, ErrorMessage = "El email no puede exceder 255 caracteres")]
        public string Email { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? Telefono { get; set; }

        [MaxLength(500, ErrorMessage = "La URL del avatar no puede exceder 500 caracteres")]
        public string? Avatar { get; set; }

        [MaxLength(50, ErrorMessage = "El documento no puede exceder 50 caracteres")]
        public string? Documento { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contraseña { get; set; } = null!;

        // Rol y estado
        [Required(ErrorMessage = "El rol es requerido")]
        [RegularExpression("^(huesped|admin|recepcionista|gerente|dueño)$",
            ErrorMessage = "El rol debe ser: huesped, admin, recepcionista, gerente o dueño")]
        public string Rol { get; set; } = "huesped";

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(activo|inactivo)$",
            ErrorMessage = "El estado debe ser: activo o inactivo")]
        public string Estado { get; set; } = "activo";

        // Dirección
        [MaxLength(255, ErrorMessage = "La calle no puede exceder 255 caracteres")]
        public string? DireccionCalle { get; set; }

        [MaxLength(100, ErrorMessage = "La ciudad no puede exceder 100 caracteres")]
        public string? DireccionCiudad { get; set; }

        [MaxLength(100, ErrorMessage = "El estado o región no puede exceder 100 caracteres")]
        public string? DireccionEstado { get; set; }

        [MaxLength(20, ErrorMessage = "El código postal no puede exceder 20 caracteres")]
        public string? CodigoPostal { get; set; }

        [MaxLength(100, ErrorMessage = "El país no puede exceder 100 caracteres")]
        public string? DireccionPais { get; set; }

        // Contacto de emergencia
        [MaxLength(255, ErrorMessage = "El nombre del contacto no puede exceder 255 caracteres")]
        public string? ContactoEmergenciaNombre { get; set; }

        [MaxLength(20, ErrorMessage = "El teléfono de emergencia no puede exceder 20 caracteres")]
        public string? ContactoEmergenciaTelefono { get; set; }

        [MaxLength(100, ErrorMessage = "La relación del contacto no puede exceder 100 caracteres")]
        public string? ContactoEmergenciaRelacion { get; set; }
    }
}