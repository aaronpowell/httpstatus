﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Teapot.Web;

public class CustomHttpStatusCodeResult(ResponseOptions options, int maxSleepMilliseconds = 30 * 1000) : IResult
{
    private const int SLEEP_MIN = 0;
    private readonly int SLEEP_MAX = maxSleepMilliseconds;
    internal static readonly string[] onlySingleHeader = ["Location", "Retry-After"];

    private static readonly MediaTypeHeaderValue jsonMimeType = new("application/json");

    public ResponseOptions Options => options;

    public async Task ExecuteAsync(HttpContext context)
    {
        await DoSleep(Options.Sleep);

        if (Options.AbortBeforeHeaders == true)
        {
            context.Abort();
            return;
        }


        context.Response.StatusCode = Options.StatusCode;

        if (!string.IsNullOrEmpty(Options.Metadata.Description))
        {
            IHttpResponseFeature? httpResponseFeature = context.Features.Get<IHttpResponseFeature>();
            if (httpResponseFeature is not null)
            {
                httpResponseFeature.ReasonPhrase = Options.Metadata.Description;
            }
        }

        if (Options.Metadata.IncludeHeaders is not null)
        {
            foreach ((string header, string values) in Options.Metadata.IncludeHeaders)
            {
                context.Response.Headers.Append(header, values);
            }
        }

        foreach ((string header, StringValues values) in Options.CustomHeaders)
        {
            if (onlySingleHeader.Contains(header))
            {
                context.Response.Headers[header] = values;
            }
            else
            {
                context.Response.Headers.Append(header, values);
            }
        }

        if (Options.Metadata.ExcludeBody || Options.SuppressBody == true)
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
                true => (JsonSerializer.Serialize(new { code = Options.StatusCode, description = Options.Metadata.Body ?? Options.Metadata.Description }), "application/json"),
                false => (Options.Metadata.Body ?? $"{Options.StatusCode} {Options.Metadata.Description}", "text/plain")
            };

            context.Response.ContentType = contentType;
            context.Response.ContentLength = body.Length;

            await context.Response.StartAsync();

            await DoSleep(Options.SleepAfterHeaders);

            if (Options.AbortAfterHeaders == true)
            {
                await DoSleep(10);
                context.Abort();
                return;
            }

            if (Options.AbortDuringBody == true)
            {
                await context.Response.WriteAsync(body.Substring(0, 1));
                await context.Response.Body.FlushAsync();
                await DoSleep(10);
                context.Abort();
                return;
            }

            if (Options.DribbleBody == true)
            {
                for (int i = 0; i < body.Length; i++)
                {
                    await context.Response.WriteAsync(body[i..(i + 1)]);
                    if (i < 20)
                    {
                        await context.Response.Body.FlushAsync();
                        await DoSleep(10);
                    }
                }
            }
            else
            {
                await context.Response.WriteAsync(body);
            }
        }
    }

    private async Task DoSleep(int? sleep)
    {
        int sleepData = Math.Clamp(sleep ?? 0, SLEEP_MIN, SLEEP_MAX);
        if (sleepData > 0)
        {
            await Task.Delay(sleepData);
        }
    }
}