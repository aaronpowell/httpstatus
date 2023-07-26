using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Teapot.Web.Models;

namespace Teapot.Web;

public class CustomHttpStatusCodeResult(
    int statusCode,
    TeapotStatusCodeMetadata metadata,
    int? sleep,
    bool? suppressBody,
    Dictionary<string, StringValues> customResponseHeaders) : IResult
{
    private const int SLEEP_MIN = 0;
    private const int SLEEP_MAX = 5 * 60 * 1000; // 5 mins in milliseconds

    private static readonly MediaTypeHeaderValue jsonMimeType = new("application/json");

    public int? Sleep => sleep;

    public bool? SuppressBody => suppressBody;

    public async Task ExecuteAsync(HttpContext context)
    {
        await DoSleep(Sleep);

        context.Response.StatusCode = statusCode;

        if (!string.IsNullOrEmpty(metadata.Description))
        {
            IHttpResponseFeature? httpResponseFeature = context.Features.Get<IHttpResponseFeature>();
            if (httpResponseFeature is not null)
            {
                httpResponseFeature.ReasonPhrase = metadata.Description;
            }
        }

        if (metadata.IncludeHeaders is not null)
        {
            foreach ((string header, string values) in metadata.IncludeHeaders)
            {
                context.Response.Headers.Add(header, values);
            }
        }

        foreach ((string header, StringValues values) in customResponseHeaders)
        {
            context.Response.Headers.Add(header, values);
        }

        if (metadata.ExcludeBody || suppressBody == true)
        {
            //remove Content-Length and Content-Type when there isn't any body
            context.Response.Headers.Remove("Content-Length");
            context.Response.Headers.Remove("Content-Type");
        }
        else
        {
            IList<MediaTypeHeaderValue> acceptTypes = context.Request.GetTypedHeaders().Accept;

            (string body, string contentType) = acceptTypes.Contains(jsonMimeType) switch
            {
                true => (JsonSerializer.Serialize(new { code = statusCode, description = metadata.Body ?? metadata.Description }), "application/json"),
                false => (metadata.Body ?? $"{statusCode} {metadata.Description}", "text/plain")
            };

            context.Response.ContentType = contentType;
            context.Response.ContentLength = body.Length;
            await context.Response.WriteAsync(body);

        }
    }

    private static async Task DoSleep(int? sleep)
    {
        int sleepData = Math.Clamp(sleep ?? 0, SLEEP_MIN, SLEEP_MAX);
        if (sleepData > 0)
        {
            await Task.Delay(sleepData);
        }
    }
}