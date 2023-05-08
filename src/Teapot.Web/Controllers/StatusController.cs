using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers;

public class StatusController : Controller {
    public const string SLEEP_HEADER = "X-HttpStatus-Sleep";
    public const string SUPPRESS_BODY_HEADER = "X-HttpStatus-SuppressBody";
    public const string CUSTOM_RESPONSE_HEADER_PREFIX = "X-HttpStatus-Response-";

    private readonly TeapotStatusCodeResults _statusCodes;

    public StatusController(TeapotStatusCodeResults statusCodes)
    {
        _statusCodes = statusCodes;
    }

    [Route("")]
    public IActionResult Index() => View(_statusCodes);

    [Route("{statusCode:int}", Name = "StatusCode")]
    [Route("{statusCode:int}/{*wildcard}", Name = "StatusCodeWildcard")]
    public IActionResult StatusCode(int statusCode, int? sleep, bool? suppressBody) {
        var statusData = _statusCodes.ContainsKey(statusCode)
            ? _statusCodes[statusCode]
            : new TeapotStatusCodeResult { Description = $"{statusCode} Unknown Code" };

        sleep ??= FindSleepInHeader();
        suppressBody ??= FindSuppressBodyInHeader();

        var customResponseHeaders = HttpContext.Request.Headers
            .Where(header => header.Key.StartsWith(CUSTOM_RESPONSE_HEADER_PREFIX))
            .ToDictionary(header => header.Key.Replace(CUSTOM_RESPONSE_HEADER_PREFIX, string.Empty), header => header.Value);

        return new CustomHttpStatusCodeResult(statusCode, statusData, sleep, suppressBody, customResponseHeaders);
    }

    [Route("Random/{range?}", Name = "Random")]
    [Route("Random/{range?}/{*wildcard}", Name = "RandomWildcard")]
    public IActionResult Random(string range = "100-599", int? sleep = null, bool? suppressBody = null)
    {
        try
        {
            var statusCode = GetRandomStatus(range);
            return StatusCode(statusCode, sleep, suppressBody);
        }
        catch
        {
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }
    }

    private int? FindSleepInHeader() {
        if (HttpContext.Request.Headers.TryGetValue(SLEEP_HEADER, out var sleepHeader) && sleepHeader.Count == 1 && sleepHeader[0] is not null) {
            var val = sleepHeader[0];
            if (int.TryParse(val, out var sleepFromHeader)) {
                return sleepFromHeader;
            }
        }

        return null;
    }
    
    private bool? FindSuppressBodyInHeader()
    {
        if (HttpContext.Request.Headers.TryGetValue(SUPPRESS_BODY_HEADER, out var suppressBodyHeader) && suppressBodyHeader.Count == 1 && suppressBodyHeader[0] is not null)
        {
            var val = suppressBodyHeader[0];
            if (bool.TryParse(val, out var suppressBodyFromHeader))
            {
                return suppressBodyFromHeader;
            }
        }

        return null;
    }

    private int GetRandomStatus(string range)
    {
        // copied from https://stackoverflow.com/a/37213725/260221
        var options = range.Split(',')
                           .Select(x => x.Split('-'))
                           .Select(p => new { First = int.Parse(p.First()), Last = int.Parse(p.Last()) })
                           .SelectMany(x => Enumerable.Range(x.First, x.Last - x.First + 1))
                           .ToArray();

        return options[new Random().Next(options.Length)];
    }
}