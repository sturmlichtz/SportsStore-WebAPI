using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SSWebAPIApp.Models;
using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Concrete;
using SSWebAPIApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSWebAPIApp
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

      services.AddAuthentication()
        .AddJwtBearer(cfg => {
          cfg.TokenValidationParameters = new TokenValidationParameters
          {
            ValidIssuer = Configuration["Tokens:Issuer"],
            ValidAudience = Configuration["Tokens:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
          };
        }); // AddJwtBearer


      services.AddMvc();

      services.AddDbContext<SportsStoreDbContext>(cfg =>
      {
        cfg.UseSqlServer(Configuration["ConnectionStrings:SportsStoreConnection"], sqlServerOptionsAction: sqlOptions =>
        {
          sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(20), errorNumbersToAdd: null);
        });
        //cfg.UseLoggerFactory(LoggerFactory.Create(cfg => { cfg.AddConsole(); })).EnableSensitiveDataLogging();
      });

      #region Without SSUser
      services.AddDbContext<IdentityDbContext>(cfg =>
      {
        cfg.UseSqlServer(Configuration["ConnectionStrings:SportsStoreIdentityConnection"], sqlServerOptionsAction: sqlOptions =>
        {
          sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
          sqlOptions.MigrationsAssembly("SSWebAPIApp");
        });
        cfg.UseLoggerFactory(LoggerFactory.Create(cfg => { cfg.AddConsole(); })).EnableSensitiveDataLogging();
      });

      services.AddIdentity<IdentityUser, IdentityRole>(cfg =>
      {
        cfg.Password.RequiredLength = 8;
        cfg.Password.RequireLowercase = false;
        cfg.SignIn.RequireConfirmedEmail = false;
        cfg.User.RequireUniqueEmail = true;
      })
        .AddEntityFrameworkStores<IdentityDbContext>();
      #endregion

      #region With SSUser
      //services.AddDbContext<IdentityDbContext<SSUser>>(cfg =>
      //{
      //  cfg.UseSqlServer(Configuration["ConnectionStrings:SportsStoreIdentityConnection"], sqlServerOptionsAction: sqlOptions =>
      //  {
      //    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
      //    //sqlOptions.MigrationsAssembly("SSWebAPIApp");
      //  });
      //  cfg.UseLoggerFactory(LoggerFactory.Create(cfg => { cfg.AddConsole(); })).EnableSensitiveDataLogging();
      //});

      //services.AddIdentity<SSUser, IdentityRole>(cfg =>
      //{
      //  cfg.Password.RequiredLength = 8;
      //  cfg.Password.RequireLowercase = false;
      //  cfg.SignIn.RequireConfirmedEmail = false;
      //  cfg.User.RequireUniqueEmail = true;
      //})
      //  .AddEntityFrameworkStores<IdentityDbContext<SSUser>>();
      #endregion

      services.AddScoped<IProductRepository, EFProductRepository>();
      services.AddScoped<IOrderRepository, EFOrderRepository>();
      services.AddScoped<IOrderDetailRepository, EFOrderDetailRepository>();

      services.AddSwaggerGen(cfg =>
      {
        cfg.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SportsStore", Version = "v1" });
      });

      // for setting a SPA (Angular/Vue/React)
      // dotnet add package Microsoft.AspNetCore.SpaServices.Extensions--version 5.0.7
      // services.AddSpaStaticFiles(cfg => { cfg.RootPath = "ClientApp/dist"; });

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseStatusCodePages();
      }

      app.UseSwagger();
      app.UseSwaggerUI(cfg =>
      {
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "SportsStore v1");
      });

      app.UseStaticFiles();

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

        // Identity Database
        IdentityDbContext identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var identityCreateDatabase = identityDbContext.Database.EnsureCreated();
        if (identityCreateDatabase)
        {
          UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
          RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
          SSIdentitySeedData.PopulateIdentity(identityDbContext, userManager, roleManager).Wait();
          logger.LogInformation($"***SSIdentitySeedData Called, '{identityDbContext.Users.Count()}' - Users Added\n'{identityDbContext.Roles.Count()}' - Roles Added***");
        }
      }

      // Setup CORS (Cross Origin Resource Sharing)
      // https://localhost:44375/api/product (enable cors)
      // (another site which requires data from api/product) https://localhost:44378/ access this api/product
      //app.UseCors(cfg => {
      //  cfg.WithOrigins("https://localhost:44378/")
      //    .AllowAnyHeader()
      //    .AllowAnyOrigin()
      //    .AllowAnyMethod();
      //});

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapGet("/", async context =>
              {
                await context.Response.WriteAsync($"<div style='background-color: Cornflowerblue; text-align: center; color: white;'>" +
                  $"<h1>Sports Store Web API Links</h1>" +
                  $"<a href='/swagger/Index.html'>Swagger</a>" +
                  $"</div>");
              });
      });
    }
  }
}
