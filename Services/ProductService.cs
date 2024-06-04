using DotnetCrud.Models;
using DotnetCrud.Repositories;

namespace DotnetCrud.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
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