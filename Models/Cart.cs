using System.ComponentModel.DataAnnotations;
namespace RickGuitars.SmellyApi.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public List<CartItem> Items { get; set; } = new();

    // Smell: calculated values are stored and can become inconsistent.
    public decimal Subtotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal Total { get; set; }

    public string AppliedCouponCode { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}