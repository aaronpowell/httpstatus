using System;
using System.Collections.Generic;

namespace Teapot.Web.Models;

public class TeapotStatusCodeResult
{
    public string Description { get; set; } = "";
    public Dictionary<string, string> IncludeHeaders { get; set; } = new();
    public bool ExcludeBody { get; set; } = true;
    public Uri Link { get; set; } = new Uri("https://httpstat.us");
    public string Body { get; set; } = "";
}