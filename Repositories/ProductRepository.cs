using Dapper;
using DotnetCrud.Data;
using DotnetCrud.DTOs;
using DotnetCrud.Models;

namespace DotnetCrud.Repositories
{
    public class ProductRepository(DatabaseContext context) : IProductRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<IEnumerable<ProductViewDTO>> GetAllAsync(ProductFilter filter)
        {
            using var connection = _context.CreateConnection();

            var sql = @"
                SELECT 
                    p.Id, p.Name, p.Price, p.CategoryId, c.Name as CategoryName
                FROM 
                    Products p
                INNER JOIN 
                    Categories c ON p.CategoryId = c.Id
                WHERE 
                    1=1";

            var countSql = "SELECT COUNT(*) FROM Products p WHERE 1=1";

            if (!string.IsNullOrEmpty(filter.Name))
            {
                sql += " AND p.Name LIKE @Name";
                countSql += " AND p.Name LIKE @Name";
            }
            if (filter.MinPrice.HasValue)
            {
                sql += " AND p.Price >= @MinPrice";
                countSql += " AND p.Price >= @MinPrice";
            }
            if (filter.MaxPrice.HasValue)
            {
                sql += " AND p.Price <= @MaxPrice";
                countSql += " AND p.Price <= @MaxPrice";
            }
            if (filter.CategoryId.HasValue)
            {
                sql += " AND p.CategoryId = @CategoryId";
                countSql += " AND p.CategoryId = @CategoryId";
            }

            sql += $" ORDER BY p.{filter.SortBy} {filter.SortOrder}";
            sql += " LIMIT @PageSize OFFSET @Offset";

            var parameters = new
            {
                Name = $"%{filter.Name}%",
                filter.MinPrice,
                filter.MaxPrice,
                filter.CategoryId,
                filter.PageSize,
                Offset = filter.PageIndex * filter.PageSize
            };

            return await connection.QueryAsync<ProductViewDTO>(sql, parameters);           
        }

        public async Task<int> CountAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT COUNT(*) FROM Products";
            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<ProductViewDTO> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var query = @"
                SELECT 
                    p.Id, p.Name, p.Price, p.CategoryId, c.Name as CategoryName
                FROM 
                    Products p
                INNER JOIN 
                    Categories c ON p.CategoryId = c.Id
                WHERE 
                    p.Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<ProductViewDTO>(query, new { Id = id });
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