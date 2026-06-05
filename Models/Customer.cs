using System.ComponentModel.DataAnnotations;

namespace RickGuitars.SmellyApi.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string DefaultShippingAddress { get; set; } = string.Empty;

    // Smell: customer category is represented using booleans and strings.
    public bool IsVip { get; set; }

    public string CustomerLevel { get; set; } = "Regular";

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public Cart? Cart { get; set; }

    public List<Order> Orders { get; set; } = new();
}