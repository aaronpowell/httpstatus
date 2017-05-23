using Microsoft.AspNetCore.Mvc;
using Teapot.Web.Models;
using Teapot.Web.WebModels;

namespace Teapot.Web.Controllers
{
    public class StatusController : Controller
    {
        //TODO: remove this hard coded value.
        private static CustomStatusCodeResults customResults = new CustomStatusCodeResults("http://localhost:58447");

        public IActionResult Index()
        {
            return View(customResults);
        }

        public IActionResult Status(int statuscode)
        {
            CustomHttpStatusObjectResult obj;
            if (customResults[statuscode] != null)
                obj = new CustomHttpStatusObjectResult(customResults[statuscode]);
            else
            {
                obj = new CustomHttpStatusObjectResult(new CustomStatusCodeResult
                {
                    StatusCode = 400,
                    Description = statuscode.ToString() + " Bad Request"
                });
            }
            return obj;
        }
    }
}
 
 
 