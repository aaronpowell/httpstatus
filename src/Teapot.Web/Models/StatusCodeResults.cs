using System;
using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class StatusCodeResults : Dictionary<int, StatusCodeResult>
    {
        public StatusCodeResults()
        {
            Add(100, new StatusCodeResult
            {
                Description = "Continue",
                ExcludeBody = true
            });
            Add(101, new StatusCodeResult
            {
                Description = "Switching Protocols",
                ExcludeBody = true
            });
            Add(102, new StatusCodeResult
            {
                Description = "Processing",
                ExcludeBody = true
            });
            Add(103, new StatusCodeResult
            {
                Description = "Early Hints",
                ExcludeBody = true,
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Link", "</Content/main.css>; rel=preload"}
                }
            });
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
            Add(207, new StatusCodeResult
            {
                Description = "Multi-Status"
            });
            Add(208, new StatusCodeResult
            {
                Description = "Already Reported"
            });
            Add(210, new StatusCodeResult
            {
                Description = "Content Different"
            });
            Add(226, new StatusCodeResult
            {
                Description = "IM Used"
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
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(302, new StatusCodeResult
            {
                Description = "Found",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(303, new StatusCodeResult
            {
                Description = "See Other",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
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
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
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
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(308, new StatusCodeResult
            {
                Description = "Permanent Redirect",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "https://httpstat.us"}
                }
            });
            Add(310, new StatusCodeResult
            {
                Description = "Too many Redirects"
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
                Description = "Precondition Failed"
            });
            Add(413, new StatusCodeResult
            {
                Description = "Request Entity Too Large"
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
                Link = new Uri("https://www.ietf.org/rfc/rfc2324.txt")
            });
            Add(421, new StatusCodeResult
            {
                Description = "Misdirected Request"
            });
            Add(422, new StatusCodeResult
            {
                Description = "Unprocessable Entity"
            });
            Add(423, new StatusCodeResult
            {
                Description = "Locked"
            });
            Add(424, new StatusCodeResult
            {
                Description = "Method failure"
            });
            Add(425, new StatusCodeResult
            {
                Description = "Too Early"
            });
            Add(426, new StatusCodeResult
            {
                Description = "Upgrade Required"
            });
            Add(428, new StatusCodeResult
            {
                Description = "Precondition Required"
            });
            Add(429, new StatusCodeResult
            {
                Description = "Too Many Requests",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Retry-After", "3600"}
                }
            });
            Add(431, new StatusCodeResult
            {
                Description = "Request Header Fields Too Large"
            });
            Add(449, new StatusCodeResult
            {
                Description = "Retry With"
            });
            Add(450, new StatusCodeResult
            {
                Description = "Blocked by Windows Parental Controls"
            });
            Add(451, new StatusCodeResult
            {
                Description = "Unavailable For Legal Reasons"
            });
            Add(456, new StatusCodeResult
            {
                Description = "Unrecoverable Error"
            });
            Add(495, new StatusCodeResult
            {
                Description = "SSL Certificate Error"
            });
            Add(496, new StatusCodeResult
            {
                Description = "SSL Certificate Required"
            });
            Add(497, new StatusCodeResult
            {
                Description = "HTTP Request Sent to HTTPS Port"
            });
            Add(498, new StatusCodeResult
            {
                Description = "Token expired/invalid"
            });
            Add(499, new StatusCodeResult
            {
                Description = "Client Closed Request"
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
            Add(506, new StatusCodeResult
            {
                Description = "Variant Also Negotiates"
            });
            Add(507, new StatusCodeResult
            {
                Description = "Insufficient storage"
            });
            Add(508, new StatusCodeResult
            {
                Description = "Loop detected"
            });
            Add(509, new StatusCodeResult
            {
                Description = "Bandwidth Limit Exceeded"
            });
            Add(510, new StatusCodeResult
            {
                Description = "Not Extended"
            });
            Add(511, new StatusCodeResult
            {
                Description = "Network Authentication Required"
            });
            Add(520, new StatusCodeResult
            {
                Description = "Unknown Error"
            });
            Add(521, new StatusCodeResult
            {
                Description = "Web Server Is Down"
            });
            Add(522, new StatusCodeResult
            {
                Description = "Connection Timed Out"
            });
            Add(524, new StatusCodeResult
            {
                Description = "A Timeout Occurred"
            });
            Add(525, new StatusCodeResult
            {
                Description = "SSL Handshake Failed"
            });
            Add(526, new StatusCodeResult
            {
                Description = "Invalid SSL Certificate"
            });
            Add(527, new StatusCodeResult
            {
                Description = "Railgun Error"
            });
        }
    }
}
