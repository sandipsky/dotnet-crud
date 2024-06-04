using DotnetCrud.DTOs;
using DotnetCrud.Models;
using DotnetCrud.Repositories;

namespace DotnetCrud.Services
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        private readonly IProductRepository _repository = repository;

        public async Task<PagedResponse<ProductViewDTO>> GetProductsAsync(ProductFilter filter)
        {
            var items = await _repository.GetAllAsync(filter);
            var count = await _repository.CountAllAsync();
            return new PagedResponse<ProductViewDTO>
            {
                Items = items.ToList(),
                TotalElements = count
            };
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