using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hangfire;

using OrderAPI.Models;
using OrderAPI.Data;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext context;

        public OrdersController(OrderDbContext context)
        {
            this.context = context;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            order.CreatedDate = DateTime.UtcNow;
            order.Status = OrderStatus.New;
            context.Orders.Add(order);

            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { orderId = order.OrderId }, order);
        }

        [HttpGet("GetOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await context.Orders.Include(o => o.Items).ToListAsync();
        }

        [HttpGet("GetOrder")]
        public async Task<ActionResult<Order>> GetOrder(int orderId)
        {
            var order = await context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                return NotFound();

            return order;
        }

        [HttpPost("Pay")]
        public IActionResult PayOrder(int id, [FromBody] bool isPaid)
        {
            BackgroundJob.Enqueue(() => UpdateOrderStatus(id, isPaid));
            return Accepted();
        }

        [NonAction]
        public async Task UpdateOrderStatus(int id, bool isPaid)
        {
            var order = await context.Orders.FindAsync(id);
            if (order == null) return;

            order.Status = isPaid ? OrderStatus.Paid : OrderStatus.Cancelled;
            await context.SaveChangesAsync();
        }

      
    }
}
