using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers.Admin
{
    public class AdminController : Controller
    {
        // GET: /admin
        public IActionResult Index()
        {
            // Servir la aplicación SPA del admin
            return View();
        }

        // Para futuro: endpoint de verificación de autenticación admin
        [HttpGet("check-auth")]
        public IActionResult CheckAuth()
        {
            // TODO: Implementar verificación de autenticación admin
            // Por ahora retorna true para desarrollo
            return Json(new { authenticated = true, role = "admin" });
        }
    }
}