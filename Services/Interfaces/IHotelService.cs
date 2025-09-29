// Services/Interfaces/IHotelService.cs
using Hotel_chain.Models.Entities;

namespace Hotel_chain.Services.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> GetByIdAsync(int id);
        Task<IEnumerable<Hotel>> SearchAsync(string? ubicacion, string? nombre);
        Task<Hotel> CreateAsync(Hotel hotel);
        Task<Hotel?> UpdateAsync(int id, Hotel hotel);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Hotel>> GetHotelesConImagenesAsync();
    }
}