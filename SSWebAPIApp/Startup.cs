using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SSWebAPIApp.Models;
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
      services.AddDbContext<SportsStoreDbContext>(cfg => {
        cfg.UseSqlServer(Configuration["ConnectionStrings:SportsStoreConnection"], sqlServerOptionsAction: sqlOptions => {
          sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(20), errorNumbersToAdd: null);
        });
        cfg.UseLoggerFactory(LoggerFactory.Create(cfg => { cfg.AddConsole(); })).EnableSensitiveDataLogging();
      });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGet("/", async context =>
              {
            await context.Response.WriteAsync("Hello World!");
          });
      });
    }
  }
}
