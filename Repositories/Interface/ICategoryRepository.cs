using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task<int> AddAsync(Category entity);
        Task<int> UpdateAsync(Category entity);
        Task<int> DeleteAsync(int id);
    }
}