using System.ComponentModel.DataAnnotations;

namespace RickGuitars.SmellyApi.Models;

public class Review
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}