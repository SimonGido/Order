using OrderAPI.Models;

namespace OrderAPI.DTO
{

    public class OrderItemDto
    {
        public string   ProductName { get; set; }
        public int      Quantity { get; set; }
        public decimal  PricePerUnit { get; set; }
    }
    public class OrderDto
    {
        public int                 OrderId { get; set; }
        public string              CustomerName { get; set; }
        public DateTime            CreatedDateTime { get; set; }
        public OrderStatus         Status { get; set; }
        public List<OrderItemDto>  Items { get; set; }
    }
}
