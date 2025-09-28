using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Hotel_chain.Data;
using Hotel_chain.Models;
using System.Linq;

namespace Hotel_chain.Controllers.Client // 🆕 Namespace actualizado
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("/Views/User/Login.cshtml");
        }

        [HttpPost]
        public IActionResult LoginUser(string email, string contra)
        {

            var usuario = _context.Usuarios
                                  .FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Contra == contra);

            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mensaje = "Correo o contraseña incorrectos";
                return View("/Views/User/Login.cshtml");
            }
        }

        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(string nombre, string apellido, string email, string contra, string telefono)
        {
            if (_context.Usuarios.Any(u => u.Email.ToLower() == email.ToLower()))
            {
                ViewBag.Mensaje = "El correo ya está registrado";
                return View();
            }

            var usuario = new Usuario
            {
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Contra = contra,
                Telefono = telefono
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["Mensaje"] = "Cuenta creada exitosamente. Ingresa tus datos para iniciar sesión.";
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}