using System.ComponentModel.DataAnnotations;

namespace RickGuitars.SmellyApi.Models;

public class CartItem
{
    [Key]
    public int Id { get; set; }

    public int CartId { get; set; }

    public Cart? Cart { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public int Quantity { get; set; }

    // Smell: duplicated price snapshot logic.
    // Later OrderItem will also store UnitPrice.
    public decimal UnitPrice { get; set; }
}