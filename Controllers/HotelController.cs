using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models;
using Hotel_chain.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hotel_chain.Controllers
{
    public class HotelController : Controller
    {
        private readonly AppDbContext _context;

        public HotelController(AppDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index(string ubicacion, string nombre)
{
    var query = _context.Hoteles.Include(h => h.Imagenes).AsQueryable();

 
    if (!string.IsNullOrEmpty(ubicacion))
        query = query.Where(h => h.Ciudad == ubicacion);

  
    if (!string.IsNullOrEmpty(nombre))
        query = query.Where(h => h.Nombre.Contains(nombre));

   
    var hoteles = query.OrderBy(h => h.Nombre).ToList();

    return View(hoteles);
}
    }
}