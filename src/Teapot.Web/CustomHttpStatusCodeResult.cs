using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using Teapot.Web.Models;

namespace Teapot.Web;

public class CustomHttpStatusCodeResult : StatusCodeResult
{
    private readonly TeapotStatusCodeResult _statusCodeResult;

    public CustomHttpStatusCodeResult([ActionResultStatusCode] int statusCode, TeapotStatusCodeResult statusCodeResult)
        : base(statusCode)
    {
        _statusCodeResult = statusCodeResult;
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        await base.ExecuteResultAsync(context);

        if (!string.IsNullOrEmpty(_statusCodeResult.Description))
        {
            var httpResponseFeature = context.HttpContext.Features.Get<IHttpResponseFeature>();
            if (httpResponseFeature is not null)
            {
                httpResponseFeature.ReasonPhrase = _statusCodeResult.Description;
            }
        }

        if (_statusCodeResult.IncludeHeaders is not null)
        {
            foreach (var header in _statusCodeResult.IncludeHeaders)
            {
                context.HttpContext.Response.Headers.Add(header.Key, header.Value);
            }
        }

        if (_statusCodeResult.ExcludeBody)
        {
            //remove Content-Length and Content-Type when there isn't any body
            if (context.HttpContext.Response.Headers.ContainsKey("Content-Length"))
            {
                context.HttpContext.Response.Headers.Remove("Content-Length");
            }

            if (context.HttpContext.Response.Headers.ContainsKey("Content-Type"))
            {
                context.HttpContext.Response.Headers.Remove("Content-Type");
            }
        }
        else
        {
            var acceptTypes = context.HttpContext.Request.GetTypedHeaders().Accept;

            if (acceptTypes is not null)
            {
                var (body, contentType) = acceptTypes.Contains(new MediaTypeHeaderValue("application/json")) switch
                {
                    true => (JsonSerializer.Serialize(new { code = StatusCode, description = _statusCodeResult.Body ?? _statusCodeResult.Description }), "application/json"),
                    false => (_statusCodeResult.Body ?? $"{StatusCode} {_statusCodeResult.Description}", "text/plain")
                };

                context.HttpContext.Response.ContentType = contentType;
                context.HttpContext.Response.ContentLength = body.Length;
                await context.HttpContext.Response.WriteAsync(body);
            }
        }
    }
}
