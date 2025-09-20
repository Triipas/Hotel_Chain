using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models;
using Hotel_chain.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel_chain.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var hoteles = _context.Hoteles.Include(h => h.Imagenes).ToList();
        return View(hoteles);
    }



        public IActionResult LoginCliente()
        {
            return View();
        }

     
        public IActionResult Habitaciones(int hotelId)
        {
             var lista = _context.Habitaciones
                        .Where(h => h.HotelId == hotelId)
                        .ToList();

  
    ViewBag.Hotel = _context.Hoteles
                            .FirstOrDefault(h => h.HotelId == hotelId);

    return View(lista);
        }

    
        [HttpPost]
       public IActionResult Reservar(int habitacionId, int usuarioId)
{
    
    var habitacion = _context.Habitaciones.FirstOrDefault(h => h.HabitacionId == habitacionId);

    if (habitacion != null && habitacion.Disponible)
    {
        var reserva = new Reserva
        {
            FechaInicio = DateTime.Now,
            FechaFin = DateTime.Now.AddDays(1),
            UsuarioId = usuarioId,
            HabitacionId = habitacion.HabitacionId,
            Estado = "confirmada",
            PrecioTotal = habitacion.PrecioNoche
        };

      
        _context.Reservas.Add(reserva);

        habitacion.Disponible = false;

   
        _context.SaveChanges();

        TempData["Mensaje"] = $"La habitación {habitacion.NumeroHabitacion} ha sido reservada.";
    }
    else
    {
        TempData["Mensaje"] = "La habitación no está disponible.";
    }

    return RedirectToAction("Index");
}
    }
}