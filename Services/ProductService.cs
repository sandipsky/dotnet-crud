using DotnetCrud.DTOs;
using DotnetCrud.Models;
using DotnetCrud.Repositories;

namespace DotnetCrud.Services
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        private readonly IProductRepository _repository = repository;

        public async Task<IEnumerable<ProductViewDTO>> GetProductsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProductViewDTO> GetProductByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateProductAsync(Product product)
        {
            return await _repository.AddAsync(product);
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            return await _repository.UpdateAsync(product);
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}