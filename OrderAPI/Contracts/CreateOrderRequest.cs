using OrderAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Requests
{
    public record OrderItemRequest(
        int     ProductId,
        int     Quantity
    );

    public record CreateOrderRequest(
        string                  CustomerName,
        List<OrderItemRequest>  Items
     );
}
