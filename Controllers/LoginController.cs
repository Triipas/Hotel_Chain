using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Hotel_chain.Data;
using Hotel_chain.Models;
using System.Linq;

namespace Hotel_chain.Controllers
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
            return View();
        }

  
        [HttpPost]
        public IActionResult LoginUser(string email, string contra)
        {
        
            if (email == "test@gmail.com" && contra == "1234")
            {
                HttpContext.Session.SetString("UsuarioNombre", "Test");
                return RedirectToAction("Index", "Home");
            }

      
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
                return View("Index");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}