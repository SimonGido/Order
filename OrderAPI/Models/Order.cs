
using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models
{
    public enum OrderStatus
    {
        New,
        Paid,
        Cancelled
    }
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
