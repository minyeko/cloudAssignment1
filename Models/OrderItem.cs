using System.ComponentModel.DataAnnotations;

namespace TechWaveOnlineShopping.Models
{
	public class OrderItem
	{
		public int OrderItemId { get; set; } // Primary Key

		[Required]
		public int OrderId { get; set; } // Foreign Key to Order table

		[Required]
		public int ProductId { get; set; } // Foreign Key to Product table

		[Required]
		public int Quantity { get; set; } // Number of units of the product

		[Required]
		public decimal Price { get; set; } // Price of the product at the time of the order

		// Navigation property
		public Order Order { get; set; }
		public Product Product { get; set; }
	}

}
