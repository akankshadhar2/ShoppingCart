namespace ShoppingCart.Models.DTO
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string OrderStatus { get; set; } = "Unplaced";
    }
}
