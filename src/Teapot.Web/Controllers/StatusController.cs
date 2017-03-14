using System.Web.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers
{
    [CorsAllowAnyOrigin]
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

    public class CorsAllowAnyOriginAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Response.Headers["Access-Control-Allow-Origin"] == null)
            {
                filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            base.OnResultExecuted(filterContext);
        }
    }
}
