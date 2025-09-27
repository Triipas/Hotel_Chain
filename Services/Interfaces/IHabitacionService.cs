using Hotel_chain.Models;

namespace Hotel_chain.Services.Interfaces
{
    public interface IHabitacionService
    {
        Task<IEnumerable<Habitacion>> GetAllAsync();
        Task<Habitacion?> GetByIdAsync(int id);
        Task<IEnumerable<Habitacion>> GetByHotelIdAsync(int hotelId);
        Task<IEnumerable<Habitacion>> SearchAsync(int? hotelId, string? tipo, int? capacidadMinima);
        Task<Habitacion> CreateAsync(Habitacion habitacion);
        Task<Habitacion?> UpdateAsync(int id, Habitacion habitacion);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Habitacion>> GetHabitacionesConHotelesAsync();
    }
}