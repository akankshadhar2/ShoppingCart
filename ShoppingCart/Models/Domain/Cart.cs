using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models.Domain
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }

        
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public List<CartItem> CartItems { get; set; }
        public Order Order { get; set; }
    }
}
