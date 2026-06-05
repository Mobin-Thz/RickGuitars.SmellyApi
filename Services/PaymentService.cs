using RickGuitars.SmellyApi.External;
using RickGuitars.SmellyApi.Models;

namespace RickGuitars.SmellyApi.Services;

public class PaymentService
{
    private readonly FakeStripeClient _stripeClient;

    public PaymentService(FakeStripeClient stripeClient)
    {
        _stripeClient = stripeClient;
    }

    public string Charge(PaymentInfo paymentInfo, decimal amount)
    {
        // Smell: PaymentService knows Stripe directly.
        // There is no IPaymentGateway abstraction.

        if (paymentInfo.PaymentProvider != "Stripe")
        {
            throw new Exception("Only Stripe payments are supported right now.");
        }

        var result = _stripeClient.Charge(paymentInfo.CardNumber, amount);

        if (!result.Success)
            throw new Exception(result.ErrorMessage);

        return result.TransactionId;
    }
}