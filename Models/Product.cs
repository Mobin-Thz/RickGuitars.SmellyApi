using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace RickGuitars.SmellyApi.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    public string SerialNumber { get; set; } = string.Empty;

    // Smell: primitive obsession.
    // ProductType can be "Guitar", "Mandolin", "Bass", "Accessory", etc.
    public string ProductType { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Builder { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    // Smell: string instead of enum or value object.
    // Examples: "Acoustic", "Electric"
    public string InstrumentType { get; set; } = string.Empty;

    public string BackWood { get; set; } = string.Empty;

    public string TopWood { get; set; } = string.Empty;

    // Smell: nullable fields for different product types.
    // Guitars use NumStrings, mandolins may use Style, accessories use AccessoryCategory.
    public int? NumStrings { get; set; }

    public string? Style { get; set; }

    public string? AccessoryCategory { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Review> Reviews { get; set; } = new();
}