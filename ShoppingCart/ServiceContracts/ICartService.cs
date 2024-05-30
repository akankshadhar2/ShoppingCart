namespace ShoppingCart.ServiceContracts
{
    public interface ICartService
    {
        Task<bool> CreateCart(CartDTO cartDto);
        Task<bool> UpdateCart(Guid id, CartDTO cartDto);
        Task<bool> DeleteCart(Guid id);
        Task<bool> AddItemToCart(Guid userId, Guid productId);
        Task<bool> DeleteItemFromCart(Guid userId, Guid productId);
        Task<CartSummaryDTO> GetUserCart(Guid userId);
    }

}