

using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }
}
