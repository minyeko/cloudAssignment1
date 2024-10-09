using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechWaveOnlineShopping.Data;
using TechWaveOnlineShopping.Helpers;
using TechWaveOnlineShopping.Models;
using TechWaveOnlineShopping.ViewModels;

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


		public IActionResult Index()
		{
			var result = IsAdminUser();
			if (result != null)
			{
				return result; // Only allow admin users to add products
			}

			// Fetch your products from the database or service
			var products = _context.Products.OrderByDescending(x => x.Id).ToList();

			// If you're using a ViewModel, map your products to the ViewModel
			var productViewModels = products.Select(p => new ProductViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Description = p.Description,
				Quantity = p.Quantity
			});

			// Pass the data to the view
			return View(productViewModels);//productViewModels
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


		public IActionResult Edit(int id)
		{
            var result = IsAdminUser();
            if (result != null)
            {
                return result; // Only allow admin users to add products
            }

            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? PhotoFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the product from the database
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Update the product fields
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Quantity = product.Quantity;

                    // Check if a new photo has been uploaded
                    if (PhotoFile != null && PhotoFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await PhotoFile.CopyToAsync(memoryStream);
                            existingProduct.Photo = memoryStream.ToArray(); // Store the image as byte array
                        }
                    }

                    // Save changes to the database
                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Product updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return to the Edit view with the current model
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


        public IActionResult View(int id)
		{
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

			if (product == null)
			{
				return NotFound();
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
