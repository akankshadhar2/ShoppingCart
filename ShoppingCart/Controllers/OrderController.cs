using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;

namespace ShoppingCart.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("User/GetOrderDetails")]
        public async Task<IActionResult> GetOrder([FromHeader] string username, [FromHeader] string password, Guid OrderID)
        {
            return await _orderService.GetOrder(username, password, OrderID);
        }

        [HttpPost("Admin/CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDto, [FromHeader] string username, [FromHeader] string password)
        {
            return await _orderService.CreateOrder(orderDto, username, password);
        }

        [HttpPut("Admin/UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, OrderDTO orderDto, [FromHeader] string username, [FromHeader] string password)
        {
            return await _orderService.UpdateOrder(id, orderDto, username, password);
        }

        [HttpDelete("Admin/DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id, [FromHeader] string username, [FromHeader] string password)
        {
            return await _orderService.DeleteOrder(id, username, password);
        }

        [HttpPost("User/Checkout")]
        public async Task<IActionResult> Checkout([FromHeader] string username, [FromHeader] string password)
        {
            return await _orderService.Checkout(username, password);
        }
    }
}




