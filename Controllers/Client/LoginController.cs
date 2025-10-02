// Controllers/Client/LoginController.cs - Actualizado
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> LoginUser(string email, string contra)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => 
                        u.Email.ToLower() == email.ToLower() && 
                        u.Contraseña == contra &&
                        u.Estado == "activo");

                if (usuario != null)
                {
                    // VALIDACIÓN: Solo permitir acceso a usuarios con rol "huesped"
                    if (usuario.Rol != "huesped")
                    {
                        _logger.LogWarning($"Intento de acceso al panel de cliente con rol '{usuario.Rol}' por {email}");
                        ViewBag.Mensaje = "Este usuario no tiene acceso al panel de clientes. Por favor, usa el panel de administración.";
                        return View("/Views/User/Login.cshtml");
                    }

                    // Actualizar último acceso
                    usuario.UltimoAcceso = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    // Guardar en sesión
                    HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                    HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                    HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());
                    HttpContext.Session.SetString("UsuarioRol", usuario.Rol);
                    
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
        public async Task<IActionResult> RegisterUser(string nombre, string apellido, string email, 
            string contra, string telefono, string documento)
        {
            try
            {
                _logger.LogInformation($"Intento de registro para email: {email}");

                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(nombre) || 
                    string.IsNullOrWhiteSpace(apellido) || 
                    string.IsNullOrWhiteSpace(email) || 
                    string.IsNullOrWhiteSpace(contra) || 
                    string.IsNullOrWhiteSpace(telefono) ||
                    string.IsNullOrWhiteSpace(documento))
                {
                    ViewBag.Mensaje = "Todos los campos son obligatorios";
                    return View("/Views/User/Login.cshtml");
                }

                // Verificar si el email ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
                {
                    ViewBag.Mensaje = "El correo ya está registrado";
                    return View("/Views/User/Login.cshtml");
                }

                // Crear nuevo usuario como huésped
                var usuario = new Usuario
                {
                    Nombre = nombre.Trim(),
                    Apellido = apellido.Trim(),
                    Email = email.Trim().ToLower(),
                    Contraseña = contra,
                    Telefono = telefono.Trim(),
                    Documento = documento.Trim(),
                    Rol = "huesped",
                    Estado = "activo",
                    FechaCreacion = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Crear registro de huésped asociado
                var huesped = new Huesped
                {
                    UsuarioId = usuario.UsuarioId
                };
                
                _context.Huespedes.Add(huesped);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"Usuario registrado exitosamente. ID: {usuario.UsuarioId}");

                TempData["Mensaje"] = "Cuenta creada exitosamente. Ingresa tus datos para iniciar sesión.";
                
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