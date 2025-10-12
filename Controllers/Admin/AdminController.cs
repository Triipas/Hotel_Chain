// Controllers/Admin/AdminController.cs - Actualizado
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Models.Entities;

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
            var isAuthenticated = await _adminAuthService.IsAdminAuthenticatedAsync(HttpContext);
            if (!isAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var currentAdmin = await _adminAuthService.GetCurrentAdminAsync(HttpContext);
            ViewBag.AdminName = currentAdmin?.Nombre ?? "Admin";
            
            // Obtener el rol detallado del staff si existe
            var rolDetallado = currentAdmin?.Staff?.RolDetallado ?? currentAdmin?.Rol ?? "Administrador";
            ViewBag.AdminPuesto = rolDetallado;

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
                ViewBag.Error = "Credenciales incorrectas o no tienes permisos de administrador. Si eres cliente, ingresa desde el panel de clientes.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            await _adminAuthService.LoginAdminAsync(HttpContext, admin);

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
                    id = currentAdmin?.UsuarioId,
                    nombre = currentAdmin?.Nombre,
                    apellido = currentAdmin?.Apellido,
                    rol = currentAdmin?.Rol,
                    rolDetallado = currentAdmin?.Staff?.RolDetallado
                } : null
            });
        }
    }
}