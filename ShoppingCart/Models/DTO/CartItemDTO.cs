namespace ShoppingCart.Models.DTO
{
    public class CartItemDTO
    {
        public Guid CartId { get; set; }
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

       

    }
}
