using Microsoft.AspNetCore.Mvc;

using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{

    [ApiController]
    [Route(WebAPIAddrsses.Orders)]
    public class OrdersAPIController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersAPIController(IOrderService orderService) => _orderService = orderService;


        [HttpGet("user/{UserName}")]
        public async Task<IActionResult> GetUserOrders(string UserName)
        {
            var order = await _orderService.GetUserOrdersAsync(UserName);

            return Ok(order.ToDTO());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrderById(int Id)
        {
            var order = await _orderService.GetOrderByIdAsync(Id);
            if (order == null)
                return NotFound();
            return Ok(order.ToDTO());
        }

        [HttpPost("{UserName}")]
        public async Task<IActionResult> CreateOrder(string UserName, [FromBody] CreateOrderDTO model)
        {
            var order = await _orderService.CreateOrderAsync(UserName, model.Items.ToCartView(), model.Order);

            return CreatedAtAction(nameof(GetOrderById), new {order.Id}, order.ToDTO());
        }
    }
}
