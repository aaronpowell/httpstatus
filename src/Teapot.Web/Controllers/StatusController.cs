using System;
using System.Web.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers
{
    public class StatusController : Controller
    {
        static readonly StatusCodeResults statusCodes = new StatusCodeResults();

        public ActionResult Index()
        {
            return View(statusCodes);
        }

        public ActionResult StatusCode(int statusCode)
        {
            var statusData = statusCodes.ContainsKey(statusCode)
                ? statusCodes[statusCode]
                : new StatusCodeResult {Description = string.Format("{0} Unknown Code", statusCode)};

            return new CustomHttpStatusCodeResult(statusCode, statusData);
        }
    }
}