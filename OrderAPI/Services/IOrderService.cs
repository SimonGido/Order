using OrderAPI.Models;
using OrderAPI.DTO;

namespace OrderAPI.Services
{
    public interface IOrderService
    {
        public Task CreateProductAsync(Product product); // NOTE: Testing purposes

        public Task DestroyProductAsync(int productId); // NOTE: Testing purposes

        public Task CreateOrderAsync(Order order);

        public Task UpdateOrderAsync(int orderId, Action<Order> updateFunc);

        public Task<ICollection<OrderDto>> GetAllOrdersAsync();

        public Task<OrderDto?> GetOrderAsync(int orderId);

        public Task<Product?> GetProductAsync(int productId);

    }
}
