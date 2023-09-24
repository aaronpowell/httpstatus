using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace Teapot.Web.Models;

public class TeapotStatusCodeResult
{
    public string Description { get; set; } = "";
    public Dictionary<string, string> IncludeHeaders { get; set; } = new();
    public bool ExcludeBody { get; set; } = false;
    public Uri? Link { get; set; }
    public string? Body { get; set; }
    public bool IsNonStandard { get; init; }
    public GetRequestSpecificHeaders? GetRequestSpecificHeaders { get; init; }
    public Dictionary<string, string>? RequestParameters { get; init; }
    public Dictionary<string, string>? RequestHeaders { get; init; }
}

public delegate IDictionary<string, string> GetRequestSpecificHeaders(IQueryCollection query, IHeaderDictionary requestHeaders);