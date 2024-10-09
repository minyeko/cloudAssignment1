using System.ComponentModel.DataAnnotations;

namespace TechWaveOnlineShopping.Models
{
	public class Order
	{
		public int OrderId { get; set; } // Primary Key

		[Required]
		public int CustomerId { get; set; } // Foreign Key to Customer table

		[Required]
		public decimal TotalAmount { get; set; } // Total price of the order

		[Required]
		[StringLength(50)]
		public string OrderStatus { get; set; } // E.g. Pending, Paid, Shipped

		[Required]
		public DateTime CreatedDate { get; set; } // When the order was placed

		public DateTime? ModifiedDate { get; set; } // Last modification time

		// Navigation property
		public ICollection<OrderItem> OrderItems { get; set; }
	}

}
