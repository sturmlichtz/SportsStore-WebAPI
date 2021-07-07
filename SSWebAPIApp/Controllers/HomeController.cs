using Microsoft.AspNetCore.Mvc;

namespace SSWebAPIApp.Controllers
{
  [Route("[controller]/[action]")]
  public class HomeController : Controller
  {
    public IActionResult Index() => View();
  }
}
