using DotnetCrud.Models;
using DotnetCrud.Repositories;

namespace DotnetCrud.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

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