// Models/DTOs/Usuario/UsuarioResponseDto.cs
namespace Hotel_chain.Models.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        // Información de Staff (si aplica)
        public string? RolDetallado { get; set; }
        public string? PermisosExtra { get; set; }

        // Información de Huesped (si aplica)
        public string? Preferencias { get; set; }
        public string? NotasInternas { get; set; }

        // Estadísticas
        public int TotalReservas { get; set; }
    }
}