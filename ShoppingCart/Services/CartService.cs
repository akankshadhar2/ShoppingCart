using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models.Domain;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;

public class CartService : ICartService
{
    private readonly ShoppingCartDbContext _context;

    public CartService(ShoppingCartDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateCart(CartDTO cartDto)
    {
        var user = await _context.Users.FindAsync(cartDto.UserId);
        if (user == null)
        {
            throw new Exception($"User with ID: {cartDto.UserId} not found");
        }

        var cart = new Cart
        {
            CartId = cartDto.CartId,
            UserId = cartDto.UserId,
            User = user
        };

        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCart(Guid id, CartDTO cartDto)
    {
        var cart = await _context.Carts
          .Include(c => c.User)
          .FirstOrDefaultAsync(c => c.CartId == id);

        if (cart == null)
        {
            return false;
        }

        cart.UserId = cartDto.UserId;

        _context.Entry(cart).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCart(Guid id)
    {
        var cart = await _context.Carts.FindAsync(id);
        if (cart == null)
        {
            return false;
        }

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddItemToCart(Guid userId, Guid productId)
    {
        // Check if user exists
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID: {userId} not found");
        }

        // Check if user has a cart, create one if not
        Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart();
            cart.UserId = userId;
            cart.CartId = Guid.NewGuid();
            cart.User = user;

            _context.Carts.Add(cart);
        }

        // Add or update CartItem for the product
        CartItem cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.ProductId == productId);
        if (cartItem == null)
        {
            cartItem = new CartItem { CartId = cart.CartId, ProductId = productId, Quantity = 1, UserId = userId };
            _context.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += 1;
        }

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<CartSummaryDTO> GetUserCart(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID: {userId} not found");
        }

        var cart = await _context.Carts
          .Include(c => c.User)
          .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return null;  // Indicate no cart found
        }

        var cartDto = new CartSummaryDTO
        {
            UserId = cart.UserId,
            UserName = cart.User?.UserName
        };

        var cartItems = await _context.CartItems
          .Include(ci => ci.Product)
          .ToListAsync(CancellationToken.None);

        var productDtos = new List<ProductDTO>();
        foreach (var cartItem in cartItems)
        {
            if (cartItem.Product != null)
            {
                productDtos.Add(new ProductDTO
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Name = cartItem.Product.Name,
                    Price = cartItem.Product.Price
                });
            }
        }

        cartDto.productList = productDtos;
        cartDto.TotalPrice = productDtos.Sum(p => p.Price * p.Quantity);

        return cartDto;
    }
    public async Task<bool> DeleteItemFromCart(Guid userId, Guid productId)
    {
        // Check if user exists
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID: {userId} not found");
        }

        // Check if user has a cart
        Cart cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null)
        {
            throw new Exception("Cart not found");
        }

        // Find CartItem for the product
        CartItem cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.ProductId == productId);
        if (cartItem == null)
        {
            throw new Exception("Item not found in cart");
        }
        else
        {
            cartItem.Quantity -= 1;

            if (cartItem.Quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }


}


