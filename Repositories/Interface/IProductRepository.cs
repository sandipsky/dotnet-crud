using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductViewDTO>> GetAllAsync();
        Task<ProductViewDTO> GetByIdAsync(int id);
        Task<int> AddAsync(Product entity);
        Task<int> UpdateAsync(Product entity);
        Task<int> DeleteAsync(int id);
    }
}