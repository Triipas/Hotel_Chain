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

           private readonly IHotelAmenidadService _hotelAmenidadService;
    private readonly IHotelCaracteristicaService _hotelCaracteristicaService;

        public HotelController(IHotelService hotelService, IHabitacionService habitacionService, IHotelAmenidadService hotelAmenidadService, IHotelCaracteristicaService hotelCaracteristicaService)
    {
        _hotelService = hotelService;
        _habitacionService = habitacionService;
        _hotelAmenidadService = hotelAmenidadService;
        _hotelCaracteristicaService = hotelCaracteristicaService;
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

        public async Task<IActionResult> Detalle(int id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);

            if (hotel == null)
            {
                TempData["Error"] = "Hotel no encontrado";
                return RedirectToAction("Index");
            }

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

var amenidades = await _hotelAmenidadService.GetByHotelIdAsync(id);
    var caracteristicas = await _hotelCaracteristicaService.GetByHotelIdAsync(id);

    ViewBag.Amenidades = amenidades;
    ViewBag.Caracteristicas = caracteristicas;

            return View(hotel);
        }

    }
    
}