using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers
{
    public class SobreNosotrosController : Controller
    {
        // Acción principal: Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
