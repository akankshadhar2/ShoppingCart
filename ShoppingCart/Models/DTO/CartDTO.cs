namespace ShoppingCart.Models.DTO
{
    public class CartDTO
    {
        public Guid UserId { get; set; }
      
        public List<CartItemDTO> CartItems { get; set; }
       
        

    }
}
