namespace RickGuitars.SmellyApi.Models;

public class PaymentInfo
{
    public string CardHolderName { get; set; } = string.Empty;

    public string CardNumber { get; set; } = string.Empty;

    public string ExpirationMonth { get; set; } = string.Empty;

    public string ExpirationYear { get; set; } = string.Empty;

    public string Cvv { get; set; } = string.Empty;

    // Smell: string instead of a payment method abstraction.
    public string PaymentProvider { get; set; } = "Stripe";
}