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

        private const int SLEEP_MIN = 0;
        private const int SLEEP_MAX = 300000; // 5 mins in milliseconds
        private const int FAIL_CHANCE_MIN = 1;
        private const int FAIL_CHANCE_MAX = 100;
 
        public ActionResult Index()
        {
            return View(StatusCodes);
        }

        public ActionResult StatusCode(int statusCode, string message = null, int? sleep = SLEEP_MIN, int? failChance = null)
        {
            if (MaybeFail(failChance))
            {
                statusCode = 403;
            }
 
            var statusData = StatusCodes.ContainsKey(statusCode)
                ? StatusCodes[statusCode]
                : new StatusCodeResult { Description = $"{statusCode} {message ?? "Unknown Code" }" };

            DoSleep(sleep);
 
            return new CustomHttpStatusCodeResult(statusCode, statusData);
        }

        public ActionResult Cors(int statusCode, int? sleep = SLEEP_MIN)
        {
            if (Request.HttpMethod != "OPTIONS")
            {
                return StatusCode(statusCode, null, sleep);
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
            int sleepData = SanitizeIntegerParameter(sleep, SLEEP_MIN, SLEEP_MAX);

            if (sleepData > 0)
            {
                System.Threading.Thread.Sleep(sleepData);
            }
        }

        private static bool MaybeFail(int? failChance)
        {
            int failData = SanitizeIntegerParameter(failChance, FAIL_CHANCE_MIN, FAIL_CHANCE_MAX);

            if (failData > 0)
            {
                System.Random random = new System.Random();

                return random.Next(0, failData) == 1;
            }

            return false;
        }
  
        private static int SanitizeIntegerParameter(int? parameter, int min, int max)
        {
            var parameterData = parameter ?? min;

            // range check - minimum should be min
            if (parameterData < min)
            {
                parameterData = min;
            }

            // range check- maximum should be max
            if (parameterData > max)
            {
                parameterData = max;
            }

            return parameterData;
        }
    }
}