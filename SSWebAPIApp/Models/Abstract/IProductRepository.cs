using System.Threading.Tasks;
using System.Collections.Generic;

using SSWebAPIApp.Models.Entities;

namespace SSWebAPIApp.Models.Abstract
{
  public interface IProductRepository
  {
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(long productId);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    Task<Product> GetProductByIdAsync(long productId);
  }
}
