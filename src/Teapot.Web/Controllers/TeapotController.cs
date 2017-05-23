using Microsoft.AspNetCore.Mvc;

namespace Teapot.Web.Controllers
{
    public class TeapotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}