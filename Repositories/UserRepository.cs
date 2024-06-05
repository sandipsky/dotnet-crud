using Dapper;
using DotnetCrud.Data;
using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public class UserRepository(DatabaseContext context) : IUserRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<IEnumerable<UserView>> GetAllAsync(UserFilter filter)
        {
            using var connection = _context.CreateConnection();

            var sql = @"SELECT * FROM Users";

            if (!string.IsNullOrEmpty(filter.Name))
            {
                sql += " AND Name LIKE @Name";
            }

            if (!string.IsNullOrEmpty(filter.Username))
            {
                sql += " AND Username LIKE @Username";
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                sql += " AND Email LIKE @Email";
            }

            sql += $" ORDER BY {filter.SortBy} {filter.SortOrder}";
            sql += " LIMIT @PageSize OFFSET @Offset";

            var parameters = new
            {
                Name = $"%{filter.Name}%",
                Username = $"%{filter.Username}%",
                Email = $"%{filter.Email}%",
                filter.PageSize,
                Offset = filter.PageIndex * filter.PageSize
            };

            return await connection.QueryAsync<UserView>(sql, parameters);
        }

        public async Task<int> CountAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(*) FROM Users";
            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<UserView> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var query = @"SELECT * FROM Users WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<UserView>(query, new { Id = id });
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT * FROM Users WHERE Username = @Username";
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
        }

        public async Task<int> AddAsync(User user)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Users (Name, Username, Email, PasswordHash) VALUES (@Name, @Username, @Email, @PasswordHash)", user);
        }

        public async Task<int> UpdateAsync(User user)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("UPDATE Users SET Name = @Name, Username = @Username, Email = @Email, PasswordHash = @PasswordHash WHERE Id = @Id", user);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
        }
    }
}