namespace RickGuitars.SmellyApi.External;

public class FakeStripeClient
{
    public FakePaymentResult Charge(string cardNumber, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            return new FakePaymentResult
            {
                Success = false,
                TransactionId = string.Empty,
                ErrorMessage = "Card number is required."
            };
        }

        if (amount <= 0)
        {
            return new FakePaymentResult
            {
                Success = false,
                TransactionId = string.Empty,
                ErrorMessage = "Invalid payment amount."
            };
        }

        if (cardNumber.EndsWith("0000"))
        {
            return new FakePaymentResult
            {
                Success = false,
                TransactionId = string.Empty,
                ErrorMessage = "Payment was declined by the bank."
            };
        }

        return new FakePaymentResult
        {
            Success = true,
            TransactionId = $"stripe_{Guid.NewGuid():N}",
            ErrorMessage = string.Empty
        };
    }
}

public class FakePaymentResult
{
    public bool Success { get; set; }

    public string TransactionId { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;
}