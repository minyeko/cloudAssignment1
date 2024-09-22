using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechWaveOnlineShopping.Data;
using TechWaveOnlineShopping.Helpers;
using TechWaveOnlineShopping.Models;

namespace TechWaveOnlineShopping.Controllers
{
    public class ProductController : ControllerExtensions
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            var result = IsAdminUser();
            if (result != null)
            {
                return result; // Only allow admin users to add products
            }
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile photoFile)
        {
            var result = IsAdminUser();
            if (result != null)
            {
                return result; // Only allow admin users to add products
            }

            if (ModelState.IsValid)
            {
                // Handle file upload
                if (photoFile != null && photoFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photoFile.CopyToAsync(memoryStream);
                        product.Photo = memoryStream.ToArray(); // Convert file to byte array
                    }
                }

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(product);
        }


        // This action returns the image file from the database
        public IActionResult GetImage(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product != null && product.Photo != null)
            {
                return File(product.Photo, "image/jpeg"); // Return the image byte array
            }

            return NotFound(); // Return 404 if image or product is not found
        }

    }


    

}
