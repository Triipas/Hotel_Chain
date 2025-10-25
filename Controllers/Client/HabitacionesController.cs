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
           private readonly IHabitacionAmenidadService _habitacionAmenidadService;

        public HabitacionController(IHabitacionService habitacionService, IHotelService hotelService,IHabitacionAmenidadService habitacionAmenidadService)
        {
            _habitacionService = habitacionService;
            _hotelService = hotelService;
            _habitacionAmenidadService = habitacionAmenidadService;
        }

        public async Task<IActionResult> Index(int? hotelId = null, string? tipo = null, int? capacidad = null)
{
    IEnumerable<Habitacion> habitaciones;

    if (hotelId.HasValue)
    {

        if (!string.IsNullOrEmpty(tipo) || capacidad.HasValue)
        {
            habitaciones = await _habitacionService.SearchAsync(hotelId.Value, tipo, capacidad);
        }
        else
        {
            habitaciones = await _habitacionService.GetByHotelIdAsync(hotelId.Value);
        }
    }
    else
    {
        habitaciones = await _habitacionService.GetAllAsync();
    }

    var hotel = hotelId.HasValue ? await _hotelService.GetByIdAsync(hotelId.Value) : null;
            ViewBag.Hotel = hotel;
    
    var amenidadesPorHabitacion = new Dictionary<int, IEnumerable<HabitacionAmenidad>>();
    foreach (var hab in habitaciones)
    {
        var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(hab.HabitacionId);
        amenidadesPorHabitacion[hab.HabitacionId] = amenidades;
    }

    ViewBag.AmenidadesPorHabitacion = amenidadesPorHabitacion;

    return View(habitaciones);
}


        public async Task<IActionResult> Detalles(int id)
        {
            var habitacion = await _habitacionService.GetByIdAsync(id);

            if (habitacion == null)
                return NotFound();

                var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(id);
            ViewBag.Amenidades = amenidades;

            return View(habitacion);
        }
    }
}