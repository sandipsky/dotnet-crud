using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Services
{
    public interface IUserService
    {
        Task<PagedResponse<UserView>> GetUsersAsync(UserFilter filter);
        Task<UserView> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task<int> DeleteUserAsync(int id);
        Task<User> LoginUserAsync(string username, string password);
        Task<bool> UserExistsAsync(string username);
        string GenerateJwtToken(User user);
        bool IsValidPassword(string password);
    }
}