using System.ComponentModel.DataAnnotations;

namespace TechWaveOnlineShopping.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }

        // New Photo property to store the path of the uploaded image
        public byte[]? Photo { get; set; } // Add Photo as byte array to store image
    }

}
