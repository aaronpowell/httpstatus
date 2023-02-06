using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers;

public class StatusController : Controller {
    public const string SLEEP_HEADER = "X-HttpStatus-Sleep";

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

        return new CustomHttpStatusCodeResult(statusCode, statusData, sleep);
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
