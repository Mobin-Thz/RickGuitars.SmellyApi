using Microsoft.AspNetCore.Mvc;
using RickGuitars.SmellyApi.Models;
using RickGuitars.SmellyApi.Services;

namespace RickGuitars.SmellyApi.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{customerId:int}")]
    public async Task<ActionResult<Cart>> GetCart(int customerId)
    {
        var cart = await _cartService.GetCartAsync(customerId);

        if (cart == null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPost("{customerId:int}/items")]
    public async Task<ActionResult<Cart>> AddItem(
        int customerId,
        [FromBody] AddCartItemRequest request)
    {
        try
        {
            var cart = await _cartService.AddItemAsync(
                customerId,
                request.ProductId,
                request.Quantity);

            return Ok(cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{customerId:int}/items/{productId:int}")]
    public async Task<ActionResult<Cart>> UpdateItemQuantity(
        int customerId,
        int productId,
        [FromBody] UpdateCartItemRequest request)
    {
        try
        {
            var cart = await _cartService.UpdateItemQuantityAsync(
                customerId,
                productId,
                request.Quantity);

            return Ok(cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{customerId:int}/apply-coupon")]
    public async Task<ActionResult<Cart>> ApplyCoupon(
        int customerId,
        [FromBody] ApplyCouponRequest request)
    {
        try
        {
            var cart = await _cartService.ApplyCouponAsync(customerId, request.CouponCode);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

// Smell: request models are placed inside controller file.
// Later, these should move to a Contracts or DTOs folder.
public class AddCartItemRequest
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}

public class UpdateCartItemRequest
{
    public int Quantity { get; set; }
}

public class ApplyCouponRequest
{
    public string CouponCode { get; set; } = string.Empty;
}