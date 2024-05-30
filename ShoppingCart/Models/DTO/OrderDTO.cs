using ShoppingCart.Models.Domain;

namespace ShoppingCart.Models.DTO
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid CartId { get; set; }

        





    }


}
