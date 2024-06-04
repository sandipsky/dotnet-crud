using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Services
{
    public interface IUserService
    {
        Task<PagedResponse<User>> GetUsersAsync(UserFilter filter);
        Task<User> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task<int> DeleteUserAsync(int id);
    }
}