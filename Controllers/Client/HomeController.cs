// Controllers/Client/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.Client
{
    public class HomeController : Controller
    {
        private readonly IHotelService _hotelService;

        public HomeController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            var hoteles = await _hotelService.GetHotelesConImagenesAsync();
            return View(hoteles);
        }

        public IActionResult LoginCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Reservar(int habitacionId, int usuarioId)
        {
            TempData["Mensaje"] = "Función de reservas en mantenimiento - será migrada a la nueva arquitectura";
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }


    }
}
