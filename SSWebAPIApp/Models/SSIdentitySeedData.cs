using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SSWebAPIApp.Models
{
  public static class SSIdentitySeedData
  {
    public static async Task PopulateIdentity(IdentityDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      const string adminRoleName = "RAdmin";
      const string adminUser = "admin@a.com";
      const string tintinUser = "tintin@t.com";
      const string password = "Pass@word1";

      var rAdminRole = await roleManager.FindByNameAsync(adminRoleName);
      if (rAdminRole == null)
      {
        await roleManager.CreateAsync(new IdentityRole(adminRoleName));
      }

      IdentityUser aUser = await userManager.FindByIdAsync(adminUser);
      if (aUser == null)
      {
        aUser = new IdentityUser { UserName = adminUser, Email = adminUser };
        await userManager.CreateAsync(aUser, password);
        await userManager.AddToRoleAsync(aUser, "RAdmin");
      }
      
      IdentityUser tUser = await userManager.FindByIdAsync(tintinUser);
      if (tUser == null)
      {
        tUser = new IdentityUser { UserName = tintinUser, Email = tintinUser };
        await userManager.CreateAsync(tUser, password);
      }

    }
  }
}
