using Microsoft.AspNetCore.Mvc;
using RickGuitars.SmellyApi.Models;
using RickGuitars.SmellyApi.Services;

namespace RickGuitars.SmellyApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<Order>> Checkout([FromBody] CheckoutRequest request)
    {
        try
        {
            var order = await _orderService.CheckoutAsync(
                request.CustomerId,
                request.ShippingAddress,
                request.PaymentInfo,
                request.CouponCode);

            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<Order>> GetOrder(int orderId)
    {
        var order = await _orderService.GetOrderAsync(orderId);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet("customer/{customerId:int}")]
    public async Task<ActionResult<List<Order>>> GetCustomerOrders(int customerId)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId);
        return Ok(orders);
    }

    [HttpPost("{orderId:int}/ship")]
    public async Task<ActionResult<Order>> MarkAsShipped(int orderId)
    {
        try
        {
            var order = await _orderService.MarkAsShippedAsync(orderId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

// Smell: API request directly uses PaymentInfo model.
// PaymentInfo contains card data and is part of internal model layer.
public class CheckoutRequest
{
    public int CustomerId { get; set; }

    public string ShippingAddress { get; set; } = string.Empty;

    public PaymentInfo PaymentInfo { get; set; } = new();

    public string CouponCode { get; set; } = string.Empty;
}