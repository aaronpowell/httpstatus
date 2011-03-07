using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class StatusCodeResults : Dictionary<int, StatusCodeResult>
    {
        public StatusCodeResults()
               {
            Add(200, new StatusCodeResult
                           {
                               Description = "OK",
                           });
            Add(201, new StatusCodeResult
                           {
                               Description = "Created",
                           });
            Add(202, new StatusCodeResult
                           {
                               Description = "Accepted"
                           });
            Add(203, new StatusCodeResult
                           {
                               Description = "Non-Authoritative Information"
                           });
            Add(204, new StatusCodeResult
                           {
                               Description = "No Content",
                               ExcludeBody = true
                           });
            Add(205, new StatusCodeResult
                           {
                               Description = "Reset Content",
                               ExcludeBody = true
                           });
            Add(206, new StatusCodeResult
                           {
                               Description = "Partial Content",
                               IncludeHeaders = new Dictionary<string, string>
                                                    {
                                                        {"Content-Range", "0-30"}
                                                    }
                           });
            Add(300, new StatusCodeResult
                           {
                               Description = "Multiple Choices"
                           });
            Add(301, new StatusCodeResult
                           {
                               Description = "Moved Permanently",
                               IncludeHeaders = new Dictionary<string, string>
                                                    {
                                                        {"Locaation", "http://httpstat.us"}
                                                    }
                           });
            Add(302, new StatusCodeResult
                           {
                               Description = "Found",
                               IncludeHeaders = new Dictionary<string, string>
                                                    {
                                                        {"Locaation", "http://httpstat.us"}
                                                    }
                           });
            Add(303, new StatusCodeResult
                           {
                               Description = "See Other",
                               IncludeHeaders = new Dictionary<string, string>
                                                    {
                                                        {"Locaation", "http://httpstat.us"}
                                                    }
                           });
            Add(304, new StatusCodeResult
                           {
                               Description = "Not Modified",
                               ExcludeBody = true
                           });
            Add(305, new StatusCodeResult
                           {
                               Description = "Use Proxy",
                               ExcludeBody = true
                           });
        }
    }
}