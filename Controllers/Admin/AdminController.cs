using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.Admin
{
    [ServiceFilter(typeof(Filters.AdminAuthFilter))]
    public class AdminController : Controller
    {
        private readonly IAdminAuthService _adminAuthService;

        public AdminController(IAdminAuthService adminAuthService)
        {
            _adminAuthService = adminAuthService;
        }

        // GET: /admin
        public async Task<IActionResult> Index()
        {
            // Verificación manual de autenticación (temporal)
            var isAuthenticated = await _adminAuthService.IsAdminAuthenticatedAsync(HttpContext);
            if (!isAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var currentAdmin = await _adminAuthService.GetCurrentAdminAsync(HttpContext);
            ViewBag.AdminName = currentAdmin?.Nombre ?? "Admin";
            ViewBag.AdminPuesto = currentAdmin?.Puesto ?? "Administrador";

            return View();
        }

        // GET: /admin/login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /admin/login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor ingresa email y contraseña";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var admin = await _adminAuthService.ValidateAdminAsync(email, password);

            if (admin == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            await _adminAuthService.LoginAdminAsync(HttpContext, admin);

            // Redirigir a la URL solicitada o al panel admin
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }

        // GET: /admin/logout
        public async Task<IActionResult> Logout()
        {
            await _adminAuthService.LogoutAdminAsync(HttpContext);
            return RedirectToAction("Login");
        }

        // Para verificación de autenticación desde el frontend
        [HttpGet]
        public async Task<IActionResult> CheckAuth()
        {
            var isAuthenticated = await _adminAuthService.IsAdminAuthenticatedAsync(HttpContext);
            var currentAdmin = await _adminAuthService.GetCurrentAdminAsync(HttpContext);

            return Json(new
            {
                authenticated = isAuthenticated,
                admin = isAuthenticated ? new
                {
                    id = currentAdmin?.RolId,
                    nombre = currentAdmin?.Nombre,
                    apellido = currentAdmin?.Apellido,
                    puesto = currentAdmin?.Puesto
                } : null
            });
        }
    }
}