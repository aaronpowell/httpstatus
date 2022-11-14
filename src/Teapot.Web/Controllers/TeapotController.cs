using Microsoft.AspNetCore.Mvc;

namespace Teapot.Web.Controllers;

public class TeapotController : Controller
{
    [HttpGet("im-a-teapot", Name = "I'm a teapot")]
    public IActionResult Teapot() => Redirect("https://www.ietf.org/rfc/rfc2324.txt");

    [HttpGet("teapot", Name = "Default")]
    public IActionResult Index() => View();
}
