using Microsoft.AspNetCore.Mvc;
using TechWaveOnlineShopping.Data;
using TechWaveOnlineShopping.Models;
using System.Threading.Tasks;
using TechWaveOnlineShopping.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TechWaveOnlineShopping.ViewModels;

namespace TechWaveOnlineShopping.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer/Register
        public IActionResult Register()
        
        {
            //// Get the underlying connection
            //var dbConnection = _context.Database.GetDbConnection();
            //var connectionString = dbConnection.ConnectionString;
            //_context.Database.CanConnect();
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Hash the password using BCrypt
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    // Create a new user entity without saving ConfirmPassword
                    var customer = new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Address= model.Address,
                        Password = hashedPassword // Hash this password for security
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Success"); // Redirect to a success page
                }
                catch (DbUpdateException ex)
                {
                    throw;
                }
                catch (DbException ex)
                {
                    throw;
                }
                catch (Exception ex) {
                    throw;
                }


            }
            return View(model);
        }

        // Success page
        public IActionResult Success()
        {
            return View();
        }
    }
}
