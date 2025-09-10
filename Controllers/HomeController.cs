using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models;

namespace Hotel_chain.Controllers
{
    public class HomeController : Controller
    {
        
        private static List<Hotel> hoteles = new List<Hotel>
        {
            new Hotel { Id = 1, Nombre = "Hotel Arequipa Plaza", Direccion = "Calle Mercaderes 123", Ciudad = "Arequipa", Telefono = "054-123456", Descripcion = "Céntrico y moderno" },
            new Hotel { Id = 2, Nombre = "Hotel Cusco Imperial", Direccion = "Av. El Sol 456", Ciudad = "Cusco", Telefono = "084-654321", Descripcion = "Tradicional y acogedor" }
        };

        private static List<Habitacion> habitaciones = new List<Habitacion>
        {
            new Habitacion { Id = 1, Numero = "101", Tipo = "Simple", Precio = 80, Disponible = true, HotelId = 1 },
            new Habitacion { Id = 2, Numero = "102", Tipo = "Doble", Precio = 120, Disponible = true, HotelId = 1 },
            new Habitacion { Id = 3, Numero = "201", Tipo = "Suite", Precio = 250, Disponible = false, HotelId = 2 }
        };

        private static List<Reserva> reservas = new List<Reserva>();

        
        public IActionResult Index()
        {
            return View(hoteles);
        }

        public IActionResult LoginCliente()
        {
            return View();
        }
        
        public IActionResult Habitaciones(int hotelId)
        {
            var lista = habitaciones.Where(h => h.HotelId == hotelId).ToList();
            ViewBag.Hotel = hoteles.FirstOrDefault(h => h.Id == hotelId);
            return View(lista);
        }

        
        [HttpPost]
        public IActionResult Reservar(int habitacionId, int usuarioId)
        {
            var habitacion = habitaciones.FirstOrDefault(h => h.Id == habitacionId);
            if (habitacion != null && habitacion.Disponible)
            {
                var reserva = new Reserva
                {
                    Id = reservas.Count + 1,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(1),
                    UsuarioId = usuarioId,
                    HabitacionId = habitacion.Id,
                    Estado = "Confirmada"
                };

                reservas.Add(reserva);
                habitacion.Disponible = false;
                TempData["Mensaje"] = $"La habitación {habitacion.Numero} ha sido reservada.";
            }
            else
            {
                TempData["Mensaje"] = "La habitación no está disponible.";
            }

            return RedirectToAction("Index");
        }
    }
}