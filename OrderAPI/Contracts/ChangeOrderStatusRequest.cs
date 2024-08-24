using OrderAPI.Models;

namespace OrderAPI.Requests
{
    public record ChangeOrderStatusRequest(
        Guid orderId,
        OrderStatus NewStatus
    );
}
