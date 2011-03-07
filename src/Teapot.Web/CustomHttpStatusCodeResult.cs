using System.Web.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web
{
    public class CustomHttpStatusCodeResult : HttpStatusCodeResult
    {
        private readonly StatusCodeResult _statusData;

        public CustomHttpStatusCodeResult(int statusCode, StatusCodeResult statusData)
            : base(statusCode, statusData.Description)
        {
            _statusData = statusData;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (_statusData.ExcludeBody)
            {
                if (!string.IsNullOrEmpty(context.HttpContext.Response.Headers["Content-Length"]))
                    context.HttpContext.Response.Headers.Remove("Content-Length");

                if (!string.IsNullOrEmpty(context.HttpContext.Response.Headers["Content-Type"]))
                    context.HttpContext.Response.Headers.Remove("Content-Type");
            }

            if (_statusData.IncludeHeaders != null)
                foreach (var header in _statusData.IncludeHeaders)
                    context.HttpContext.Response.Headers.Add(header.Key, header.Value);

            base.ExecuteResult(context);
        }
    }
}