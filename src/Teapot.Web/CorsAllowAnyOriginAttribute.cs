using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Teapot.Web
{
    public class CorsAllowAnyOriginAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Response.Headers["Access-Control-Allow-Origin"] == null)
            {
                filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            if (filterContext.HttpContext.Request.Headers["Access-Control-Request-Headers"] != null)
            {
                filterContext.HttpContext.Response.Headers.Add(
                    "Access-Control-Allow-Headers",
                    filterContext.HttpContext.Request.Headers["Access-Control-Request-Headers"]
                );
            }

            var accessControlExposeHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            accessControlExposeHeaders.Add("Link"); // 103
            accessControlExposeHeaders.Add("Content-Range"); // 206
            accessControlExposeHeaders.Add("Location"); // 301, 302, 303, 305, 307, 308
            accessControlExposeHeaders.Add("WWW-Authenticate"); // 401
            accessControlExposeHeaders.Add("Proxy-Authenticate"); // 407
            accessControlExposeHeaders.Add("Retry-After"); // 429

            filterContext.HttpContext.Response.Headers.Add(
                "Access-Control-Expose-Headers",
                string.Join(", ", accessControlExposeHeaders));

            base.OnResultExecuted(filterContext);
        }
    }
}
