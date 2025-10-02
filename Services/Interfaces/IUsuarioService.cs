// Services/Interfaces/IUsuarioService.cs
using Hotel_chain.Models.Entities;

namespace Hotel_chain.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> SearchAsync(string? rol, string? estado, string? busqueda);
        Task<Usuario> CreateAsync(Usuario usuario, string? rolDetallado = null, string? permisosExtra = null, string? preferencias = null, string? notasInternas = null);
        Task<Usuario?> UpdateAsync(int id, Usuario usuario, string? rolDetallado = null, string? permisosExtra = null, string? preferencias = null, string? notasInternas = null);
        Task<bool> DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    }
}