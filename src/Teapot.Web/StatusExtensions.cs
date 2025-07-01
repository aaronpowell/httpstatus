using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Teapot.Web.Configuration;
using Teapot.Web.Models;

namespace Teapot.Web;

internal static class StatusExtensions
{
    public const string SLEEP_HEADER = "X-HttpStatus-Sleep";
    public const string SUPPRESS_BODY_HEADER = "X-HttpStatus-SuppressBody";
    public const string DRIBBLE_BODY_HEADER = "X-HttpStatus-DribbleBody";
    public const string SLEEP_AFTER_HEADERS = "X-HttpStatus-SleepAfterHeaders";
    public const string ABORT_BEFORE_HEADERS = "X-HttpStatus-AbortBeforeHeaders";
    public const string ABORT_AFTER_HEADERS = "X-HttpStatus-AbortAfterHeaders";
    public const string ABORT_DURING_BODY = "X-HttpStatus-AbortDuringBody";
    public const string CUSTOM_RESPONSE_HEADER_PREFIX = "X-HttpStatus-Response-";

    private static readonly string[] httpMethods = ["Get", "Put", "Post", "Delete", "Head", "Options", "Trace", "Patch"];

    internal static WebApplication MapStatusEndpoints(this WebApplication app, string policyName)
    {
        app.MapMethods("/{status:int}", httpMethods, HandleStatusRequestAsync);
        //.RequireRateLimiting(policyName);
        app.MapMethods("/{status:int}/{*wildcard}", httpMethods, HandleStatusRequestAsync);
        //.RequireRateLimiting(policyName);

        app.MapMethods("/random/{range}", httpMethods, HandleRandomRequest);
        //.RequireRateLimiting(policyName);
        app.MapMethods("/random/{range}/{*wildcard}", httpMethods, HandleRandomRequest);
        //.RequireRateLimiting(policyName);

        app.MapGet("im-a-teapot", () => TypedResults.Redirect("https://www.ietf.org/rfc/rfc2324.txt"));

        return app;
    }

    internal static IResult HandleStatusRequestAsync(
        int status,
        int? sleep,
        bool? suppressBody,
        string? wildcard,
        HttpRequest req,
        [FromServices] TeapotStatusCodeMetadataCollection statusCodes,
        [FromServices] TimeoutOptions timeoutOptions
        )
    {
        TeapotStatusCodeMetadata statusData = statusCodes.TryGetValue(status, out TeapotStatusCodeMetadata? value) ?
            value :
            new TeapotStatusCodeMetadata { Description = $"{status} Unknown Code" };

        ResponseOptions options = new(status, statusData)
        {
            Sleep = sleep,
            SuppressBody = suppressBody
        };
        return CommonHandleStatusRequestAsync(options, wildcard, req, timeoutOptions);
    }

    internal static IResult CommonHandleStatusRequestAsync(
        ResponseOptions options,
        string? wildcard,
        HttpRequest req,
        TimeoutOptions timeoutOptions)
    {
        options.Sleep ??= ParseHeaderInt(req, SLEEP_HEADER);
        options.SleepAfterHeaders ??= ParseHeaderInt(req, SLEEP_AFTER_HEADERS);
        options.SuppressBody ??= ParseHeaderBool(req, SUPPRESS_BODY_HEADER);
        options.DribbleBody ??= ParseHeaderBool(req, DRIBBLE_BODY_HEADER);
        options.AbortAfterHeaders ??= ParseHeaderBool(req, ABORT_AFTER_HEADERS);
        options.AbortBeforeHeaders ??= ParseHeaderBool(req, ABORT_BEFORE_HEADERS);
        options.AbortDuringBody ??= ParseHeaderBool(req, ABORT_DURING_BODY);

        options.CustomHeaders = req.Headers
            .Where(header => header.Key.StartsWith(CUSTOM_RESPONSE_HEADER_PREFIX, StringComparison.InvariantCultureIgnoreCase))
            .ToDictionary(
                header => header.Key.Replace(CUSTOM_RESPONSE_HEADER_PREFIX, string.Empty, StringComparison.InvariantCultureIgnoreCase),
                header => header.Value);

        return new CustomHttpStatusCodeResult(options, timeoutOptions.MaxSleepMilliseconds);
    }

    internal static IResult HandleRandomRequest(
        HttpRequest req,
        [FromServices] TeapotStatusCodeMetadataCollection statusCodes,
        [FromServices] TimeoutOptions timeoutOptions,
        int? sleep,
        bool? suppressBody,
        string? wildcard,
        string range = "100-599")
    {
        try
        {
            var status = GetRandomStatus(range);
            TeapotStatusCodeMetadata statusData = statusCodes.TryGetValue(status, out TeapotStatusCodeMetadata? value) ?
                value :
                new TeapotStatusCodeMetadata { Description = $"{status} Unknown Code" };

            ResponseOptions options = new(status, statusData);
            return CommonHandleStatusRequestAsync(options, wildcard, req, timeoutOptions);
        }
        catch
        {
            return TypedResults.BadRequest();
        }
    }

    private static int? ParseHeaderInt(HttpRequest req, string headerName)
    {
        if (req.Headers.TryGetValue(headerName, out StringValues values) && values.Count == 1 && values[0] is not null)
        {
            string? val = values[0];
            if (int.TryParse(val, out int value))
            {
                return value;
            }
        }

        return null;
    }

    private static bool? ParseHeaderBool(HttpRequest req, string headerName)
    {
        if (req.Headers.TryGetValue(headerName, out StringValues values) && values.Count == 1 && values[0] is not null)
        {
            string? val = values[0];
            if (bool.TryParse(val, out bool value))
            {
                return value;
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
