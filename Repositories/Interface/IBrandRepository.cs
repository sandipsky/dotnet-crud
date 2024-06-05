using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand> GetByIdAsync(int id);
        Task<int> AddAsync(Brand entity);
        Task<int> UpdateAsync(Brand entity);
        Task<int> DeleteAsync(int id);
    }
}