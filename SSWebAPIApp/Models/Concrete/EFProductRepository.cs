using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Entities;

namespace SSWebAPIApp.Models.Concrete
{
  public class EFProductRepository : IProductRepository
  {
    private readonly SportsStoreDbContext _context;
    private readonly ILogger<EFProductRepository> _logger;

    public EFProductRepository(SportsStoreDbContext context, ILogger<EFProductRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<Product> AddProductAsync(Product product)
    {
      await _context.Products.AddAsync(product);
      int recEffect = await _context.SaveChangesAsync();
      if (recEffect == 1)
      {
        _logger.LogInformation($"***EFProductRepository.AddProductAsync, New Product - Id: {product.ProductId}, ProductName: {product.ProductName}, Added Successfully***");
        return product;
      }
      return null;
    }

    public async Task<bool> DeleteProductAsync(long productId)
    {
      var product = await GetProductByIdAsync(productId);
      _context.Products.Remove(product);
      int recEffect = await _context.SaveChangesAsync();
      if (recEffect == 1)
      {
        _logger.LogInformation($"***EFProductRepository.DeleteProductAsync, Product with - Id: {product.ProductId}, ProductName: {product.ProductName}, Deleted Successfully***");
        return true;
      }
      return false;
    }

    public async Task<Product> GetProductByIdAsync(long productId) => await _context.Products.FindAsync(productId);

    public async Task<IEnumerable<Product>> GetProductsAsync() => await _context.Products.ToListAsync();

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category) => await
      _context.Products.Where(p=>p.Category == category).ToListAsync();

    public async Task<Product> UpdateProductAsync(Product product)
    {
      _context.Entry<Product>(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
      int recEffect = await _context.SaveChangesAsync();
      if (recEffect == 1)
      {
        _logger.LogInformation($"***EFProductRepository.UpdateProductAsync, Product - Id: {product.ProductId}, Updated Successfully***");
      }
      return product;
    }
  }
}
