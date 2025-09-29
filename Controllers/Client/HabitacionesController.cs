// Controllers/Client/HabitacionController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.Client
{
    public class HabitacionController : Controller
    {
        private readonly IHabitacionService _habitacionService;
        private readonly IHotelService _hotelService;

        public HabitacionController(IHabitacionService habitacionService, IHotelService hotelService)
        {
            _habitacionService = habitacionService;
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index(int hotelId, string? tipo = null, int? capacidad = null)
        {
            IEnumerable<Habitacion> habitaciones;

            if (!string.IsNullOrEmpty(tipo) || capacidad.HasValue)
            {
                habitaciones = await _habitacionService.SearchAsync(hotelId, tipo, capacidad);
            }
            else
            {
                habitaciones = await _habitacionService.GetByHotelIdAsync(hotelId);
            }

            var hotel = await _hotelService.GetByIdAsync(hotelId);
            ViewBag.Hotel = hotel;

            return View(habitaciones);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var habitacion = await _habitacionService.GetByIdAsync(id);

            if (habitacion == null)
                return NotFound();

            return View(habitacion);
        }
    }
}