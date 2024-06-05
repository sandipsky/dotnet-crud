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
                    p.Id, p.Name, p.Price, p.Description, p.IsFeatured, p.Image, p.CategoryId, c.Name as CategoryName, p.BrandId, b.Name as BrandName
                FROM 
                    Products p
                INNER JOIN 
                    Categories c ON p.CategoryId = c.Id
                INNER JOIN 
                    Brands b ON p.BrandId = b.Id
                WHERE 
                    1=1";

            if (!string.IsNullOrEmpty(filter.Name))
            {
                sql += " AND p.Name LIKE @Name";
            }
            if (filter.MinPrice.HasValue)
            {
                sql += " AND p.Price >= @MinPrice";
            }
            if (filter.MaxPrice.HasValue)
            {
                sql += " AND p.Price <= @MaxPrice";
            }
            if (filter.CategoryId.HasValue)
            {
                sql += " AND p.CategoryId = @CategoryId";
            }
            if (filter.BrandId.HasValue)
            {
                sql += " AND p.BrandId = @BrandId";
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
                    p.Id, p.Name, p.Price, p.Description, p.IsFeatured, p.Image, p.CategoryId, c.Name as CategoryName, p.BrandId, b.Name as BrandName
                FROM 
                    Products p
                INNER JOIN 
                    Categories c ON p.CategoryId = c.Id
                INNER JOIN 
                    Brands b ON p.BrandId = b.Id
                WHERE 
                    1=1";
            return await connection.QuerySingleOrDefaultAsync<ProductViewDTO>(query, new { Id = id });
        }

        public async Task<int> AddAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Products (Name, Price, CategoryId, BrandId, Description, Image, IsFeatured) VALUES (@Name, @Price, @CategoryId, @BrandId, @Description, @Image, @IsFeatured)", product);
        }

        public async Task<int> UpdateAsync(Product product)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("UPDATE Products SET Name = @Name, Price = @Price, CategoryId = @CategoryId, BrandId = @BrandId, Description = @Description, Image = @Image, IsFeatured = @IsFeatured WHERE Id = @Id", product);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Products WHERE Id = @Id", new { Id = id });
        }
    }
}