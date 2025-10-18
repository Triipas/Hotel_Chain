using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_chain.Services.Implementation
{
    public class ResenaService : IResenaService
    {
        private readonly AppDbContext _context;

        public ResenaService(AppDbContext context)
        {
            _context = context;
        }

public async Task<decimal> GetPromedioCalificacionAsync(int hotelId)
        {
            var reseñas = await _context.Resenas
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();

            if (!reseñas.Any())
                return 0;

            return reseñas.Average(r => r.Calificacion);
        }


public async Task<bool> ExisteResenaAsync(int reservaId, int usuarioId)
{
    return await _context.Resenas.AnyAsync(r => r.ReservaId == reservaId && r.UsuarioId == usuarioId);
}

        public async Task<Reseña> CreateAsync(Reseña resena)
        {
            _context.Resenas.Add(resena);
            await _context.SaveChangesAsync();
            return resena;
        }

        public async Task<Reseña?> GetByIdAsync(int id)
        {
            return await _context.Resenas
                .Include(r => r.Usuario)
                .Include(r => r.Hotel)
                .Include(r => r.Reserva)
                .FirstOrDefaultAsync(r => r.ResenaId == id);
        }

        public async Task<IEnumerable<Reseña>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Resenas
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.Usuario)
                .Include(r => r.Reserva)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reseña>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Resenas
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Hotel)
                .Include(r => r.Reserva)
                .ToListAsync();
        }

        public async Task UpdateAsync(Reseña resena)
        {
            _context.Resenas.Update(resena);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var resena = await _context.Resenas.FindAsync(id);
            if (resena != null)
            {
                _context.Resenas.Remove(resena);
                await _context.SaveChangesAsync();
            }
        }
    }
}
