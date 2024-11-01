
using Microsoft.EntityFrameworkCore;
using StockService.Data;
using StockService.Models;


namespace StockService.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;


        public ProductRepository(ProductContext context)
        {
            _context = context;

        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}