// Models/DTOs/Usuario/UsuarioCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace Hotel_chain.Models.DTOs.Usuario
{
    public class UsuarioCreateDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [MaxLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [MaxLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El documento es requerido")]
        [MaxLength(50, ErrorMessage = "El documento no puede exceder 50 caracteres")]
        public string Documento { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contraseña { get; set; } = null!;

        [Required(ErrorMessage = "El rol es requerido")]
        [RegularExpression("^(huesped|admin|recepcionista|gerente|dueño)$", 
            ErrorMessage = "El rol debe ser: huesped, admin, recepcionista, gerente o dueño")]
        public string Rol { get; set; } = "huesped";

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(activo|inactivo)$", 
            ErrorMessage = "El estado debe ser: activo o inactivo")]
        public string Estado { get; set; } = "activo";

        // Campos opcionales para Staff
        [MaxLength(100)]
        public string? RolDetallado { get; set; }

        public string? PermisosExtra { get; set; }

        // Campos opcionales para Huesped
        public string? Preferencias { get; set; }

        public string? NotasInternas { get; set; }
    }
}