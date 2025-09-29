// Controllers/Client/LoginController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using System.Linq;

namespace Hotel_chain.Controllers.Client
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(AppDbContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("/Views/User/Login.cshtml");
        }

        [HttpPost]
        public IActionResult LoginUser(string email, string contra)
        {
            try
            {
                var usuario = _context.Usuarios
                                      .FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Contra == contra);

                if (usuario != null)
                {
                    HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                    HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                    HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Mensaje = "Correo o contraseña incorrectos";
                    return View("/Views/User/Login.cshtml");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar iniciar sesión");
                ViewBag.Mensaje = "Error al procesar la solicitud. Intenta nuevamente.";
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
            try
            {
                _logger.LogInformation($"Intento de registro para email: {email}");

                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(nombre) || 
                    string.IsNullOrWhiteSpace(apellido) || 
                    string.IsNullOrWhiteSpace(email) || 
                    string.IsNullOrWhiteSpace(contra) || 
                    string.IsNullOrWhiteSpace(telefono))
                {
                    ViewBag.Mensaje = "Todos los campos son obligatorios";
                    return View("/Views/User/Login.cshtml");
                }

                // Verificar si el email ya existe
                if (_context.Usuarios.Any(u => u.Email.ToLower() == email.ToLower()))
                {
                    ViewBag.Mensaje = "El correo ya está registrado";
                    return View("/Views/User/Login.cshtml");
                }

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = nombre.Trim(),
                    Apellido = apellido.Trim(),
                    Email = email.Trim().ToLower(),
                    Contra = contra,
                    Telefono = telefono.Trim()
                };

                _context.Usuarios.Add(usuario);
                
                // Guardar cambios en la base de datos
                var result = _context.SaveChanges();
                
                _logger.LogInformation($"Usuario registrado exitosamente. ID: {usuario.UsuarioId}, Filas afectadas: {result}");

                TempData["Mensaje"] = "Cuenta creada exitosamente. Ingresa tus datos para iniciar sesión.";
                
                // Retornar un mensaje de éxito
                return Content("Cuenta creada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                ViewBag.Mensaje = "Error al crear la cuenta. Por favor intenta nuevamente.";
                return View("/Views/User/Login.cshtml");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}