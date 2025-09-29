// Controllers/Client/HotelController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.Client
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
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
    }
}