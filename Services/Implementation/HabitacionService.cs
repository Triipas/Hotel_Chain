// Services/Implementation/HabitacionService.cs
using Microsoft.EntityFrameworkCore;
using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Services.Implementation
{
    public class HabitacionService : IHabitacionService
    {
        private readonly AppDbContext _context;

        public HabitacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Habitacion>> GetAllAsync()
        {
            return await _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Imagenes)
                .OrderBy(h => h.Hotel.Nombre)
                .ThenBy(h => h.NumeroHabitacion)
                .ToListAsync();
        }

        public async Task<Habitacion?> GetByIdAsync(int id)
        {
            return await _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Imagenes)
                .Include(h => h.Reservas)
                .FirstOrDefaultAsync(h => h.HabitacionId == id);
        }

        public async Task<IEnumerable<Habitacion>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Imagenes)
                .Where(h => h.HotelId == hotelId)
                .OrderBy(h => h.NumeroHabitacion)
                .ToListAsync();
        }

        public async Task<IEnumerable<Habitacion>> SearchAsync(int? hotelId, string? tipo, int? capacidadMinima)
        {
            var query = _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Imagenes)
                .AsQueryable();

            if (hotelId.HasValue)
                query = query.Where(h => h.HotelId == hotelId.Value);

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(h => h.Tipo == tipo);

            if (capacidadMinima.HasValue)
                query = query.Where(h => h.Capacidad >= capacidadMinima.Value);

            return await query
                .OrderBy(h => h.Hotel.Nombre)
                .ThenBy(h => h.NumeroHabitacion)
                .ToListAsync();
        }

        public async Task<Habitacion> CreateAsync(Habitacion habitacion)
        {
            _context.Habitaciones.Add(habitacion);
            await _context.SaveChangesAsync();
            
            // Recargar con el hotel incluido
            return await GetByIdAsync(habitacion.HabitacionId) ?? habitacion;
        }

        public async Task<Habitacion?> UpdateAsync(int id, Habitacion habitacion)
        {
            var existingHabitacion = await _context.Habitaciones.FindAsync(id);
            if (existingHabitacion == null)
                return null;

            existingHabitacion.HotelId = habitacion.HotelId;
            existingHabitacion.NumeroHabitacion = habitacion.NumeroHabitacion;
            existingHabitacion.Tipo = habitacion.Tipo;
            existingHabitacion.Capacidad = habitacion.Capacidad;
            existingHabitacion.PrecioNoche = habitacion.PrecioNoche;
            existingHabitacion.Descripcion = habitacion.Descripcion;
            existingHabitacion.Disponible = habitacion.Disponible;

            await _context.SaveChangesAsync();
            
            // Recargar con el hotel incluido
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var habitacion = await _context.Habitaciones
                .Include(h => h.Reservas)
                .FirstOrDefaultAsync(h => h.HabitacionId == id);
                
            if (habitacion == null)
                return false;

            // Verificar si tiene reservas asociadas
            if (habitacion.Reservas.Any())
            {
                throw new InvalidOperationException($"No se puede eliminar la habitación '{habitacion.NumeroHabitacion}' porque tiene reservas asociadas. Cancela primero las reservas.");
            }

            // Eliminar imágenes asociadas a la habitación
            var imagenes = await _context.Imagenes
                .Where(i => i.HabitacionId == id)
                .ToListAsync();
            
            if (imagenes.Any())
            {
                _context.Imagenes.RemoveRange(imagenes);
            }

            _context.Habitaciones.Remove(habitacion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Habitacion>> GetHabitacionesConHotelesAsync()
        {
            return await _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Imagenes)
                .ToListAsync();
        }
    }
}