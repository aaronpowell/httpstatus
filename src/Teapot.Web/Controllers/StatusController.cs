using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers;

public class StatusController : Controller {
    public const string SLEEP_HEADER = "X-HttpStatus-Sleep";
    public const string CUSTOM_RESPONSE_HEADER_PREFIX = "X-HttpStatus-Response-";

    private readonly TeapotStatusCodeResults _statusCodes;

    public StatusController(TeapotStatusCodeResults statusCodes) {
        _statusCodes = statusCodes;
    }

    [Route("")]
    public IActionResult Index() => View(_statusCodes);

    [Route("{statusCode:int}", Name = "StatusCode")]
    [Route("{statusCode:int}/{*wildcard}", Name = "StatusCodeWildcard")]
    public IActionResult StatusCode(int statusCode, int? sleep) {
        var statusData = _statusCodes.ContainsKey(statusCode)
            ? _statusCodes[statusCode]
            : new TeapotStatusCodeResult { Description = $"{statusCode} Unknown Code" };

        sleep ??= FindSleepInHeader();

        var customResponseHeaders = HttpContext.Request.Headers
            .Where(header => header.Key.StartsWith(CUSTOM_RESPONSE_HEADER_PREFIX))
            .ToDictionary(header => header.Key.Replace(CUSTOM_RESPONSE_HEADER_PREFIX, string.Empty), header => header.Value);

        return new CustomHttpStatusCodeResult(statusCode, statusData, sleep, customResponseHeaders);
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
}
