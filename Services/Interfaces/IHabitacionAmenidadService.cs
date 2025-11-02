using Hotel_chain.Models.Entities;

namespace Hotel_chain.Services.Interfaces
{
    public interface IHabitacionAmenidadService
    {
        Task<IEnumerable<HabitacionAmenidad>> GetAllAsync();
        Task<HabitacionAmenidad?> GetByIdAsync(int id);
        Task<IEnumerable<HabitacionAmenidad>> GetByHabitacionIdAsync(int habitacionId);
        Task<HabitacionAmenidad> CreateAsync(HabitacionAmenidad habitacionAmenidad);
        Task<bool> UpdateAsync(int id, HabitacionAmenidad habitacionAmenidad);
        Task<bool> DeleteAsync(int id);
    }
}