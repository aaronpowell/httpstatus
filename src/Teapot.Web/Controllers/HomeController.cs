using Microsoft.AspNetCore.Mvc;

namespace Teapot.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
