// Services/Interfaces/IAdminAuthService.cs
using Hotel_chain.Models.Entities;

namespace Hotel_chain.Services.Interfaces
{
    public interface IAdminAuthService
    {
        Task<Rol?> ValidateAdminAsync(string email, string password);
        Task<bool> IsAdminAuthenticatedAsync(HttpContext context);
        Task LoginAdminAsync(HttpContext context, Rol admin);
        Task LogoutAdminAsync(HttpContext context);
        Task<Rol?> GetCurrentAdminAsync(HttpContext context);
    }
}