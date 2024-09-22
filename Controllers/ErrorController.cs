using Microsoft.AspNetCore.Mvc;

namespace TechWaveOnlineShopping.Controllers
{
    public class ErrorController : Controller
    {
        // Unauthorized error action
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
