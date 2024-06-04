using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetCrud.DTOs;
using DotnetCrud.Models;
using DotnetCrud.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace DotnetCrud.Services
{
    public class UserService(IUserRepository repository, IConfiguration configuration) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly IConfiguration _configuration = configuration;

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

        public async Task<User> LoginUserAsync(string username, string password)
        {
            var user = await _repository.GetUserByUsernameAsync(username);
            // if (user == null || !VerifyPassword(user.PasswordHash, password))
            // {
            //     return null;
            // }
            if (user == null || user.PasswordHash != password)
            {
                return null;
            }
            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPassword(string storedPasswordHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
        }
    }
}