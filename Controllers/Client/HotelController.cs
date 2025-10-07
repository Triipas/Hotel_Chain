// Controllers/Client/HotelController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.Client
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IHabitacionService _habitacionService;

        public HotelController(IHotelService hotelService, IHabitacionService habitacionService)
        {
            _hotelService = hotelService;
            _habitacionService = habitacionService;
        }

        public async Task<IActionResult> Index(string? ubicacion, string? nombre)
        {
            IEnumerable<Hotel> hoteles;

            if (!string.IsNullOrEmpty(ubicacion) || !string.IsNullOrEmpty(nombre))
            {
                hoteles = await _hotelService.SearchAsync(ubicacion, nombre);
            }
            else
            {
                hoteles = await _hotelService.GetAllAsync();
            }

            return View(hoteles);
        }

        [HttpPost]
        public async Task<IActionResult> Buscar(string? ubicacion, string? nombre)
        {
            return await Index(ubicacion, nombre);
        }

        // GET: /Hotel/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            
            if (hotel == null)
            {
                TempData["Error"] = "Hotel no encontrado";
                return RedirectToAction("Index");
            }

            // Obtener habitaciones del hotel para calcular precio mínimo
            var habitaciones = await _habitacionService.GetByHotelIdAsync(id);
            
            if (habitaciones.Any())
            {
                ViewBag.PrecioMinimo = habitaciones.Min(h => h.PrecioNoche);
                ViewBag.TotalHabitaciones = habitaciones.Count();
            }
            else
            {
                ViewBag.PrecioMinimo = 0;
                ViewBag.TotalHabitaciones = 0;
            }

            return View(hotel);
        }
    }
}