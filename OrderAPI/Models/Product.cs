using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal PricePerUnit { get; set; }
    }
}
