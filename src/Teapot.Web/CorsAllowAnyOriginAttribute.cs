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

            var accessControlAllowHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            accessControlAllowHeaders.Add("Retry-After");

            var accessControlRequestHeadersHeaderValue = filterContext.HttpContext.Request.Headers["Access-Control-Request-Headers"];
            if (!string.IsNullOrWhiteSpace(accessControlRequestHeadersHeaderValue))
            {
                var accessControlRequestHeaders = accessControlRequestHeadersHeaderValue
                    .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var accessControlRequestHeader in accessControlRequestHeaders)
                {
                    if (!accessControlAllowHeaders.Contains(accessControlRequestHeader))
                    {
                        accessControlAllowHeaders.Add(accessControlRequestHeader);
                    }
                }
            }

            filterContext.HttpContext.Response.Headers.Add(
                "Access-Control-Allow-Headers",
                string.Join(", ", accessControlAllowHeaders));

            base.OnResultExecuted(filterContext);
        }
    }
}
