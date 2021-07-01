using System.Threading.Tasks;
using System.Collections.Generic;

using SSWebAPIApp.Models.Entities;

namespace SSWebAPIApp.Models.Abstract
{
  public interface IOrderDetailRepository
  {
    Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync();
    Task<IEnumerable<OrderDetail>> GetOrderDetailAsync(long orderId);
    Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail);
    Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail);
    Task<bool> RemoveOrderDetailAsync(long orderId);
  }
}
