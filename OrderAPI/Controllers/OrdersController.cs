using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hangfire;

using OrderAPI.Models;
using OrderAPI.Requests;
using OrderAPI.Services;
using OrderAPI.DTO;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderRequest createOrder)
        {
            Order newOrder = new();
            newOrder.CustomerName = createOrder.CustomerName;
            newOrder.CreatedDate = DateTime.UtcNow;
            newOrder.Status = OrderStatus.New;
            newOrder.Items = new List<OrderItem>();

            var orderItemResponses = await ProcessOrderItems(newOrder, createOrder.Items);
            await orderService.CreateOrderAsync(newOrder);

            OrderDto orderResponse = new();
            orderResponse.OrderId = newOrder.OrderId;
            orderResponse.CustomerName = newOrder.CustomerName;
            orderResponse.CreatedDateTime = newOrder.CreatedDate;
            orderResponse.Status = newOrder.Status;
            orderResponse.Items = orderItemResponses;
             
            return CreatedAtAction(
                actionName:     nameof(GetOrder), 
                routeValues:    new { orderId = newOrder.OrderId }, 
                value:          orderResponse
               );
        }

        [HttpPost("RemoveProduct")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            await orderService.DestroyProductAsync(productId);
            return Accepted();
        }

        [HttpPost("Pay")]
        public async Task<IActionResult> PayOrder(int orderId, [FromBody] bool isPaid)
        {
            bool orderExists = await orderService.OrderExists(orderId);
            if (orderExists)
            {
                BackgroundJob.Enqueue(() => UpdateOrderStatus(orderId, isPaid));
                return Accepted();
            }
            return Problem();
        }

        [NonAction]
        public async Task UpdateOrderStatus(int orderId, bool isPaid)
        {
            await orderService.UpdateOrderAsync(orderId, (order) =>
            {
                order.Status = isPaid ? OrderStatus.Paid : OrderStatus.Cancelled;
            });
        }

        [HttpGet("GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int orderId)
        {
            var order = await orderService.GetOrderAsync(orderId);
            if (order != null)
            {               
                return Ok(order);
            }
            else
            {
                return Problem();
            }
        }

      
        [HttpGet("GetOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        
        private async Task<List<OrderItemDto>> ProcessOrderItems(Order newOrder, IEnumerable<OrderItemRequest> itemRequests)
        {
            List<OrderItemDto> orderItemResponses = new();
            foreach (var orderItem in  itemRequests)
            {
                OrderItem newOrderItem = new();
                Product? product = await orderService.GetProductAsync(orderItem.ProductId);
                if (product != null)
                {
                    newOrderItem.Quantity = orderItem.Quantity;
                    newOrderItem.Order = newOrder;
                    newOrderItem.Product = product;
                    newOrder.Items.Add(newOrderItem);

                    OrderItemDto orderItemResponse = new();
                    orderItemResponse.ProductName = product.ProductName;
                    orderItemResponse.Quantity = orderItem.Quantity;
                    orderItemResponse.PricePerUnit = product.PricePerUnit;
                    orderItemResponses.Add(orderItemResponse);
                }
            }
            return orderItemResponses;
        }
    }
}
