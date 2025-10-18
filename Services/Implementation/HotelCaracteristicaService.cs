using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class HotelCaracteristicaService : IHotelCaracteristicaService
{
    private readonly AppDbContext _context;

    public HotelCaracteristicaService(AppDbContext context)
    {
        _context = context;
    }

    // Obtener todas las características
    public async Task<IEnumerable<HotelCaracteristica>> GetAllAsync()
    {
        return await _context.HotelCaracteristicas.ToListAsync();
    }

    // Obtener por Id
    public async Task<HotelCaracteristica?> GetByIdAsync(int id)
    {
        return await _context.HotelCaracteristicas.FindAsync(id);
    }

    // Obtener por hotel
    public async Task<IEnumerable<HotelCaracteristica>> GetByHotelIdAsync(int hotelId)
    {
        return await _context.HotelCaracteristicas
            .Where(c => c.HotelId == hotelId)
            .ToListAsync();
    }

    // Crear nueva característica
    public async Task<HotelCaracteristica> CreateAsync(HotelCaracteristica caracteristica)
    {
        _context.HotelCaracteristicas.Add(caracteristica);
        await _context.SaveChangesAsync();
        return caracteristica;
    }

    // Actualizar característica
    public async Task<bool> UpdateAsync(int id, HotelCaracteristica caracteristica)
    {
        var existing = await _context.HotelCaracteristicas.FindAsync(id);
        if (existing == null) return false;

        existing.Caracteristica = caracteristica.Caracteristica;
        existing.HotelId = caracteristica.HotelId;

        _context.HotelCaracteristicas.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    // Eliminar característica
    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.HotelCaracteristicas.FindAsync(id);
        if (existing == null) return false;

        _context.HotelCaracteristicas.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}