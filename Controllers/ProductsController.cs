using Microsoft.AspNetCore.Mvc;
using RickGuitars.SmellyApi.Models;
using RickGuitars.SmellyApi.Services;

namespace RickGuitars.SmellyApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Product>>> Search(
        [FromQuery] string? productType,
        [FromQuery] string? builder,
        [FromQuery] string? model,
        [FromQuery] string? instrumentType,
        [FromQuery] string? backWood,
        [FromQuery] string? topWood,
        [FromQuery] int? numStrings,
        [FromQuery] string? style,
        [FromQuery] string? accessoryCategory,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] bool? featuredOnly)
    {
        var products = await _productService.SearchAsync(
            productType,
            builder,
            model,
            instrumentType,
            backWood,
            topWood,
            numStrings,
            style,
            accessoryCategory,
            minPrice,
            maxPrice,
            featuredOnly);

        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        var createdProduct = await _productService.CreateAsync(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            createdProduct);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> Update(int id, Product product)
    {
        var updatedProduct = await _productService.UpdateAsync(id, product);

        if (updatedProduct == null)
            return NotFound();

        return Ok(updatedProduct);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}