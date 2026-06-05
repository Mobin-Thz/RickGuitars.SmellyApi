using Microsoft.AspNetCore.Mvc;
using RickGuitars.SmellyApi.Data;
using System.Text;

namespace RickGuitars.SmellyApi.Controllers;

[ApiController]
[Route("api/deepsource-demo")]
public class DeepSourceIssuesController : ControllerBase
{
    private readonly RickGuitarsDbContext _db;

    // Problem 1: hardcoded secret
    private const string StripeSecretKey = "sk_live_123456789_fake_secret_key";

    // Problem 2: unused field
    private readonly string _unusedConnectionString = "Server=localhost;User Id=sa;Password=admin123;";

    public DeepSourceIssuesController(RickGuitarsDbContext db)
    {
        _db = db;
    }

    [HttpGet("product/{id}")]
    public IActionResult GetProduct(int id)
    {
        // Problem 3: possible null reference
        var product = _db.Products.FirstOrDefault(p => p.Id == id);

        // product can be null here
        return Ok(new
        {
            product.Id,
            product.Name,
            product.Price
        });
    }

    [HttpGet("search")]
    public IActionResult Search(string name)
    {
        // Problem 4: SQL injection style string building
        var query = "SELECT * FROM Products WHERE Name = '" + name + "'";

        // Problem 5: unused variable
        var debugMode = true;

        return Ok(new
        {
            Message = "This query is unsafe.",
            Query = query
        });
    }

    [HttpPost("checkout")]
    public IActionResult Checkout(int productId, int quantity, string customerEmail, string couponCode)
    {
        // Problem 6: long method + too many responsibilities
        // It validates input, reads database, calculates discount,
        // creates logs, handles payment, and creates response.

        if (quantity <= 0)
        {
            return BadRequest("Invalid quantity");
        }

        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            return BadRequest("Email is required");
        }

        if (!customerEmail.Contains("@"))
        {
            return BadRequest("Invalid email");
        }

        var product = _db.Products.FirstOrDefault(p => p.Id == productId);

        // Problem 7: possible null reference again
        var total = product.Price * quantity;

        if (couponCode == "SUMMER10")
        {
            total = total * 0.90m;
        }
        else if (couponCode == "VIP20")
        {
            total = total * 0.80m;
        }
        else if (couponCode == "ADMIN100")
        {
            total = 0;
        }
        else if (couponCode == "TEST")
        {
            total = 1;
        }
        else if (couponCode == "BLACKFRIDAY")
        {
            total = total * 0.50m;
        }

        // Problem 8: inefficient string concatenation in loop
        var receipt = "";
        for (var i = 0; i < quantity; i++)
        {
            receipt += product.Name + ", ";
        }

        // Problem 9: empty catch block
        try
        {
            FakePayment(customerEmail, total);
        }
        catch (Exception)
        {
        }

        return Ok(new
        {
            Product = product.Name,
            Quantity = quantity,
            Total = total,
            Receipt = receipt,
            SecretUsed = StripeSecretKey
        });
    }

    [HttpGet("report")]
    public IActionResult GenerateReport()
    {
        var products = _db.Products.ToList();

        // Problem 10: inefficient string building
        var report = "";
        foreach (var product in products)
        {
            report += product.Id + " - " + product.Name + " - " + product.Price + "\n";
        }

        return Ok(report);
    }

    [HttpGet("complexity")]
    public IActionResult ComplexityDemo(int score)
    {
        // Problem 11: high cyclomatic complexity
        if (score < 0)
        {
            return BadRequest("Invalid");
        }
        else if (score == 0)
        {
            return Ok("Zero");
        }
        else if (score > 0 && score < 10)
        {
            return Ok("Very low");
        }
        else if (score >= 10 && score < 20)
        {
            return Ok("Low");
        }
        else if (score >= 20 && score < 30)
        {
            return Ok("Medium low");
        }
        else if (score >= 30 && score < 40)
        {
            return Ok("Medium");
        }
        else if (score >= 40 && score < 50)
        {
            return Ok("Medium high");
        }
        else if (score >= 50 && score < 60)
        {
            return Ok("High");
        }
        else if (score >= 60 && score < 70)
        {
            return Ok("Very high");
        }
        else
        {
            return Ok("Extreme");
        }
    }

    // Problem 12: async void should usually be avoided
    private async void WriteAuditLog(string message)
    {
        await Task.Delay(100);
        Console.WriteLine(message);
    }

    private void FakePayment(string email, decimal amount)
    {
        if (email == "fail@test.com")
        {
            throw new Exception("Payment failed");
        }

        Console.WriteLine("Charged " + amount);
    }
}