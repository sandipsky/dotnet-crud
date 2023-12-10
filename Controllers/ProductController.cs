using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crud.Data;
using crud.Models;
using Microsoft.AspNetCore.Authorization;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductContext _context;

    public ProductController(ProductContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> getProducts()
    {
        var Products = await _context.Products.ToListAsync();
        return Ok(Products);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<object>> FilterProduct(
    [FromQuery] string sortBy,
    [FromQuery] string sortOrder,
    [FromQuery] string filterByName,
    [FromQuery] decimal? minPrice,
    [FromQuery] decimal? maxPrice,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        IQueryable<Product> productsQuery = _context.Products;


        if (!string.IsNullOrWhiteSpace(filterByName))
        {
            productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(filterByName.ToLower()));
        }


        if (minPrice.HasValue || maxPrice.HasValue)
        {
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value && p.Price <= maxPrice.Value);
            }
            else if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }
            else if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }
        }


        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "name":
                    productsQuery = (sortOrder.ToLower() == "desc")
                        ? productsQuery.OrderByDescending(p => p.Name)
                        : productsQuery.OrderBy(p => p.Name);
                    break;
                case "price":
                    productsQuery = (sortOrder.ToLower() == "desc")
                        ? productsQuery.OrderByDescending(p => p.Price)
                        : productsQuery.OrderBy(p => p.Price);
                    break;

            }
        }

        // Apply pagination
        int totalItems = await productsQuery.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var products = await productsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var result = new
        {
            Items = products,
            TotalItems = totalItems,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        // Add any specific logic for viewing products if needed
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(long id)
    {
        var Product = await _context.Products.FindAsync(id);
        if (Product == null)
        {
            return NotFound();
        }
        return Ok(Product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProductItem(Product Product)
    {
        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProductById), new { id = Product.Id }, new { id = Product.Id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(long id, Product ProductItem)
    {
        var existingProduct = _context.Products.Find(id);

        if (existingProduct == null)
        {
            return NotFound();
        }

        // Update the existingProduct with the values from ProductItem
        // _context.Entry(ProductItem).State = EntityState.Modified;
        existingProduct.Name = ProductItem.Name;
        existingProduct.Price = ProductItem.Price;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency conflicts if needed
            return Conflict(); // HTTP 409 Conflict
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        var ProductItem = await _context.Products.FindAsync(id);
        if (ProductItem == null)
        {
            return NotFound();
        }

        _context.Products.Remove(ProductItem);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpGet("secret")]
    public IActionResult Secret()
    {
        var response = new { message = "You are awesome" };
        return Ok(response);
    }

}