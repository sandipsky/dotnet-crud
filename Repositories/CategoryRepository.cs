using Dapper;
using DotnetCrud.Data;
using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public class CategoryRepository(DatabaseContext context) : IRepository<Category>
    {
        private readonly DatabaseContext _context = context;

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Category>("SELECT * FROM Categories");
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Category>("SELECT * FROM Categories WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Category category)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Categories (Name) VALUES (@Name)", category);
        }

        public async Task<int> UpdateAsync(Category category)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("UPDATE Categories SET Name = @Name WHERE Id = @Id", category);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Categories WHERE Id = @Id", new { Id = id });
        }
    }
}