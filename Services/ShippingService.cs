using RickGuitars.SmellyApi.External;
using RickGuitars.SmellyApi.Shared;

namespace RickGuitars.SmellyApi.Services;

public class ShippingService
{
    private readonly FakeShippingClient _shippingClient;

    public ShippingService(FakeShippingClient shippingClient)
    {
        _shippingClient = shippingClient;
    }

    public decimal CalculateShipping(string shippingAddress, int totalItems, decimal subtotal, string couponCode)
    {
        // Smell: common coupling.
        if (ShopGlobalState.FreeShippingEnabled)
            return 0;

        // Smell: duplicate coupon knowledge.
        if (couponCode == "FREESHIP" && subtotal >= 1500)
            return 0;

        return _shippingClient.CalculateShipping(shippingAddress, totalItems, subtotal);
    }

    public string CreateShipment(string shippingAddress)
    {
        return _shippingClient.CreateShipment(shippingAddress);
    }
}