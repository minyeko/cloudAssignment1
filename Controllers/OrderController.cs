using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechWaveOnlineShopping.Data;
using TechWaveOnlineShopping.ViewModels;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Confirmation(int orderId)
    {
        // Fetch order details from the database using the orderId
        var order = _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            return NotFound();
        }

        var customer = _context.Customers.Where(x => x.CustomerId == order.CustomerId).FirstOrDefault();

        var model = new OrderConfirmationViewModel
        {
            OrderId = order.OrderId,
            CustomerName = customer.FirstName + " " + customer.LastName, // Assuming Customer is a navigation property
            OrderStatus = order.OrderStatus,
            CreatedDate = order.CreatedDate,
            TotalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity),
            OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
            {
                ProductName = _context.Products.FirstOrDefault(p => p.Id == oi.ProductId).Name, // Assuming Product is a navigation property
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList()
        };

        return View(model);
    }
}
