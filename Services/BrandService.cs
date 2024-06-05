using DotnetCrud.Models;
using DotnetCrud.Repositories;

namespace DotnetCrud.Services
{
    public class BrandService(IBrandRepository repository) : IBrandService
    {
        private readonly IBrandRepository _repository = repository;

        public async Task<IEnumerable<Brand>> GetCategoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateBrandAsync(Brand brand)
        {
            return await _repository.AddAsync(brand);
        }

        public async Task<int> UpdateBrandAsync(Brand brand)
        {
            return await _repository.UpdateAsync(brand);
        }

        public async Task<int> DeleteBrandAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}