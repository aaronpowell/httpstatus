using System.Collections.Generic;
using System.Net;
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

        public ActionResult Cors(int statusCode)
        {
            if (Request.HttpMethod != "OPTIONS")
            {
                return StatusCode(statusCode);
            }

            var allowedOrigin = Request.Headers.Get("Origin") ?? "*";
            var allowedMethod = Request.Headers.Get("Access-Control-Request-Method") ?? "GET";
            var allowedHeaders = Request.Headers.Get("Access-Control-Request-Headers") ?? "X-Anything";

            var responseHeaders = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Origin", allowedOrigin },
                { "Access-Control-Allow-Headers", allowedHeaders },
                { "Access-Control-Allow-Methods", allowedMethod }
            };

            var statusData = new StatusCodeResult { IncludeHeaders = responseHeaders };
            return new CustomHttpStatusCodeResult((int)HttpStatusCode.OK, statusData);
        }
    }
}
