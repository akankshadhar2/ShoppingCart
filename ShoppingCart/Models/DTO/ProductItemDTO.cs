namespace ShoppingCart.Models.DTO
{
    public class ProductItemDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
