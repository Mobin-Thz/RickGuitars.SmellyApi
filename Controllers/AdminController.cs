using Microsoft.AspNetCore.Mvc;
using RickGuitars.SmellyApi.Data;
using RickGuitars.SmellyApi.Shared;

namespace RickGuitars.SmellyApi.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly RickGuitarsDbContext _db;

    public AdminController(RickGuitarsDbContext db)
    {
        _db = db;
    }

    [HttpPost("products/{productId:int}/stock")]
    public async Task<IActionResult> UpdateStock(
        int productId,
        [FromBody] UpdateStockRequest request)
    {
        // Smell: Controller directly uses DbContext.
        var product = await _db.Products.FindAsync(productId);

        if (product == null)
            return NotFound();

        product.StockQuantity = request.StockQuantity;

        await _db.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPost("products/{productId:int}/feature")]
    public async Task<IActionResult> SetFeatured(
        int productId,
        [FromBody] SetFeaturedRequest request)
    {
        // Smell: Controller directly uses DbContext.
        var product = await _db.Products.FindAsync(productId);

        if (product == null)
            return NotFound();

        product.IsFeatured = request.IsFeatured;

        await _db.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPost("global/free-shipping")]
    public IActionResult SetFreeShipping([FromBody] SetFreeShippingRequest request)
    {
        // Smell: Controller changes global mutable state.
        ShopGlobalState.FreeShippingEnabled = request.Enabled;

        return Ok(new
        {
            message = "Free shipping setting changed.",
            freeShippingEnabled = ShopGlobalState.FreeShippingEnabled
        });
    }

    [HttpPost("global/discount")]
    public IActionResult SetGlobalDiscount([FromBody] SetGlobalDiscountRequest request)
    {
        // Smell: Controller changes global mutable state.
        ShopGlobalState.GlobalDiscountRate = request.DiscountRate;

        return Ok(new
        {
            message = "Global discount setting changed.",
            globalDiscountRate = ShopGlobalState.GlobalDiscountRate
        });
    }

    [HttpGet("reports/sales")]
    public IActionResult GetSalesReport()
    {
        // Smell: reporting logic in controller.
        var orders = _db.Orders.ToList();

        var totalSales = orders.Sum(o => o.Total);
        var totalOrders = orders.Count;
        var averageOrderValue = totalOrders == 0 ? 0 : totalSales / totalOrders;

        return Ok(new
        {
            totalSales,
            totalOrders,
            averageOrderValue
        });
    }
}

public class UpdateStockRequest
{
    public int StockQuantity { get; set; }
}

public class SetFeaturedRequest
{
    public bool IsFeatured { get; set; }
}

public class SetFreeShippingRequest
{
    public bool Enabled { get; set; }
}

public class SetGlobalDiscountRequest
{
    public decimal DiscountRate { get; set; }
}