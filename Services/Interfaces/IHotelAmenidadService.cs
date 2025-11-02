using Hotel_chain.Models.Entities;

public interface IHotelAmenidadService
{
    Task<IEnumerable<HotelAmenidad>> GetByHotelIdAsync(int hotelId);
    Task<IEnumerable<HotelAmenidad>> GetAllAsync();
    Task<HotelAmenidad?> GetByIdAsync(int id);
    Task<HotelAmenidad> CreateAsync(HotelAmenidad amenidad);
    Task<bool> UpdateAsync(int id, HotelAmenidad amenidad);
    Task<bool> DeleteAsync(int id);
}