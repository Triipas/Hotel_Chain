using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers
{
    public class ServiciosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
