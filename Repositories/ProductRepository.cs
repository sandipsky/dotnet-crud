using Dapper;
using DotnetCrud.Data;
using DotnetCrud.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetCrud.Repositories
{
    public class ProductRepository(DatabaseContext context) : IRepository<Product>
    {
        private readonly DatabaseContext _context = context;

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>("SELECT * FROM Products");
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Products (Name, Price, CategoryId) VALUES (@Name, @Price, @CategoryId)", product);
        }

        public async Task<int> UpdateAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("UPDATE Products SET Name = @Name, Price = @Price, CategoryId = @CategoryId WHERE Id = @Id", product);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
        }
    }
}