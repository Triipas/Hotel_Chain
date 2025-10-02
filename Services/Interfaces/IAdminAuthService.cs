// Services/Interfaces/IAdminAuthService.cs
using Hotel_chain.Models.Entities;

namespace Hotel_chain.Services.Interfaces
{
    public interface IAdminAuthService
    {
        Task<Usuario?> ValidateAdminAsync(string email, string password);
        Task<bool> IsAdminAuthenticatedAsync(HttpContext context);
        Task LoginAdminAsync(HttpContext context, Usuario admin);
        Task LogoutAdminAsync(HttpContext context);
        Task<Usuario?> GetCurrentAdminAsync(HttpContext context);
    }
}