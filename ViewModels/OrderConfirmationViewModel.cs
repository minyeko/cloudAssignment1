
namespace TechWaveOnlineShopping.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemViewModel>OrderItems { get; set; } = new List<OrderItemViewModel>();
    }

}