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

        private static readonly int SLEEP_MIN = 0;
        private static readonly int SLEEP_MAX = 300000; // 5 mins in milliseconds

        public ActionResult Index()
        {
            return View(StatusCodes);
        }

        public ActionResult StatusCode(int statusCode, int? sleep = 0)
        {
            var statusData = StatusCodes.ContainsKey(statusCode)
                ? StatusCodes[statusCode]
                : new StatusCodeResult { Description = string.Format("{0} Unknown Code", statusCode) };

            DoSleep(sleep);

            return new CustomHttpStatusCodeResult(statusCode, statusData);
        }

        public ActionResult Cors(int statusCode, int? sleep=0)
        {
            if (Request.HttpMethod != "OPTIONS")
            {
                return StatusCode(statusCode, sleep);
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

            DoSleep(sleep);

            return new CustomHttpStatusCodeResult((int)HttpStatusCode.OK, statusData);
        }

        private static void DoSleep(int? sleep)
        {
            int sleepData = SanitizeSleepParameter(sleep, SLEEP_MIN, SLEEP_MAX);

            if (sleepData > 0)
            {
                System.Threading.Thread.Sleep(sleepData);
            }
        }

        /// <summary>
        /// Limits the sleep parameter 
        /// </summary>
        /// <param name="sleep"></param>
        /// <returns></returns>
        private static int SanitizeSleepParameter(int? sleep, int min, int max)
        {
            var sleepData = sleep ?? 0;

            // range check - minimum should be 0
            if (sleepData < min)
            {
                sleepData = min;
            }

            // range check- maximum should be 300000 (5 mins)
            if (sleepData > max)
            {
                sleepData = max;
            }

            return sleepData;
        }
    }
}
