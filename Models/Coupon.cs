using System.ComponentModel.DataAnnotations;

namespace RickGuitars.SmellyApi.Models;

public class Coupon
{
    [Key]
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    // Smell: string-based discount type.
    // Examples: "Percentage", "FixedAmount", "FreeShipping"
    public string DiscountType { get; set; } = string.Empty;

    public decimal Value { get; set; }

    public bool IsActive { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public decimal? MinimumOrderAmount { get; set; }

    public bool VipOnly { get; set; }
}