using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SSWebAPIApp.Models;
using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSWebAPIApp
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

      services.AddDbContext<SportsStoreDbContext>(cfg => {
        cfg.UseSqlServer(Configuration["ConnectionStrings:SportsStoreConnection"], sqlServerOptionsAction: sqlOptions => {
          sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(20), errorNumbersToAdd: null);
        });
        cfg.UseLoggerFactory(LoggerFactory.Create(cfg => { cfg.AddConsole(); })).EnableSensitiveDataLogging();
      });

      services.AddScoped<IProductRepository, EFProductRepository>();
      services.AddScoped<IOrderRepository, EFOrderRepository>();
      services.AddScoped<IOrderDetailRepository, EFOrderDetailRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseStatusCodePages();
      }

      using (var scope = app.ApplicationServices.CreateScope())
      {
        SportsStoreDbContext context = scope.ServiceProvider.GetRequiredService<SportsStoreDbContext>();
        var createDatabase = context.Database.EnsureCreated();
        if (createDatabase)
        {
          SportsStoreSeedData.PopulateSportsStore(context);
          logger.LogInformation($"***SportsStoreSeedData Called, '{context.Products.Count()}' - Products Added" +
            $"\n'{context.Orders.Count()}' - Orders Added" +
            $"\n'{context.OrderDetails.Count()}' - OrderDetails Added***");
        }
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapGet("/", async context =>
              {
            await context.Response.WriteAsync("Hello World!");
          });
      });
    }
  }
}
