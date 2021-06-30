using SSWebAPIApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSWebAPIApp.Models
{
  public static class SportsStoreSeedData
  {
    public static void PopulateSportsStore(SportsStoreDbContext context)
    {
      if (!context.Products.Any())
      {
        context.Products.AddRange(
          new Product { ProductName = "Kayak", Description = "A boat for one person", Price = 275.00m, Category = "Watersports" },
          new Product { ProductName = "Unsteady Chair", Description = "Secretly give your opponent a disadvantage", Price = 29.95m, Category = "Chess" },
          new Product { ProductName = "Lifejacket", Description = "Protective and fashionable", Price = 48.95m, Category = "Watersports" },
          new Product { ProductName = "Soccer ball", Description = "FIFA-approved size and weight", Price = 19.50m, Category = "Soccer" },
          new Product { ProductName = "Spalding Ball", Description = "NBA official Basketball", Price = 160.00m, Category = "Basketball" },
          new Product { ProductName = "Corner flags", Description = "Give your playing field that professional touch", Price = 34.95m, Category = "Soccer" },
          new Product { ProductName = "Thinking cap", Description = "Improve your brain efficiency by 75%", Price = 16.00m, Category = "Chess" },
          new Product { ProductName = "Ring Net", Description = "NBA size ring nets", Price = 60.00m, Category = "Basketball" },
          new Product { ProductName = "Shoe", Description = "Studded shoes", Price = 950.00m, Category = "Soccer" });
      }
      context.SaveChanges();
      if (!context.Orders.Any())
      {
        List<Order> orderList = new List<Order>
        {
          new Order { Name = "Bruce Wayne", City = "Mumbai", Country = "India", Giftwrap = "false", State = "Maharashtra", Zip = "400019", Shipped = "false"},
          new Order { Name = "Peter Parker", City = "Pune", Country = "India", Giftwrap = "false", State = "Maharashtra", Zip = "500019", Shipped = "false"}
        };
        context.Orders.AddRange(orderList);

        if (context.SaveChanges() > 0)
        {
          if (!context.OrderDetails.Any())
          {
            var addOrderedProducts = context.Products.Where(p => p.Category == "Watersports" || p.Category == "Chess").OrderBy(p => p.Category).ToList();
            foreach (var order in orderList)
            {
              var orderID = order.OrderId;
              var orderDetails = new List<OrderDetail>();
              int ctr = 1;
              foreach (var item in addOrderedProducts)
              {
                if (orderID == 1 && item.Category == "Watersports")
                {
                  orderDetails.Add(new OrderDetail { OrderId = orderID, ProductId = item.ProductId, ProductName = item.ProductName, Price = item.Price, Count = ctr++ });
                }
                else if (orderID == 2 && item.Category == "Chess")
                {
                  orderDetails.Add(new OrderDetail { OrderId = orderID, ProductId = item.ProductId, ProductName = item.ProductName, Price = item.Price, Count = ctr++ });
                }
                if (ctr == 3) ctr = 1;
              }
              context.OrderDetails.AddRange(orderDetails);
            }
            context.SaveChanges();
          }
        }
      }
      context.SaveChanges();
    }
  }
}
