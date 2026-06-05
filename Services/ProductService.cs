using Microsoft.EntityFrameworkCore;
using RickGuitars.SmellyApi.Data;
using RickGuitars.SmellyApi.Models;
using RickGuitars.SmellyApi.Shared;

namespace RickGuitars.SmellyApi.Services;

public class ProductService
{
    private readonly RickGuitarsDbContext _db;

    public ProductService(RickGuitarsDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        if (ShopGlobalState.MaintenanceMode)
            return new List<Product>();

        return await _db.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Reviews)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _db.Products
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<List<Product>> SearchAsync(
        string? productType,
        string? builder,
        string? model,
        string? instrumentType,
        string? backWood,
        string? topWood,
        int? numStrings,
        string? style,
        string? accessoryCategory,
        decimal? minPrice,
        decimal? maxPrice,
        bool? featuredOnly)
    {
        var query = _db.Products
            .Where(p => !p.IsDeleted)
            .AsQueryable();

        // Smell: rigid search logic.
        // Every new searchable property requires changing this method.

        if (!string.IsNullOrWhiteSpace(productType))
            query = query.Where(p => p.ProductType.ToLower() == productType.ToLower());

        if (!string.IsNullOrWhiteSpace(builder))
            query = query.Where(p => p.Builder.ToLower() == builder.ToLower());

        if (!string.IsNullOrWhiteSpace(model))
            query = query.Where(p => p.Model.ToLower().Contains(model.ToLower()));

        if (!string.IsNullOrWhiteSpace(instrumentType))
            query = query.Where(p => p.InstrumentType.ToLower() == instrumentType.ToLower());

        if (!string.IsNullOrWhiteSpace(backWood))
            query = query.Where(p => p.BackWood.ToLower() == backWood.ToLower());

        if (!string.IsNullOrWhiteSpace(topWood))
            query = query.Where(p => p.TopWood.ToLower() == topWood.ToLower());

        if (numStrings.HasValue)
            query = query.Where(p => p.NumStrings == numStrings.Value);

        if (!string.IsNullOrWhiteSpace(style))
            query = query.Where(p => p.Style != null && p.Style.ToLower() == style.ToLower());

        if (!string.IsNullOrWhiteSpace(accessoryCategory))
            query = query.Where(p =>
                p.AccessoryCategory != null &&
                p.AccessoryCategory.ToLower() == accessoryCategory.ToLower());

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (featuredOnly == true)
            query = query.Where(p => p.IsFeatured);

        return await query.ToListAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product updatedProduct)
    {
        var product = await _db.Products.FindAsync(id);

        if (product == null || product.IsDeleted)
            return null;

        product.SerialNumber = updatedProduct.SerialNumber;
        product.ProductType = updatedProduct.ProductType;
        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Builder = updatedProduct.Builder;
        product.Model = updatedProduct.Model;
        product.InstrumentType = updatedProduct.InstrumentType;
        product.BackWood = updatedProduct.BackWood;
        product.TopWood = updatedProduct.TopWood;
        product.NumStrings = updatedProduct.NumStrings;
        product.Style = updatedProduct.Style;
        product.AccessoryCategory = updatedProduct.AccessoryCategory;
        product.Price = updatedProduct.Price;
        product.StockQuantity = updatedProduct.StockQuantity;
        product.IsFeatured = updatedProduct.IsFeatured;

        await _db.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product == null)
            return false;

        product.IsDeleted = true;

        await _db.SaveChangesAsync();

        return true;
    }
}