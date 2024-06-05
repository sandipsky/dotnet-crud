using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Services
{
    public interface IProductService
    {
        Task<PagedResponse<ProductViewDTO>> GetProductsAsync(ProductFilter filter);
        Task<ProductViewDTO> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(Product product);
        Task<int> UpdateProductAsync(Product product);
        Task<int> DeleteProductAsync(int id);
        Task<string> SaveImageAsync(IFormFile image);
    }
}