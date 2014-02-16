using System;
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
                    {"Location", "http://httpstat.us"}
                }
            });
            Add(302, new StatusCodeResult
            {
                Description = "Found",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "http://httpstat.us"}
                }
            });
            Add(303, new StatusCodeResult
            {
                Description = "See Other",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "http://httpstat.us"}
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
            Add(306, new StatusCodeResult
            {
                Description = "Unused"
            });
            Add(307, new StatusCodeResult
            {
                Description = "Temporary Redirect",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "http://httpstat.us"}
                }
            });
            Add(400, new StatusCodeResult
            {
                Description = "Bad Request"
            });
            Add(401, new StatusCodeResult
            {
                Description = "Unauthorized",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"WWW-Authenticate", "Basic realm=\"Fake Realm\""}
                }
            });
            Add(402, new StatusCodeResult
            {
                Description = "Payment Required"
            });
            Add(403, new StatusCodeResult
            {
                Description = "Forbidden"
            });
            Add(404, new StatusCodeResult
            {
                Description = "Not Found"
            });
            Add(405, new StatusCodeResult
            {
                Description = "Method Not Allowed"
            });
            Add(406, new StatusCodeResult
            {
                Description = "Not Acceptable"
            });
            Add(407, new StatusCodeResult
            {
                Description = "Proxy Authentication Required",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Proxy-Authenticate", "Basic realm=\"Fake Realm\""}
                }
            });
            Add(408, new StatusCodeResult
            {
                Description = "Request Timeout"
            });
            Add(409, new StatusCodeResult
            {
                Description = "Conflict"
            });
            Add(410, new StatusCodeResult
            {
                Description = "Gone"
            });
            Add(411, new StatusCodeResult
            {
                Description = "Length Required"
            });
            Add(412, new StatusCodeResult
            {
                Description = "Precondition Required"
            });
            Add(413, new StatusCodeResult
            {
                Description = "Request Entry Too Large"
            });
            Add(414, new StatusCodeResult
            {
                Description = "Request-URI Too Long"
            });
            Add(415, new StatusCodeResult
            {
                Description = "Unsupported Media Type"
            });
            Add(416, new StatusCodeResult
            {
                Description = "Requested Range Not Satisfiable"
            });
            Add(417, new StatusCodeResult
            {
                Description = "Expectation Failed"
            });
            Add(418, new StatusCodeResult
            {
                Description = "I'm a teapot",
                Link = new Uri("http://www.ietf.org/rfc/rfc2324.txt")
            });
            Add(500, new StatusCodeResult
            {
                Description = "Internal Server Error"
            });
            Add(501, new StatusCodeResult
            {
                Description = "Not Implemented"
            });
            Add(502, new StatusCodeResult
            {
                Description = "Bad Gateway"
            });
            Add(503, new StatusCodeResult
            {
                Description = "Service Unavailable"
            });
            Add(504, new StatusCodeResult
            {
                Description = "Gateway Timeout"
            });
            Add(505, new StatusCodeResult
            {
                Description = "HTTP Version Not Supported"
            });
        }
    }
}
