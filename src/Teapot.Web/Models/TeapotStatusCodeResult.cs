using System;
using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class TeapotStatusCodeResult
    {
        public string Description { get; set; }
        public Dictionary<string, string> IncludeHeaders { get; set; }
        public bool ExcludeBody { get; set; }
        public Uri Link { get; set; }
    }
}