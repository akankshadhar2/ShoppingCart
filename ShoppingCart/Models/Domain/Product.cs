using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models.Domain
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
