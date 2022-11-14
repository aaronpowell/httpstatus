using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers;

public class StatusController : Controller
{
    private readonly TeapotStatusCodeResults _statusCodes;

    public StatusController(TeapotStatusCodeResults statusCodes)
    {
        _statusCodes = statusCodes;
    }

    [Route("")]
    public IActionResult Index() => View(_statusCodes);

    [Route("{statusCode:int}", Name = "StatusCode")]
    [Route("{statusCode:int}/{*wildcard}", Name = "StatusCodeWildcard")]
    public async Task<IActionResult> StatusCode(int statusCode, int? sleep)
    {
        var statusData = _statusCodes.ContainsKey(statusCode)
            ? _statusCodes[statusCode]
            : new TeapotStatusCodeResult { Description = $"{statusCode} Unknown Code" };

        await DoSleep(sleep);

        return new CustomHttpStatusCodeResult(statusCode, statusData);
    }

    private static async Task DoSleep(int? sleep)
    {
        const int SLEEP_MIN = 0;
        const int SLEEP_MAX = 5 * 60 * 1000; // 5 mins in milliseconds

        var sleepData = Math.Clamp(sleep ?? 0, SLEEP_MIN, SLEEP_MAX);
        if (sleepData > 0)
        {
            await Task.Delay(sleepData);
        }
    }
}
