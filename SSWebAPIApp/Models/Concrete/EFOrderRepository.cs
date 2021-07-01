using Microsoft.EntityFrameworkCore;
using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSWebAPIApp.Models.Concrete
{
  public class EFOrderRepository : IOrderRepository
  {
    private readonly SportsStoreDbContext _context;
    public EFOrderRepository(SportsStoreDbContext sportsStoreDbContext) => _context = sportsStoreDbContext;
    public async Task<Order> GetOrderByIdAsync(long orderId) => await _context.Orders.FindAsync(orderId);

    public async Task<IEnumerable<Order>> GetOrdersAsync() => await _context.Orders.ToListAsync();

    public async Task<bool> RemoveOrderAsync(long orderId)
    {
      var order = await GetOrderByIdAsync(orderId);
      if (order != null)
      {
        _context.Orders.Remove(order);
        return await _context.SaveChangesAsync() > 0;
      }
      return false;
    }

    public async Task<Order> SaveOrderAsync(Order order)
    {
      if (order.OrderId == 0)
      {
        await _context.Orders.AddAsync(order);
      }
      else
      {
        _context.Entry<Order>(order).State = EntityState.Modified;
      }
      if (await _context.SaveChangesAsync() > 0)
      {
        return order;
      }
      return null;
    }
  }
}
