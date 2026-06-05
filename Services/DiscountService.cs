using Microsoft.EntityFrameworkCore;
using RickGuitars.SmellyApi.Data;
using RickGuitars.SmellyApi.Models;
using RickGuitars.SmellyApi.Shared;

namespace RickGuitars.SmellyApi.Services;

public class DiscountService
{
    private readonly RickGuitarsDbContext _db;

    public DiscountService(RickGuitarsDbContext db)
    {
        _db = db;
    }

    public async Task<decimal> CalculateDiscountAsync(
        string couponCode,
        Customer customer,
        decimal subtotal)
    {
        var discount = 0m;

        // Smell: global discount affects checkout without being explicit.
        if (ShopGlobalState.GlobalDiscountRate > 0)
        {
            discount += subtotal * ShopGlobalState.GlobalDiscountRate;
        }

        if (string.IsNullOrWhiteSpace(couponCode))
            return discount;

        var coupon = await _db.Coupons
            .FirstOrDefaultAsync(c => c.Code == couponCode);

        if (coupon == null)
            return discount;

        if (!coupon.IsActive)
            return discount;

        if (coupon.ExpiresAt.HasValue && coupon.ExpiresAt.Value < DateTime.UtcNow)
            return discount;

        if (coupon.MinimumOrderAmount.HasValue && subtotal < coupon.MinimumOrderAmount.Value)
            return discount;

        if (coupon.VipOnly && !customer.IsVip)
            return discount;

        // Smell: string-based discount type.
        // Adding a new discount type requires changing this if/else block.
        if (coupon.DiscountType == "Percentage")
        {
            discount += subtotal * (coupon.Value / 100);
        }
        else if (coupon.DiscountType == "FixedAmount")
        {
            discount += coupon.Value;
        }
        else if (coupon.DiscountType == "FreeShipping")
        {
            // Smell: this does not actually calculate shipping;
            // it relies on another service to know about the same string concept.
            discount += 0;
        }

        return discount;
    }
}