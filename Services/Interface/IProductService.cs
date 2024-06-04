using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewDTO>> GetProductsAsync();
        Task<ProductViewDTO> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(Product product);
        Task<int> UpdateProductAsync(Product product);
        Task<int> DeleteProductAsync(int id);
    }
}