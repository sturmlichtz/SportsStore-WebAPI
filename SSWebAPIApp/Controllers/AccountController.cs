using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using SSWebAPIApp.Models.ViewModels;
using System.Text;

namespace SSWebAPIApp.Controllers
{
  [Route("[controller]/[action]")]
  public class AccountController : Controller
  {
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
      _configuration = configuration;
      _userManager = userManager;
      _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(model.Username);
        if (user != null)
        {
          var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
          if (result.Succeeded)
          {
            // Create Jwt token
            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Email), // sub is name of the subject
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Jti is unique string rep of the token
              new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            // Key for encryption of the token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _configuration["Tokens:Issuer"],
              _configuration["Tokens:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Tokens:Expires"])),
              signingCredentials: creds);

            var results = new
            {
              token = new JwtSecurityTokenHandler().WriteToken(token),
              expiration = token.ValidTo
            };

            return Created("", results);
          }
        }
      }

      return BadRequest($"Username/password is incorrect");
    }
  }
}
