using Microsoft.AspNetCore.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers
{
    public class RedirectedController : Controller
    {
        public IActionResult Index(int statuscode)
        {
            string message = string.Empty;
            if (statuscode == 301)
                message = "301 permanent redirect as resource completed has been served on this link";
            else if (statuscode == 308)
                message = "308 permanent redirect as resource completed has been served on this link";
            else if (statuscode == 303)
                message = "303 see other resource completed has been served on this link";
            else if (statuscode == 307)
                message = "307 temporary redirect page has been served on this link";
            else if (statuscode == 302)
                message = "302 found temporary redirect page on this link";
            else
                message = "invalid request to this page";
            return View(new MessageResult() { Message = message });
        }

    }
}