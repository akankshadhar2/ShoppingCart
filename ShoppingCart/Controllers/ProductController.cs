using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public ProductController(IProductService productService, ICartService cartService, IUserService userService, IAdminService adminService)
        {
            _productService = productService;
            _cartService = cartService;
            _userService = userService;
            _adminService = adminService;
        }

        [HttpGet("User/ProductCatalogue")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("User/ProductSearchById/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("User/SearchProductsByName")]
        public async Task<IActionResult> SearchProducts(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Please provide a search term");
            }

            var products = await _productService.SearchProductsByNameAsync(searchTerm);
            if (products == null || products.Count == 0)
            {
                return NotFound("Item not found");
            }
            return Ok(products);
        }

        [HttpPost("Admin/AddProduct")]
        public async Task<IActionResult> CreateProduct([FromHeader] string username, [FromHeader] string password, ProductDTO productDto)
        {
            if (!await _adminService.IsAdmin(username, password))
            {
                return Unauthorized();
            }

            await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = productDto.ProductId }, productDto);
        }

        [HttpPut("Admin/UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct([FromHeader] string username, [FromHeader] string password, Guid id, ProductDTO productDto)
        {
            if (!await _adminService.IsAdmin(username, password))
            {
                return Unauthorized();
            }

            if (await _productService.UpdateProductAsync(id, productDto))
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("Admin/DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromHeader] string username, [FromHeader] string password, Guid id)
        {
            if (!await _adminService.IsAdmin(username, password))
            {
                return Unauthorized();
            }

            if (await _productService.DeleteProductAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPost("User/AddToCart")]
        public async Task<IActionResult> AddToCart(string username, Guid productId)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
            {
                return BadRequest("Invalid username");
            }

            var result = await _cartService.AddItemToCart(user.UserId, productId);
            if (result)
            {
                return Ok("Item added to cart");
            }
            else
            {
                return BadRequest("Error adding item to cart");
            }
        }

        [HttpPost("User/DeleteFromCart")]
        public async Task<IActionResult> DeleteFromCart(string username, Guid productId)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
            {
                return BadRequest("Invalid username");
            }

            var result = await _cartService.DeleteItemFromCart(user.UserId, productId);
            if (result)
            {
                return Ok("Item deleted from cart");
            }
            else
            {
                return BadRequest("Error deleting item from cart");
            }

        }
    }
}





