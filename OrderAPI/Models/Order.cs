using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderAPI.Requests;

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
        [Key]
        public int OrderId { get; set; }
        public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> Items { get;  set; }
    }
}
