using Microsoft.EntityFrameworkCore;
using StockService.Models;

namespace StockService.Data;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> opts) : base(opts)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Category>()
            .HasMany(categoria => categoria.Products)
            .WithOne(produto => produto.Category)
            .HasForeignKey(produto => produto.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}