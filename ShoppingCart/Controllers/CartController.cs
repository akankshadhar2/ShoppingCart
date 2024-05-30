using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;
using System.Threading.Tasks;
using ShoppingCart.Models.Domain;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{

    [ApiController]
    //[Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public CartController(ICartService cartService, IUserService userService, IAdminService adminService)
        {
            _cartService = cartService;
            _userService = userService;
            _adminService = adminService;
        }

        [HttpPost("Admin/CreateCart")]
        public async Task<IActionResult> CreateCart(CartDTO cartDto, [FromHeader] string username, [FromHeader] string password)
        {
            if (!await _adminService.IsAdmin(username, password))
            {
                return Unauthorized();
            }

            var created = await _cartService.CreateCart(cartDto);
            return created ? Ok("Created") : BadRequest("Cart creation failed");
        }

        [HttpPut("Admin/UpdateCart/{id}")]
        public async Task<IActionResult> UpdateCart(Guid id, CartDTO cartDto, [FromHeader] string username, [FromHeader] string password)
        {
            if (!await _adminService.IsAdmin(username, password))
            {
                return Unauthorized();
            }

            var updated = await _cartService.UpdateCart(id, cartDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("Admin/DeleteCart/{id}")]
        public async Task<IActionResult> DeleteCart(Guid id, [FromHeader] string username, [FromHeader] string password)
        {
            if (!await _adminService.IsAdmin(username, password))
            {
                return Unauthorized();
            }
            var deleted = await _cartService.DeleteCart(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("User/GetCart")]
        public async Task<IActionResult> GetUserCart([FromHeader] string username, [FromHeader] string password)
        {
            
            Guid userId = await _userService.GetUserIdByUsername(username);
            var cartDto = await _cartService.GetUserCart(userId); 

            if (cartDto == null)
            {
                return NotFound();
            }

            return Ok(cartDto);
        }
    }
}


public class CartDTO
{
    public Guid CartId { get; set; }
    public List<Product> CartItems { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}
public class CartSummaryDTO
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }


    public decimal TotalPrice { get; set; }
    public int Quantity { get; set; }
    public List<ProductDTO> productList { get; set; }

}