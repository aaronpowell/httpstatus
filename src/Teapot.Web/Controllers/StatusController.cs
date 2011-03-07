using System.Web.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers
{
    public class StatusController : Controller
    {
        private static readonly StatusCodeResults _statusCodes = new StatusCodeResults();

        public ActionResult Index()
        {
            return View(_statusCodes);
        }

        public ActionResult StatusCode(int statusCode)
        {
            var status = _statusCodes.ContainsKey(statusCode)
                ? _statusCodes[statusCode]
                : new StatusCodeResult {Description = string.Format("{0} Unknown Code", statusCode)};

            return new HttpStatusCodeResult(statusCode, status.Description);
        }
    }
}
