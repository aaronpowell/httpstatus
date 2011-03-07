using System;
using System.Web.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers
{
    public class StatusController : Controller
    {
        private static readonly StatusCodes _statusCodes = new StatusCodes();

        public ActionResult Index()
        {
            return View(_statusCodes);
        }

        public ActionResult StatusCode(string status)
        {
            if (_statusCodes.ContainsKey(status))
            {
                return new HttpStatusCodeResult(int.Parse(status));
            }

            throw new NotImplementedException();
        }
    }
}
