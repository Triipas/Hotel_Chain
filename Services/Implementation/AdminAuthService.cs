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

        public AdminAuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rol?> ValidateAdminAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            // Por ahora usamos la tabla Roles como tabla de usuarios admin
            var admin = await _context.Roles
                .FirstOrDefaultAsync(r => r.Email.ToLower() == email.ToLower() && 
                                         r.Contraseña == password);

            return admin;
        }

        public async Task<bool> IsAdminAuthenticatedAsync(HttpContext context)
        {
            var adminIdStr = context.Session.GetString(ADMIN_ID_SESSION_KEY);
            
            if (string.IsNullOrEmpty(adminIdStr) || !int.TryParse(adminIdStr, out int adminId))
                return false;

            // Verificar que el admin aún existe y está activo
            var admin = await _context.Roles.FindAsync(adminId);
            return admin != null;
        }

        public async Task LoginAdminAsync(HttpContext context, Rol admin)
        {
            context.Session.SetString(ADMIN_SESSION_KEY, "true");
            context.Session.SetString(ADMIN_ID_SESSION_KEY, admin.RolId.ToString());
            context.Session.SetString(ADMIN_NAME_SESSION_KEY, $"{admin.Nombre} {admin.Apellido}");
            
            await Task.CompletedTask;
        }

        public async Task LogoutAdminAsync(HttpContext context)
        {
            context.Session.Remove(ADMIN_SESSION_KEY);
            context.Session.Remove(ADMIN_ID_SESSION_KEY);
            context.Session.Remove(ADMIN_NAME_SESSION_KEY);
            
            await Task.CompletedTask;
        }

        public async Task<Rol?> GetCurrentAdminAsync(HttpContext context)
        {
            var adminIdStr = context.Session.GetString(ADMIN_ID_SESSION_KEY);
            
            if (string.IsNullOrEmpty(adminIdStr) || !int.TryParse(adminIdStr, out int adminId))
                return null;

            return await _context.Roles.FindAsync(adminId);
        }
    }
}