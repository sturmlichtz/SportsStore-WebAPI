using Microsoft.AspNetCore.Identity;

namespace SSWebAPIApp.Models.Entities
{
  public class SSUser: IdentityUser
  {
    public string FavColor { get; set; }
  }
}
