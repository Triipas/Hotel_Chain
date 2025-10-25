using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers
{
    public class SobreNosotrosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
