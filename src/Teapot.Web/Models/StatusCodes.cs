using System;
using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class StatusCodes : Dictionary<string, StatusCode>
    {
        public StatusCodes()
        {
            Add("200", new StatusCode
                           {
                               Description = "OK",
                           });
            Add("201", new StatusCode
                           {
                               Description = "Created",
                           });
            Add("202", new StatusCode
                           {
                               Description = "Accepted"
                           });
            Add("203", new StatusCode
                           {
                               Description = "Non-Authoritative Information"
                           });
            Add("204", new StatusCode
                           {
                               Description = "No Content",
                               ExcludeHeaders = new[] { "Content-Type", "Content-Length"},
                               ExcludeBody = true
                           });
        }
    }

    public class StatusCode
    {
        public string Description { get; set; }
        public IEnumerable<string> ExcludeHeaders { get; set; }
        public IEnumerable<string> IncludeHeaders { get; set; }
        public bool ExcludeBody { get; set; }
        public Uri Link { get; set; }
    }
}