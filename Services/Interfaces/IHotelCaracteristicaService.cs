using Hotel_chain.Models.Entities;

public interface IHotelCaracteristicaService
{
    Task<IEnumerable<HotelCaracteristica>> GetAllAsync();
    Task<HotelCaracteristica?> GetByIdAsync(int id);
    Task<IEnumerable<HotelCaracteristica>> GetByHotelIdAsync(int hotelId);
    Task<HotelCaracteristica> CreateAsync(HotelCaracteristica caracteristica);
    Task<bool> UpdateAsync(int id, HotelCaracteristica caracteristica);
    Task<bool> DeleteAsync(int id);
}