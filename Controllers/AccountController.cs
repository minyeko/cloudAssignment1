using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechWaveOnlineShopping.Data;
using TechWaveOnlineShopping.ViewModels;

namespace TechWaveOnlineShopping.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.Customers.SingleOrDefault(u => u.Email == model.Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                // Password is correct, proceed with login
                // (e.g., set session, cookies, etc.)
                HttpContext.Session.SetString("FirstName", user.FirstName);
                HttpContext.Session.SetString("CustomerId", user.CustomerId.ToString());
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                
                // Set a login success message
                TempData["LoginMessage"] = $"Welcome, {user.FirstName}!";

                return RedirectToAction("Index", "Home");
            }

            // Invalid login attempt
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            
            // Set a logout message
            TempData["LogoutMessage"] = "Logout successful.";

            // Redirect to the login page or another page
            return RedirectToAction("Login");

        }

    }
}
