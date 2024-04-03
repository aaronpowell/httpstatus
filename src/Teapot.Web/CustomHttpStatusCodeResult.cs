using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Teapot.Web.Models;

namespace Teapot.Web;

public class CustomHttpStatusCodeResult : StatusCodeResult {
    private const int SLEEP_MIN = 0;
    private const int SLEEP_MAX = 5 * 60 * 1000; // 5 mins in milliseconds

    private readonly TeapotStatusCodeResult _statusCodeResult;
    private readonly int? _sleep;
    private readonly bool? _suppressBody;
    private readonly Dictionary<string, StringValues> _customResponseHeaders;

    public int? Sleep => _sleep;

    public bool? SuppressBody => _suppressBody;

    public CustomHttpStatusCodeResult([ActionResultStatusCode] int statusCode, TeapotStatusCodeResult statusCodeResult, int? sleep, bool? suppressBody, Dictionary<string, StringValues> customResponseHeaders)
        : base(statusCode) {
        _statusCodeResult = statusCodeResult;
        _sleep = sleep;
        _suppressBody = suppressBody;
        _customResponseHeaders = customResponseHeaders;
    }

    public override async Task ExecuteResultAsync(ActionContext context) {
        await DoSleep(Sleep);

        await base.ExecuteResultAsync(context);

        if (!string.IsNullOrEmpty(_statusCodeResult.Description)) {
            var httpResponseFeature = context.HttpContext.Features.Get<IHttpResponseFeature>();
            if (httpResponseFeature is not null) {
                httpResponseFeature.ReasonPhrase = _statusCodeResult.Description;
            }
        }

        if (_statusCodeResult.IncludeHeaders is not null) {
            foreach ((var header, var values) in _statusCodeResult.IncludeHeaders) {
                context.HttpContext.Response.Headers.Add(header, values);
            }
        }

        foreach ((string header, StringValues values) in _customResponseHeaders)
        {
            if (string.Equals(header, "Set-Cookie", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var value in values)
                {
                    context.HttpContext.Response.Headers.Append("Set-Cookie", value);
                }
            }
            else
            {
                context.HttpContext.Response.Headers[header] = values;
            }
        }

        if (_statusCodeResult.ExcludeBody || _suppressBody == true) {
            //remove Content-Length and Content-Type when there isn't any body
            context.HttpContext.Response.Headers.Remove("Content-Length");
            context.HttpContext.Response.Headers.Remove("Content-Type");
        } else {
            var acceptTypes = context.HttpContext.Request.GetTypedHeaders().Accept;

            if (acceptTypes is not null) {
                var (body, contentType) = acceptTypes.Contains(new MediaTypeHeaderValue("application/json")) switch {
                    true => (JsonSerializer.Serialize(new { code = StatusCode, description = _statusCodeResult.Body ?? _statusCodeResult.Description }), "application/json"),
                    false => (_statusCodeResult.Body ?? $"{StatusCode} {_statusCodeResult.Description}", "text/plain")
                };

                context.HttpContext.Response.ContentType = contentType;
                context.HttpContext.Response.ContentLength = body.Length;
                await context.HttpContext.Response.WriteAsync(body);
            }
        }
    }

    private static async Task DoSleep(int? sleep) {
        var sleepData = Math.Clamp(sleep ?? 0, SLEEP_MIN, SLEEP_MAX);
        if (sleepData > 0) {
            await Task.Delay(sleepData);
        }
    }

}
