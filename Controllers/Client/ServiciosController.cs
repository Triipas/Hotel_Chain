using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers
{
    public class ServiciosController : Controller
    {
        // Acción principal: Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
