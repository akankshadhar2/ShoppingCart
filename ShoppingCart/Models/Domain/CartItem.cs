using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models.Domain
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }

        public Guid CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        public Guid ProductId { get; set; }
      
      

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public Guid UserId { get;  set; }
        
      
    }
}
