using Microsoft.EntityFrameworkCore;
using RickGuitars.SmellyApi.Data;
using RickGuitars.SmellyApi.Models;

namespace RickGuitars.SmellyApi.Services;

public class CartService
{
    private readonly RickGuitarsDbContext _db;
    private readonly DiscountService _discountService;
    private readonly ShippingService _shippingService;

    public CartService(
        RickGuitarsDbContext db,
        DiscountService discountService,
        ShippingService shippingService)
    {
        _db = db;
        _discountService = discountService;
        _shippingService = shippingService;
    }

    public async Task<Cart?> GetCartAsync(int customerId)
    {
        return await _db.Carts
            .Include(c => c.Customer)
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<Cart> AddItemAsync(int customerId, int productId, int quantity)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero.");

        var customer = await _db.Customers.FindAsync(customerId);

        if (customer == null)
            throw new Exception("Customer not found.");

        var product = await _db.Products.FindAsync(productId);

        if (product == null || product.IsDeleted)
            throw new Exception("Product not found.");

        if (product.StockQuantity < quantity)
            throw new Exception("Not enough stock.");

        var cart = await GetCartAsync(customerId);

        if (cart == null)
        {
            cart = new Cart
            {
                CustomerId = customerId
            };

            _db.Carts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem == null)
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Product = product,
                Quantity = quantity,
                UnitPrice = product.Price
            });
        }
        else
        {
            existingItem.Quantity += quantity;
            existingItem.UnitPrice = product.Price;
        }

        await RecalculateCartAsync(cart, customer, cart.AppliedCouponCode);

        await _db.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> UpdateItemQuantityAsync(int customerId, int productId, int quantity)
    {
        var cart = await GetCartAsync(customerId);

        if (cart == null)
            throw new Exception("Cart not found.");

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
            throw new Exception("Cart item not found.");

        if (quantity <= 0)
        {
            cart.Items.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }

        if (cart.Customer == null)
            throw new Exception("Customer not loaded.");

        await RecalculateCartAsync(cart, cart.Customer, cart.AppliedCouponCode);

        await _db.SaveChangesAsync();

        return cart;
    }

    public async Task<Cart> ApplyCouponAsync(int customerId, string couponCode)
    {
        var cart = await GetCartAsync(customerId);

        if (cart == null)
            throw new Exception("Cart not found.");

        if (cart.Customer == null)
            throw new Exception("Customer not loaded.");

        cart.AppliedCouponCode = couponCode;

        await RecalculateCartAsync(cart, cart.Customer, couponCode);

        await _db.SaveChangesAsync();

        return cart;
    }

    private async Task RecalculateCartAsync(Cart cart, Customer customer, string couponCode)
    {
        // Smell: duplicated price/tax/discount calculation will also appear in OrderService.
        var subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
        var discount = await _discountService.CalculateDiscountAsync(couponCode, customer, subtotal);
        var shipping = _shippingService.CalculateShipping(
            customer.DefaultShippingAddress,
            cart.Items.Sum(i => i.Quantity),
            subtotal,
            couponCode);

        cart.Subtotal = subtotal;
        cart.DiscountAmount = discount;
        cart.ShippingCost = shipping;
        cart.Total = subtotal - discount + shipping;
        cart.UpdatedAt = DateTime.UtcNow;
    }
}