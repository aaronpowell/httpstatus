using System;
using System.Collections.Generic;

namespace Teapot.Web.Models;

public class TeapotStatusCodeResult
{
    public string Description { get; set; } = "";
    public Dictionary<string, string> IncludeHeaders { get; set; } = new();
    public bool ExcludeBody { get; set; } = false;
    public Uri? Link { get; set; }
    public string? Body { get; set; }
    public bool IsNonStandard { get; init; }
}