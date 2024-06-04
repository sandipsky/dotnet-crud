using DotnetCrud.Models;
using DotnetCrud.Repositories;

namespace DotnetCrud.Services
{
    public class CategoryService(ICategoryRepository repository) : ICategoryService
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateCategoryAsync(Category category)
        {
            return await _repository.AddAsync(category);
        }

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            return await _repository.UpdateAsync(category);
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}