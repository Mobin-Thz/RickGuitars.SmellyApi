using System.ComponentModel.DataAnnotations;

namespace RickGuitars.SmellyApi.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public List<OrderItem> Items { get; set; } = new();

    public decimal Subtotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal Total { get; set; }

    // Smell: string status instead of enum or state model.
    public string Status { get; set; } = "Pending";

    public string PaymentStatus { get; set; } = "NotPaid";

    public string ShippingAddress { get; set; } = string.Empty;

    public string TrackingNumber { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? PaidAt { get; set; }

    public DateTime? ShippedAt { get; set; }
}