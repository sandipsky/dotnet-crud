using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetCrud.DTOs;
using DotnetCrud.Models;
using DotnetCrud.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace DotnetCrud.Services
{
    public class UserService(IUserRepository repository, IConfiguration configuration) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly IConfiguration _configuration = configuration;

        public async Task<PagedResponse<UserView>> GetUsersAsync(UserFilter filter)
        {
            var items = await _repository.GetAllAsync(filter);
            var count = await _repository.CountAllAsync();
            return new PagedResponse<UserView>
            {
                Items = items.ToList(),
                TotalElements = count
            };
        }

        public async Task<UserView> GetUserByIdAsync(int id)
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

        public async Task<User> LoginUserAsync(string username, string password)
        {
            var user = await _repository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }
            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                    // Add more claims if needed
                }),
                Expires = DateTime.UtcNow.AddDays(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPassword(string storedPasswordHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            var user = await _repository.GetUserByUsernameAsync(username);
            return user != null;
        }

        public bool IsValidPassword(string password)
        {
            if (password.Length < 8) return false;
            if (!Regex.IsMatch(password, @"[A-Z]")) return false;
            if (!Regex.IsMatch(password, @"[0-9]")) return false;
            if (!Regex.IsMatch(password, @"[\W]")) return false;
            return true;
        }
    }
}
