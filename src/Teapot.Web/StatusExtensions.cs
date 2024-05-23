using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Teapot.Web.Models;

namespace Teapot.Web;

internal static class StatusExtensions
{
    public const string SLEEP_HEADER = "X-HttpStatus-Sleep";
    public const string SUPPRESS_BODY_HEADER = "X-HttpStatus-SuppressBody";
    public const string CUSTOM_RESPONSE_HEADER_PREFIX = "X-HttpStatus-Response-";

    private static readonly string[] httpMethods = ["Get", "Put", "Post", "Delete", "Head", "Options", "Trace", "Patch"];

    internal static WebApplication MapStatusEndpoints(this WebApplication app, string policyName)
    {
        app.MapMethods("/{status:int}", httpMethods, HandleStatusRequestAsync)
            .RequireRateLimiting(policyName);
        app.MapMethods("/{status:int}/{*wildcard}", httpMethods, HandleStatusRequestAsync)
            .RequireRateLimiting(policyName);

        app.MapMethods("/random/{range}", httpMethods, HandleRandomRequest)
            .RequireRateLimiting(policyName);
        app.MapMethods("/random/{range}/{*wildcard}", httpMethods, HandleRandomRequest)
            .RequireRateLimiting(policyName);

        app.MapGet("im-a-teapot", () => TypedResults.Redirect("https://www.ietf.org/rfc/rfc2324.txt"));

        return app;
    }

    internal static IResult HandleStatusRequestAsync(
        int status,
        int? sleep,
        bool? suppressBody,
        string? wildcard,
        HttpRequest req,
        [FromServices] TeapotStatusCodeMetadataCollection statusCodes)
    {
        TeapotStatusCodeMetadata statusData = statusCodes.TryGetValue(status, out TeapotStatusCodeMetadata? value) ?
            value :
            new TeapotStatusCodeMetadata { Description = $"{status} Unknown Code" };
        sleep ??= FindSleepInHeader(req);
        suppressBody ??= FindSuppressBodyInHeader(req);

        Dictionary<string, StringValues> customResponseHeaders = req.Headers
            .Where(header => header.Key.StartsWith(CUSTOM_RESPONSE_HEADER_PREFIX))
            .ToDictionary(
                header => header.Key.Replace(CUSTOM_RESPONSE_HEADER_PREFIX, string.Empty),
                header => header.Value);

        return new CustomHttpStatusCodeResult(status, statusData, sleep, suppressBody, customResponseHeaders);
    }

    internal static IResult HandleRandomRequest(
        HttpRequest req,
        [FromServices] TeapotStatusCodeMetadataCollection statusCodes,
        int? sleep,
        bool? suppressBody,
        string? wildcard,
        string range = "100-599")
    {
        try
        {
            int statusCode = GetRandomStatus(range);
            return HandleStatusRequestAsync(statusCode, sleep, suppressBody, wildcard, req, statusCodes);
        }
        catch
        {
            return TypedResults.BadRequest();
        }
    }

    private static int? FindSleepInHeader(HttpRequest req)
    {
        if (req.Headers.TryGetValue(SLEEP_HEADER, out StringValues sleepHeader) && sleepHeader.Count == 1 && sleepHeader[0] is not null)
        {
            string? val = sleepHeader[0];
            if (int.TryParse(val, out int sleepFromHeader))
            {
                return sleepFromHeader;
            }
        }

        return null;
    }

    private static bool? FindSuppressBodyInHeader(HttpRequest req)
    {
        if (req.Headers.TryGetValue(SUPPRESS_BODY_HEADER, out StringValues suppressBodyHeader) && suppressBodyHeader.Count == 1 && suppressBodyHeader[0] is not null)
        {
            string? val = suppressBodyHeader[0];
            if (bool.TryParse(val, out bool suppressBodyFromHeader))
            {
                return suppressBodyFromHeader;
            }
        }

        return null;
    }

    private static int GetRandomStatus(string range)
    {
        // copied from https://stackoverflow.com/a/37213725/260221
        int[] options = range.Split(',')
                           .Select(x => x.Split('-'))
                           .Select(p => new { First = int.Parse(p.First()), Last = int.Parse(p.Last()) })
                           .SelectMany(x => Enumerable.Range(x.First, x.Last - x.First + 1))
                           .ToArray();

        return options[new Random().Next(options.Length)];
    }
}
