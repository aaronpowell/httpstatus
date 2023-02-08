using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using Teapot.Web.Models;

namespace Teapot.Web.Controllers;

public class StatusController : Controller {
    public const string SLEEP_HEADER = "X-HttpStatus-Sleep";
    public const string CUSTOM_RESPONSE_HEADER_PREFIX = "X-HttpStatus-Response-";

    private readonly TeapotStatusCodeResults _statusCodes;
    private readonly IRandomSequenceGenerator _randomSequenceGenerator;

    public StatusController(TeapotStatusCodeResults statusCodes, IRandomSequenceGenerator randomSequenceGenerator)
    {
        _statusCodes = statusCodes;
        _randomSequenceGenerator = randomSequenceGenerator;
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

    [Route("Random/{range?}", Name = "Random")]
    [Route("Random/{range?}/{*wildcard}", Name = "RandomWildcard")]
    public IActionResult Random(string range = "100-599", int? sleep = null)
    {
        if (_randomSequenceGenerator.TryParse(range, out var random))
        {
            var statusCode = random.Next;
            return StatusCode(statusCode, sleep);
        }
        return new StatusCodeResult((int)HttpStatusCode.BadRequest);
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