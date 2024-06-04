using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(UserFilter filter);
        Task<User> GetByIdAsync(int id);
        Task<int> AddAsync(User entity);
        Task<int> UpdateAsync(User entity);
        Task<int> DeleteAsync(int id);
        Task<int> CountAllAsync();
    }
}