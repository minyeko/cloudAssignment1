using Microsoft.AspNetCore.Mvc;

namespace TechWaveOnlineShopping.Helpers
{
    // Extend the default Controller class to add reusable logic
    public class ControllerExtensions : Controller
    {
        // Method to check if the user is authenticated and is in the "Admin" role
        protected IActionResult IsAdminUser()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("IsAdmin") != null)
            {
                var isAdmin = bool.Parse(HttpContext.Session.GetString("IsAdmin"));
                if(isAdmin == true)
                {
                    return null;
                }
            }
            return RedirectToAction("Unauthorized", "Error");
        }
    }
}
