using Hotel_chain.Models.Entities;

namespace Hotel_chain.Services.Interfaces
{
    public interface IResenaService
    {
        Task<Reseña> CreateAsync(Reseña resena);
        Task<Reseña?> GetByIdAsync(int id);
        Task<IEnumerable<Reseña>> GetByHotelIdAsync(int hotelId);
        Task<IEnumerable<Reseña>> GetByUsuarioIdAsync(int usuarioId);
        Task<decimal> GetPromedioCalificacionAsync(int hotelId);
          Task<bool> ExisteResenaAsync(int reservaId, int usuarioId);
        Task UpdateAsync(Reseña resena);
        Task DeleteAsync(int id);
    }
}