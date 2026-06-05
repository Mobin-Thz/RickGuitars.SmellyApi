namespace RickGuitars.SmellyApi.External;

public class FakeShippingClient
{
    public decimal CalculateShipping(string shippingAddress, int totalItems, decimal subtotal)
    {
        if (string.IsNullOrWhiteSpace(shippingAddress))
            return 0;

        if (subtotal > 3000)
            return 0;

        if (totalItems >= 3)
            return 49.99m;

        return 24.99m;
    }

    public string CreateShipment(string shippingAddress)
    {
        if (string.IsNullOrWhiteSpace(shippingAddress))
            throw new Exception("Shipping address is required.");

        return $"TRK-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(10000, 99999)}";
    }
}