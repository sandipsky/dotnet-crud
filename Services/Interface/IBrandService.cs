using DotnetCrud.Models;

namespace DotnetCrud.Services
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetCategoriesAsync();
        Task<Brand> GetBrandByIdAsync(int id);
        Task<int> CreateBrandAsync(Brand brand);
        Task<int> UpdateBrandAsync(Brand brand);
        Task<int> DeleteBrandAsync(int id);
    }
}