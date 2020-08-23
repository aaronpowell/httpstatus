using System;
using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class TeapotStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
    {
        public TeapotStatusCodeResults()
        {
            Add(100, new TeapotStatusCodeResult
            {
                Description = "Continue",
                ExcludeBody = true
            });
            Add(101, new TeapotStatusCodeResult
            {
                Description = "Switching Protocols",
                ExcludeBody = true
            });
            Add(102, new TeapotStatusCodeResult
            {
                Description = "Processing",
                ExcludeBody = true
            });
            Add(103, new TeapotStatusCodeResult
            {
                Description = "Early Hints",
                ExcludeBody = true,
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Link", "</css/main.css>; rel=preload"}
                }
            });
            Add(200, new TeapotStatusCodeResult
            {
                Description = "OK",
            });
            Add(201, new TeapotStatusCodeResult
            {
                Description = "Created",
            });
            Add(202, new TeapotStatusCodeResult
            {
                Description = "Accepted"
            });
            Add(203, new TeapotStatusCodeResult
            {
                Description = "Non-Authoritative Information"
            });
            Add(204, new TeapotStatusCodeResult
            {
                Description = "No Content",
                ExcludeBody = true
            });
            Add(205, new TeapotStatusCodeResult
            {
                Description = "Reset Content",
                ExcludeBody = true
            });
            Add(206, new TeapotStatusCodeResult
            {
                Description = "Partial Content",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Content-Range", "0-30"}
                }
            });
            Add(300, new TeapotStatusCodeResult
            {
                Description = "Multiple Choices"
            });
            Add(301, new TeapotStatusCodeResult
            {
                Description = "Moved Permanently",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(302, new TeapotStatusCodeResult
            {
                Description = "Found",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(303, new TeapotStatusCodeResult
            {
                Description = "See Other",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(304, new TeapotStatusCodeResult
            {
                Description = "Not Modified",
                ExcludeBody = true
            });
            Add(305, new TeapotStatusCodeResult
            {
                Description = "Use Proxy",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(306, new TeapotStatusCodeResult
            {
                Description = "Unused"
            });
            Add(307, new TeapotStatusCodeResult
            {
                Description = "Temporary Redirect",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(308, new TeapotStatusCodeResult
            {
                Description = "Permanent Redirect",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(400, new TeapotStatusCodeResult
            {
                Description = "Bad Request"
            });
            Add(401, new TeapotStatusCodeResult
            {
                Description = "Unauthorized",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"WWW-Authenticate", "Basic realm=\"Fake Realm\""}
                }
            });
            Add(402, new TeapotStatusCodeResult
            {
                Description = "Payment Required"
            });
            Add(403, new TeapotStatusCodeResult
            {
                Description = "Forbidden"
            });
            Add(404, new TeapotStatusCodeResult
            {
                Description = "Not Found"
            });
            Add(405, new TeapotStatusCodeResult
            {
                Description = "Method Not Allowed"
            });
            Add(406, new TeapotStatusCodeResult
            {
                Description = "Not Acceptable"
            });
            Add(407, new TeapotStatusCodeResult
            {
                Description = "Proxy Authentication Required",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Proxy-Authenticate", "Basic realm=\"Fake Realm\""}
                }
            });
            Add(408, new TeapotStatusCodeResult
            {
                Description = "Request Timeout"
            });
            Add(409, new TeapotStatusCodeResult
            {
                Description = "Conflict"
            });
            Add(410, new TeapotStatusCodeResult
            {
                Description = "Gone"
            });
            Add(411, new TeapotStatusCodeResult
            {
                Description = "Length Required"
            });
            Add(412, new TeapotStatusCodeResult
            {
                Description = "Precondition Failed"
            });
            Add(413, new TeapotStatusCodeResult
            {
                Description = "Request Entity Too Large"
            });
            Add(414, new TeapotStatusCodeResult
            {
                Description = "Request-URI Too Long"
            });
            Add(415, new TeapotStatusCodeResult
            {
                Description = "Unsupported Media Type"
            });
            Add(416, new TeapotStatusCodeResult
            {
                Description = "Requested Range Not Satisfiable"
            });
            Add(417, new TeapotStatusCodeResult
            {
                Description = "Expectation Failed"
            });
            Add(418, new TeapotStatusCodeResult
            {
                Description = "I'm a teapot",
                Link = new Uri("https://www.ietf.org/rfc/rfc2324.txt")
            });
            Add(421, new TeapotStatusCodeResult
            {
                Description = "Misdirected Request"
            });
            Add(422, new TeapotStatusCodeResult
            {
                Description = "Unprocessable Entity"
            });
            Add(423, new TeapotStatusCodeResult
            {
                Description = "Locked"
            });
            Add(425, new TeapotStatusCodeResult
            {
                Description = "Too Early"
            });
            Add(426, new TeapotStatusCodeResult
            {
                Description = "Upgrade Required"
            });
            Add(428, new TeapotStatusCodeResult
            {
                Description = "Precondition Required"
            });
            Add(429, new TeapotStatusCodeResult
            {
                Description = "Too Many Requests",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Retry-After", "5"}
                }
            });
            Add(431, new TeapotStatusCodeResult
            {
                Description = "Request Header Fields Too Large"
            });
            Add(451, new TeapotStatusCodeResult
            {
                Description = "Unavailable For Legal Reasons"
            });
            Add(500, new TeapotStatusCodeResult
            {
                Description = "Internal Server Error"
            });
            Add(501, new TeapotStatusCodeResult
            {
                Description = "Not Implemented"
            });
            Add(502, new TeapotStatusCodeResult
            {
                Description = "Bad Gateway"
            });
            Add(503, new TeapotStatusCodeResult
            {
                Description = "Service Unavailable"
            });
            Add(504, new TeapotStatusCodeResult
            {
                Description = "Gateway Timeout"
            });
            Add(505, new TeapotStatusCodeResult
            {
                Description = "HTTP Version Not Supported"
            });
            Add(506, new TeapotStatusCodeResult
            {
                Description = "Variant Also Negotiates"
            });
            Add(507, new TeapotStatusCodeResult {
                Description = "Insufficient Storage"
            });
            Add(511, new TeapotStatusCodeResult
            {
                Description = "Network Authentication Required"
            });
            Add(520, new TeapotStatusCodeResult
            {
                Description = "Web server is returning an unknown error"
            });
            Add(522, new TeapotStatusCodeResult
            {
                Description = "Connection timed out"
            });
            Add(524, new TeapotStatusCodeResult
            {
                Description = "A timeout occurred"
            });
        }
    }
}
