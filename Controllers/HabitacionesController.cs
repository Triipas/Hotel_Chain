using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Data;
using Hotel_chain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hotel_chain.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly AppDbContext _context;

        public HabitacionController(AppDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index(int hotelId, string tipo = null, int? capacidad = null)
        {
            var query = _context.Habitaciones
                                .Include(h => h.Imagenes)
                                .Where(h => h.HotelId == hotelId)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(h => h.Tipo == tipo);

            if (capacidad.HasValue)
                query = query.Where(h => h.Capacidad >= capacidad.Value);

            var habitaciones = query.OrderBy(h => h.NumeroHabitacion).ToList();

            ViewBag.Hotel = _context.Hoteles
                                    .Include(h => h.Imagenes) 
                                    .FirstOrDefault(h => h.HotelId == hotelId);

            return View(habitaciones);
        }

        
        public IActionResult Detalles(int id)
        {
            var habitacion = _context.Habitaciones
                                     .Include(h => h.Imagenes)  
                                     .Include(h => h.Hotel)   
                                     .ThenInclude(h => h.Imagenes) 
                                     .FirstOrDefault(h => h.HabitacionId == id);

            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }
    }
}