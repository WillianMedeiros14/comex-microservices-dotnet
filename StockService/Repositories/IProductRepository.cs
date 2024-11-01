
using StockService.Models;

namespace StockService.Repository
{
    public interface IProductRepository
    {
        Task SaveChangesAsync();
        Task<Product> GetProductById(int id);
    }
}
