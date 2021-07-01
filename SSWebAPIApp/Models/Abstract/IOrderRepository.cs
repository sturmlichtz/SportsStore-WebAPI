using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Entities;

namespace SSWebAPIApp.Models.Abstract
{
  public interface IOrderRepository
  {
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(long orderId);
    Task<bool> RemoveOrderAsync(long orderId);
    Task<Order> SaveOrderAsync(Order order);
  }
}
