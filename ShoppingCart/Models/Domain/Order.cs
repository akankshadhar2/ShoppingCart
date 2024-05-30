using ShoppingCart.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key]
    public Guid OrderId { get; set; }

    public Guid CartId { get; set; }

    [ForeignKey("CartId")]
    public Cart Cart { get; set; }

    public Guid UserId { get; set; }

    public string OrderStatus { get; set; }
    public decimal TotalPrice { get; set; }

 
}
