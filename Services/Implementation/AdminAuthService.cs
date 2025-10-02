// Services/Implementation/AdminAuthService.cs
using Microsoft.EntityFrameworkCore;
using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Services.Implementation
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly AppDbContext _context;
        private const string ADMIN_SESSION_KEY = "AdminUser";
        private const string ADMIN_ID_SESSION_KEY = "AdminUserId";
        private const string ADMIN_NAME_SESSION_KEY = "AdminUserName";
        private const string ADMIN_ROLE_SESSION_KEY = "AdminUserRole";

        public AdminAuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ValidateAdminAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            // Buscar usuario con rol de admin y estado activo
            var admin = await _context.Usuarios
                .Include(u => u.Staff) // Incluir información de staff
                .FirstOrDefaultAsync(u => 
                    u.Email.ToLower() == email.ToLower() && 
                    u.Contraseña == password &&
                    (u.Rol == "admin" || u.Rol == "recepcionista" || u.Rol == "dueño") &&
                    u.Estado == "activo");

            // Actualizar último acceso
            if (admin != null)
            {
                admin.UltimoAcceso = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return admin;
        }

        public async Task<bool> IsAdminAuthenticatedAsync(HttpContext context)
        {
            var adminIdStr = context.Session.GetString(ADMIN_ID_SESSION_KEY);
            
            if (string.IsNullOrEmpty(adminIdStr) || !int.TryParse(adminIdStr, out int adminId))
                return false;

            // Verificar que el admin aún existe, está activo y tiene rol de admin
            var admin = await _context.Usuarios
                .FirstOrDefaultAsync(u => 
                    u.UsuarioId == adminId && 
                    u.Estado == "activo" &&
                    (u.Rol == "admin" || u.Rol == "recepcionista" || u.Rol == "dueño"));
            
            return admin != null;
        }

        public async Task LoginAdminAsync(HttpContext context, Usuario admin)
        {
            context.Session.SetString(ADMIN_SESSION_KEY, "true");
            context.Session.SetString(ADMIN_ID_SESSION_KEY, admin.UsuarioId.ToString());
            context.Session.SetString(ADMIN_NAME_SESSION_KEY, $"{admin.Nombre} {admin.Apellido}");
            context.Session.SetString(ADMIN_ROLE_SESSION_KEY, admin.Rol);
            
            await Task.CompletedTask;
        }

        public async Task LogoutAdminAsync(HttpContext context)
        {
            context.Session.Remove(ADMIN_SESSION_KEY);
            context.Session.Remove(ADMIN_ID_SESSION_KEY);
            context.Session.Remove(ADMIN_NAME_SESSION_KEY);
            context.Session.Remove(ADMIN_ROLE_SESSION_KEY);
            
            await Task.CompletedTask;
        }

        public async Task<Usuario?> GetCurrentAdminAsync(HttpContext context)
        {
            var adminIdStr = context.Session.GetString(ADMIN_ID_SESSION_KEY);
            
            if (string.IsNullOrEmpty(adminIdStr) || !int.TryParse(adminIdStr, out int adminId))
                return null;

            return await _context.Usuarios
                .Include(u => u.Staff)
                .FirstOrDefaultAsync(u => u.UsuarioId == adminId);
        }
    }
}