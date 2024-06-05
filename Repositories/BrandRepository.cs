using Dapper;
using DotnetCrud.Data;
using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public class BrandRepository(DatabaseContext context) : IBrandRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Brand>("SELECT * FROM Brands");
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Brand>("SELECT * FROM Brands WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Brand brand)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Brands (Name) VALUES (@Name)", brand);
        }

        public async Task<int> UpdateAsync(Brand brand)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("UPDATE Brands SET Name = @Name WHERE Id = @Id", brand);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Brands WHERE Id = @Id", new { Id = id });
        }
    }
}