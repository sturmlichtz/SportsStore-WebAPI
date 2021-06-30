
using Microsoft.EntityFrameworkCore;
using SSWebAPIApp.Models.Entities;

namespace SSWebAPIApp.Models
{
  public class SportsStoreDbContext : DbContext
  {
    public SportsStoreDbContext(DbContextOptions<SportsStoreDbContext> dbContextOptions) : base(dbContextOptions) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      #region Default
      //base.OnModelCreating(modelBuilder); 
      #endregion

      // Create Composite primary key in OrderDetail
      modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderId, od.ProductId });
    }
  }
}
