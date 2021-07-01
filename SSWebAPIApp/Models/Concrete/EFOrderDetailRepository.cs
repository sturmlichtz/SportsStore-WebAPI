using Microsoft.EntityFrameworkCore;
using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSWebAPIApp.Models.Concrete
{
  public class EFOrderDetailRepository: IOrderDetailRepository
  {
    private readonly SportsStoreDbContext _context;

    public EFOrderDetailRepository(SportsStoreDbContext sportsStoreDbContext) => _context = sportsStoreDbContext;

    public async Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail)
    {
      await _context.OrderDetails.AddAsync(orderDetail);
      if (await _context.SaveChangesAsync() > 0)
      {
        return orderDetail;
      }
      return null;
    }

    public async Task<IEnumerable<OrderDetail>> GetOrderDetailAsync(long orderId) => await _context.OrderDetails.Where(o => o.OrderId == orderId).ToListAsync();

    public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync() => await _context.OrderDetails.ToListAsync();

    public async Task<bool> RemoveOrderDetailAsync(long orderId)
    {
      var orderDetail = await GetOrderDetailAsync(orderId);
      _context.OrderDetails.RemoveRange(orderDetail);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail)
    {
      _context.Entry<OrderDetail>(orderDetail).State = EntityState.Modified;
      await _context.SaveChangesAsync();
      return orderDetail;
    }
  }
}
