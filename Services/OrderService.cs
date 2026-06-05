using Microsoft.EntityFrameworkCore;
using RickGuitars.SmellyApi.Data;
using RickGuitars.SmellyApi.Models;
using RickGuitars.SmellyApi.Shared;

namespace RickGuitars.SmellyApi.Services;

public class OrderService
{
    private readonly RickGuitarsDbContext _db;
    private readonly PaymentService _paymentService;
    private readonly ShippingService _shippingService;
    private readonly DiscountService _discountService;

    public OrderService(
        RickGuitarsDbContext db,
        PaymentService paymentService,
        ShippingService shippingService,
        DiscountService discountService)
    {
        _db = db;
        _paymentService = paymentService;
        _shippingService = shippingService;
        _discountService = discountService;
    }

    public async Task<Order> CheckoutAsync(
        int customerId,
        string shippingAddress,
        PaymentInfo paymentInfo,
        string couponCode)
    {
        if (ShopGlobalState.MaintenanceMode)
            throw new Exception("Checkout is currently disabled.");

        var customer = await _db.Customers.FindAsync(customerId);

        if (customer == null)
            throw new Exception("Customer not found.");

        var cart = await _db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (cart == null || cart.Items.Count == 0)
            throw new Exception("Cart is empty.");

        foreach (var item in cart.Items)
        {
            if (item.Product == null)
                throw new Exception("Product not loaded.");

            if (item.Product.StockQuantity < item.Quantity)
                throw new Exception($"Not enough stock for {item.Product.Name}.");
        }

        // Smell: duplicated calculation logic.
        var subtotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);

        var discount = await _discountService.CalculateDiscountAsync(
            couponCode,
            customer,
            subtotal);

        var shippingCost = _shippingService.CalculateShipping(
            shippingAddress,
            cart.Items.Sum(i => i.Quantity),
            subtotal,
            couponCode);

        var total = subtotal - discount + shippingCost;

        var transactionId = _paymentService.Charge(paymentInfo, total);

        var trackingNumber = _shippingService.CreateShipment(shippingAddress);

        var order = new Order
        {
            CustomerId = customerId,
            ShippingAddress = shippingAddress,
            Subtotal = subtotal,
            DiscountAmount = discount,
            ShippingCost = shippingCost,
            Total = total,

            // Smell: string statuses.
            Status = "Paid",
            PaymentStatus = "Paid",
            TrackingNumber = trackingNumber,
            PaidAt = DateTime.UtcNow
        };

        foreach (var cartItem in cart.Items)
        {
            if (cartItem.Product == null)
                throw new Exception("Product not loaded.");

            order.Items.Add(new OrderItem
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                SerialNumber = cartItem.Product.SerialNumber,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                TotalPrice = cartItem.UnitPrice * cartItem.Quantity
            });

            // Smell: OrderService also updates inventory.
            cartItem.Product.StockQuantity -= cartItem.Quantity;
        }

        // Smell: OrderService also clears the cart.
        cart.Items.Clear();
        cart.Subtotal = 0;
        cart.DiscountAmount = 0;
        cart.ShippingCost = 0;
        cart.Total = 0;
        cart.AppliedCouponCode = string.Empty;
        cart.UpdatedAt = DateTime.UtcNow;

        _db.Orders.Add(order);

        await _db.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> GetOrderAsync(int orderId)
    {
        return await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order?> MarkAsShippedAsync(int orderId)
    {
        var order = await _db.Orders.FindAsync(orderId);

        if (order == null)
            return null;

        if (order.Status != "Paid")
            throw new Exception("Only paid orders can be shipped.");

        order.Status = "Shipped";
        order.ShippedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return order;
    }
}