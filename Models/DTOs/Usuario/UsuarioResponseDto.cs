namespace Hotel_chain.Models.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Avatar { get; set; }
        public string? Documento { get; set; }
        public string Rol { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        // Dirección
        public string? DireccionCalle { get; set; }
        public string? DireccionCiudad { get; set; }
        public string? DireccionEstado { get; set; }
        public string? CodigoPostal { get; set; }
        public string? DireccionPais { get; set; }

        // Contacto de emergencia
        public string? ContactoEmergenciaNombre { get; set; }
        public string? ContactoEmergenciaTelefono { get; set; }
        public string? ContactoEmergenciaRelacion { get; set; }
    }
}