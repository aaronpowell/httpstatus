using System.Web.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web
{
    public class CustomHttpStatusCodeResult : HttpStatusCodeResult
    {
        public CustomHttpStatusCodeResult(int statusCode, StatusCodeResult statusData)
            : base(statusCode, statusData.Description)
        {
        }
    }
}