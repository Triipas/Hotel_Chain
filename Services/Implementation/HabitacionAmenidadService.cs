using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_chain.Services.Implementation
{
    public class HabitacionAmenidadService : IHabitacionAmenidadService
    {
        private readonly AppDbContext _context;

        public HabitacionAmenidadService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HabitacionAmenidad>> GetAllAsync()
        {
            return await _context.HabitacionAmenidades
                                 .Include(a => a.Habitacion)
                                 .ToListAsync();
        }

        public async Task<HabitacionAmenidad?> GetByIdAsync(int id)
        {
            return await _context.HabitacionAmenidades
                                 .Include(a => a.Habitacion)
                                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<HabitacionAmenidad>> GetByHabitacionIdAsync(int habitacionId)
        {
            return await _context.HabitacionAmenidades
                                 .Where(a => a.HabitacionId == habitacionId)
                                 .ToListAsync();
        }

        public async Task<HabitacionAmenidad> CreateAsync(HabitacionAmenidad habitacionAmenidad)
        {
            _context.HabitacionAmenidades.Add(habitacionAmenidad);
            await _context.SaveChangesAsync();
            return habitacionAmenidad;
        }

        public async Task<bool> UpdateAsync(int id, HabitacionAmenidad habitacionAmenidad)
        {
            var existing = await _context.HabitacionAmenidades.FindAsync(id);
            if (existing == null) return false;

            existing.Amenidad = habitacionAmenidad.Amenidad;
            existing.HabitacionId = habitacionAmenidad.HabitacionId;

            _context.HabitacionAmenidades.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.HabitacionAmenidades.FindAsync(id);
            if (existing == null) return false;

            _context.HabitacionAmenidades.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}