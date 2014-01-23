using System.Web.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers
{
    public class StatusController : Controller
    {
        static readonly StatusCodeResults StatusCodes = new StatusCodeResults();

        public ActionResult Index()
        {
            return View(StatusCodes);
        }

        public ActionResult StatusCode(int statusCode)
        {
            var statusData = StatusCodes.ContainsKey(statusCode)
                ? StatusCodes[statusCode]
                : new StatusCodeResult {Description = string.Format("{0} Unknown Code", statusCode)};

            return new CustomHttpStatusCodeResult(statusCode, statusData);
        }
    }
}
