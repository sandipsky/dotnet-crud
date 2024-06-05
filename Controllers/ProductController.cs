using Microsoft.AspNetCore.Mvc;
using DotnetCrud.Models;
using DotnetCrud.Services;
using DotnetCrud.DTOs;

namespace DotnetCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] ProductFilter filter)
        {
            var products = await _productService.GetProductsAsync(filter);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromForm] ProductForm productDto)
        {
            var imageUrl = await _productService.SaveImageAsync(productDto.Image);
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                IsFeatured = productDto.IsFeatured,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId,
                Image = imageUrl
            };

            await _productService.CreateProductAsync(product);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] ProductForm productDto)
        {

            var imageUrl = await _productService.SaveImageAsync(productDto.Image);
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                IsFeatured = productDto.IsFeatured,
                CategoryId = productDto.CategoryId,
                BrandId = productDto.BrandId,
                Image = imageUrl
            };

            await _productService.UpdateProductAsync(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}