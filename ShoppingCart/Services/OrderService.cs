using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models.Domain;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;

public class OrderService : IOrderService
{
    private readonly ShoppingCartDbContext _context;
    private readonly IAdminService _adminService;
    private readonly IUserService _userService;
    private readonly IProductService _productService;

    public OrderService(ShoppingCartDbContext context, IProductService productService, IAdminService adminService, IUserService userService)
    {
        _context = context;
        _productService = productService;
        _adminService = adminService;
        _userService = userService;
    }

    public async Task<IActionResult> GetOrder(string username, string password, Guid OrderID)
    {
        if (!await _userService.ValidateCredentials(username, password))
        {
            return new UnauthorizedObjectResult("Invalid username or password");
        }

        Guid userId = await _userService.GetUserIdByUsername(username);
        var order = await _context.Orders.FindAsync(OrderID);

        return new OkObjectResult(order);
    }

    public async Task<IActionResult> CreateOrder(OrderDTO orderDto, string username, string password)
    {
        if (!await _adminService.IsAdmin(username, password))
        {
            return new UnauthorizedResult();
        }
        Order order = new Order
        {
            OrderStatus = orderDto.OrderStatus,
            TotalPrice = orderDto.TotalPrice,
            OrderId = orderDto.OrderId,
            CartId = orderDto.CartId
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return new OkObjectResult("Order Created");
    }

    public async Task<IActionResult> UpdateOrder(Guid id, OrderDTO orderDto, string username, string password)
    {
        if (!await _adminService.IsAdmin(username, password))
        {
            return new UnauthorizedResult();
        }
        Order order = await _context.Orders.FindAsync(id);
        if (order == null)
            return new NotFoundResult();

        order.OrderStatus = orderDto.OrderStatus;
        order.TotalPrice = orderDto.TotalPrice;
        order.OrderId = orderDto.OrderId;
        order.CartId = orderDto.CartId;

        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }

    public async Task<IActionResult> DeleteOrder(Guid id, string username, string password)
    {
        if (!await _adminService.IsAdmin(username, password))
        {
            return new UnauthorizedResult();
        }
        Order order = await _context.Orders.FindAsync(id);
        if (order == null)
            return new NotFoundResult();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }

    public async Task<IActionResult> Checkout(string username, string password)
    {
        Guid userId = await _userService.GetUserIdByUsername(username);
        if (userId == Guid.Empty)
        {
            return new BadRequestObjectResult("User not found");
        }

        Cart cart = await _context.Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
        {
            return new BadRequestObjectResult("Cart is empty");
        }

        var orderResult = await PlaceOrder(cart);
        if (orderResult.Success)
        {
            Guid orderId = orderResult.OrderId;
            return new OkObjectResult($"Order placed successfully. Order ID: {orderId}");
        }
        else
        {
            return new BadRequestObjectResult(orderResult.ErrorMessage);
        }
    }

        public async Task<OrderResult> PlaceOrder(Cart cart)
    {
        decimal sum = 0;
        if (cart == null)
        {
            return new OrderResult { Success = false, ErrorMessage = "Cart is empty" };
        }
        foreach (var cartItem in cart.CartItems)
        {
            var product = await _context.Products.FindAsync(cartItem.ProductId);
            sum += product.Price * cartItem.Quantity;
        }

        Order order = new Order
        {
            CartId = cart.CartId,
            OrderStatus = "Placed",
            TotalPrice = sum,
            UserId = cart.UserId
        };

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(_context.CartItems.Where(cartItem => cartItem.CartId == cart.CartId));

        await _context.SaveChangesAsync();
        return new OrderResult { Success = true, OrderId = order.OrderId };
    }
}

public class OrderResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }

    public Guid OrderId { get; set; }
}