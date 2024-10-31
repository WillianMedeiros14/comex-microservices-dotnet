using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> opts) : base(opts)
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