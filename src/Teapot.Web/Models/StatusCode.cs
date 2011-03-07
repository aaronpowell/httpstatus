using System;
using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class StatusCode
    {
        public string Description { get; set; }
        public IEnumerable<string> ExcludeHeaders { get; set; }
        public IEnumerable<string> IncludeHeaders { get; set; }
        public bool ExcludeBody { get; set; }
        public Uri Link { get; set; }
    }
}