using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechWaveOnlineShopping.Data;
using TechWaveOnlineShopping.Helpers;
using TechWaveOnlineShopping.Models;

namespace TechWaveOnlineShopping.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Initialize the session-based cart if not already present
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This method retrieves the cart items and returns the ViewCart view
        public IActionResult ViewCart()
        {
            // Retrieve the cart from session
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart");

            // If the cart is null (empty), initialize it as an empty list
            if (cart == null)
            {
                cart = new List<Product>();
            }

            // Pass the cart to the View
            return View(cart);
        }

        private List<Product> GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart");
            if (cart == null)
            {
                cart = new List<Product>();
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return cart;
        }

        // Add product to the cart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
           var cart = GetCart();
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {

                // Check if the product already exists in the cart
                var existingProduct = cart.FirstOrDefault(p => p.Id == productId);
                if (existingProduct != null)
                {
                    // Update the quantity if the product is already in the cart
                    existingProduct.Quantity += quantity;
                }
                else
                {
                    // Add the product with the specified quantity
                    product.Quantity = quantity; // Set the quantity to the user-defined value
                    cart.Add(product);
                }

                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            // Return the updated cart count
            return Ok(new { count = cart.Count });
        }


        // Get cart count
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(cart.Count);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            // Fetch cart from session
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart");

            // Find the product in the cart
            var cartItem = cart.FirstOrDefault(c => c.Id == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }

            // Save updated cart back to session
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Return updated total for the product and the entire cart
            return Json(new { newTotalPrice = cartItem.Price * cartItem.Quantity, cartTotal = cart.Sum(c => c.Price * c.Quantity) });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            // Fetch cart from session
            var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart");

            // Remove the product from the cart
            cart.RemoveAll(c => c.Id == productId);

            // Save updated cart back to session
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Return the updated cart total
            return Json(new { cartTotal = cart.Sum(c => c.Price * c.Quantity) });
        }


		public int Checkout()
		{
			// Retrieve cart from session
			var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart");

			if (cart == null || !cart.Any())
			{
				// Handle empty cart case
				ModelState.AddModelError("", "Your cart is empty.");
				TempData["ErrorMessage"] = "Your cart is empty.";
				return 0;
			}

            // Assuming you already have the logged-in customer ID
            if (HttpContext.Session == null || HttpContext.Session.GetString("CustomerId") == null)
            {
				TempData["ErrorMessage"] = "Please login first before checkout.";
				return -1;
            }
            int customerId = int.Parse(HttpContext.Session.GetString("CustomerId").ToString());// Get from logged-in user (e.g., User.Identity)

			// Create new order
			var order = new Order
			{
				CustomerId = customerId,
				TotalAmount = cart.Sum(x => x.Quantity * x.Price), // Calculate total
				OrderStatus = "Pending",
				CreatedDate = DateTime.Now
			};

			_context.Orders.Add(order);
			_context.SaveChanges();

			// Add Order Items
			foreach (var item in cart)
			{
				var orderItem = new OrderItem
				{
					OrderId = order.OrderId,
					ProductId = item.Id,
					Quantity = item.Quantity,
					Price = item.Price // Price at the time of order
				};

				_context.OrderItems.Add(orderItem);
			}

			_context.SaveChanges();

			// Clear cart after successful checkout
			HttpContext.Session.Remove("Cart");

			// Redirect to a success page or order confirmation
			TempData["SuccessMessage"] = "Your order has been successfully placed!";
            return order.OrderId;
		}


	}
}
