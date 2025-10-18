using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class HotelAmenidadService : IHotelAmenidadService
{
    private readonly AppDbContext _context;

    public HotelAmenidadService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HotelAmenidad>> GetAllAsync()
    {
        return await _context.HotelAmenidades.ToListAsync();
    }

    public async Task<IEnumerable<HotelAmenidad>> GetByHotelIdAsync(int hotelId)
    {
        return await _context.HotelAmenidades
            .Where(a => a.HotelId == hotelId)
            .ToListAsync();
    }

    public async Task<HotelAmenidad?> GetByIdAsync(int id)
    {
        return await _context.HotelAmenidades
            .FirstOrDefaultAsync(a => a.AmenidadHotelId == id);
    }

    public async Task<HotelAmenidad> CreateAsync(HotelAmenidad amenidad)
    {
        _context.HotelAmenidades.Add(amenidad);
        await _context.SaveChangesAsync();
        return amenidad;
    }

    public async Task<bool> UpdateAsync(int id, HotelAmenidad amenidad)
    {
        var existing = await _context.HotelAmenidades.FindAsync(id);
        if (existing == null)
            return false;

        existing.Amenidad = amenidad.Amenidad;
        existing.HotelId = amenidad.HotelId;

        _context.HotelAmenidades.Update(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.HotelAmenidades.FindAsync(id);
        if (existing == null)
            return false;

        _context.HotelAmenidades.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}