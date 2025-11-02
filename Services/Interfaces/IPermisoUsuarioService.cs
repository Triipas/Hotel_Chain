using Hotel_chain.Models.Entities;

namespace Hotel_chain.Services.Interfaces
{
    public interface IPermisoUsuarioService
    {
        Task<IEnumerable<PermisoUsuario>> GetPermisosByUsuarioIdAsync(int usuarioId);
        Task<bool> UsuarioTienePermisoAsync(int usuarioId, string permiso);
        Task<PermisoUsuario> AddPermisoAsync(PermisoUsuario permisoUsuario);
        Task<bool> RemovePermisoAsync(int permisoUsuarioId);
    }
}