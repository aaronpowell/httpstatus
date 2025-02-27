using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Teapot.Web.Models;

namespace Teapot.Web;

public record class ResponseOptions
{
    public ResponseOptions(int statusCode, 
        int? sleep = null, 
        int? sleepAfterHeaders = null, 
        bool? abortBeforeHeaders = null,
        bool? abortAfterHeaders = null,
        bool? abortDuringBody = null,
        bool? suppressBody = null,
        TeapotStatusCodeMetadata? metadata=null,
        Dictionary<string, StringValues>? customHeaders = null)
    {
        StatusCode = statusCode;
        Sleep = sleep;
        SleepAfterHeaders = sleepAfterHeaders;
        AbortBeforeHeaders = abortBeforeHeaders;
        AbortAfterHeaders = abortAfterHeaders;
        AbortDuringBody = abortDuringBody;
        SuppressBody = suppressBody;
        CustomHeaders = customHeaders ?? new Dictionary<string, StringValues>();
        Metadata = metadata ?? new();
    }

    public int StatusCode { get; set; }
    public int? Sleep { get; set; }
    public int? SleepAfterHeaders { get; set; }
    public bool? SuppressBody { get; set; }
    public bool? AbortBeforeHeaders { get; set; }
    public bool? AbortAfterHeaders { get; set; }
    public bool? AbortDuringBody { get; set; }
    public Dictionary<string, StringValues> CustomHeaders { get; set; } 
    public TeapotStatusCodeMetadata Metadata { get; set; }
}
