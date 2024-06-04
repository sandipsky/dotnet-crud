using DotnetCrud.DTOs;
using DotnetCrud.Models;
using DotnetCrud.Repositories;

namespace DotnetCrud.Services
{
    public class UserService(IUserRepository repository) : IUserService
    {
        private readonly IUserRepository _repository = repository;

        public async Task<PagedResponse<User>> GetUsersAsync(UserFilter filter)
        {
            var items = await _repository.GetAllAsync(filter);
            var count = await _repository.CountAllAsync();
            return new PagedResponse<User>
            {
                Items = items.ToList(),
                TotalElements = count
            };
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateUserAsync(User user)
        {
            return await _repository.AddAsync(user);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            return await _repository.UpdateAsync(user);
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}