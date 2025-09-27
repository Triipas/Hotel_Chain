using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models;
using Hotel_chain.Services.Interfaces; // 🆕 Usar servicios en lugar de DbContext directo

namespace Hotel_chain.Controllers.Client // 🆕 Namespace actualizado
{
    public class HomeController : Controller
    {
        private readonly IHotelService _hotelService; // 🆕 Usar servicio

        public HomeController(IHotelService hotelService) // 🆕 Inyectar servicio
        {
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index() // 🆕 Hacer async
        {
            var hoteles = await _hotelService.GetHotelesConImagenesAsync(); // 🆕 Usar servicio
            return View(hoteles);
        }

        public IActionResult LoginCliente()
        {
            return View();
        }

        // 🔄 Mantener el resto de métodos exactamente igual por ahora
        // TODO: Migrar gradualmente a usar servicios
        
        [HttpPost]
        public IActionResult Reservar(int habitacionId, int usuarioId)
        {
            // TODO: Este método se migrará a usar ReservaService en el futuro
            // Por ahora mantener la lógica existente para no romper funcionalidad
            
            // Redirigir temporalmente hasta implementar ReservaService
            TempData["Mensaje"] = "Función de reservas en mantenimiento - será migrada a la nueva arquitectura";
            return RedirectToAction("Index");
        }
    }
}