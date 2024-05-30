using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.Domain;
using ShoppingCart.Models.DTO;
using System.Threading.Tasks;
using static OrderService;

namespace ShoppingCart.ServiceContracts
{
    public interface IOrderService
    {
        Task<OrderResult> PlaceOrder(Cart cart);
        Task<IActionResult> GetOrder(string username, string password, Guid OrderID);

        Task<IActionResult> CreateOrder(OrderDTO orderDto, string username, string password);
        Task<IActionResult> UpdateOrder(Guid id, OrderDTO orderDto, string username, string password);
        Task<IActionResult> DeleteOrder(Guid id, string username, string password);
        Task<IActionResult> Checkout(string username, string password);
    }

}
