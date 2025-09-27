using Microsoft.EntityFrameworkCore;
using Hotel_chain.Data;
using Hotel_chain.Models;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Services.Implementation
{
    public class HotelService : IHotelService
    {
        private readonly AppDbContext _context;

        public HotelService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hoteles
                .Include(h => h.Imagenes)
                .OrderBy(h => h.Nombre)
                .ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hoteles
                .Include(h => h.Imagenes)
                .Include(h => h.Habitaciones)
                .ThenInclude(hab => hab.Imagenes)
                .FirstOrDefaultAsync(h => h.HotelId == id);
        }

        public async Task<IEnumerable<Hotel>> SearchAsync(string? ubicacion, string? nombre)
        {
            var query = _context.Hoteles.Include(h => h.Imagenes).AsQueryable();

            if (!string.IsNullOrEmpty(ubicacion))
                query = query.Where(h => h.Ciudad == ubicacion);

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(h => h.Nombre.Contains(nombre));

            return await query.OrderBy(h => h.Nombre).ToListAsync();
        }

        public async Task<Hotel> CreateAsync(Hotel hotel)
        {
            _context.Hoteles.Add(hotel);
            await _context.SaveChangesAsync();
            return hotel;
        }

        public async Task<Hotel?> UpdateAsync(int id, Hotel hotel)
        {
            var existingHotel = await _context.Hoteles.FindAsync(id);
            if (existingHotel == null)
                return null;

            existingHotel.Nombre = hotel.Nombre;
            existingHotel.Direccion = hotel.Direccion;
            existingHotel.Ciudad = hotel.Ciudad;
            existingHotel.Descripcion = hotel.Descripcion;
            existingHotel.TelefonoContacto = hotel.TelefonoContacto;

            await _context.SaveChangesAsync();
            return existingHotel;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hotel = await _context.Hoteles.FindAsync(id);
            if (hotel == null)
                return false;

            _context.Hoteles.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Hotel>> GetHotelesConImagenesAsync()
        {
            return await _context.Hoteles
                .Include(h => h.Imagenes)
                .ToListAsync();
        }
    }
}