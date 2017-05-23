using System;
using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class CustomStatusCodeResults : Dictionary<int, CustomStatusCodeResult>
    {
        public CustomStatusCodeResults(string uriBaseLocation)
        {
            #region 100region
            Add(100, new CustomStatusCodeResult
            {
                StatusCode = 100,
                Description = "continue",
            });
            Add(101, new CustomStatusCodeResult
            {
                StatusCode = 101,
                Description = "switching protocols",
            });
            Add(102, new CustomStatusCodeResult
            {
                StatusCode = 102,
                Description = "processing"
            });
            #endregion
            #region 200region
            Add(200, new CustomStatusCodeResult
            {
                StatusCode = 200,
                Description = "OK",
            });
            Add(201, new CustomStatusCodeResult
            {
                StatusCode = 201,
                Description = "Created",
            });
            Add(202, new CustomStatusCodeResult
            {
                StatusCode = 202,
                Description = "Accepted"
            });
            Add(203, new CustomStatusCodeResult
            {
                StatusCode = 203,
                Description = "Non-Authoritative Information"
            });
            Add(204, new CustomStatusCodeResult
            {
                StatusCode = 204,
                Description = "No Content",
                ExcludeBody = true
            });
            Add(205, new CustomStatusCodeResult
            {
                StatusCode = 205,
                Description = "Reset Content",
                ExcludeBody = true
            });
            Add(206, new CustomStatusCodeResult
            {
                StatusCode = 206,
                Description = "Partial Content",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Content-Range", "0-30"}
                }
            });
            Add(207, new CustomStatusCodeResult
            {
                StatusCode = 207,
                Description = "Multi - Status",
                ExcludeBody = true
            });
            Add(208, new CustomStatusCodeResult
            {
                StatusCode = 208,
                Description = "Already Reported",
                ExcludeBody = true
            });
            Add(226, new CustomStatusCodeResult
            {
                StatusCode = 226,
                Description = "IM Used",
                ExcludeBody = true
            });
            #endregion
            #region 300region
            Add(300, new CustomStatusCodeResult
            {
                StatusCode = 300,
                Description = "Multiple Choices"
            });
            Add(301, new CustomStatusCodeResult
            {
                StatusCode = 301,
                Description = "Moved Permanently",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "/redirected/index/301"}
                }
            });
            Add(302, new CustomStatusCodeResult
            {
                StatusCode = 302,
                Description = "Found",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "/redirected/index/302"}
                }
            });
            Add(303, new CustomStatusCodeResult
            {
                StatusCode = 303,
                Description = "See Other",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "/redirected/index/303"}
                }
            });
            Add(304, new CustomStatusCodeResult
            {
                StatusCode = 304,
                Description = "Not Modified",
                ExcludeBody = true
            });
            Add(305, new CustomStatusCodeResult
            {
                StatusCode = 305,
                Description = "Use Proxy",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "/redirected/index/305"}
                }
            });
            Add(306, new CustomStatusCodeResult
            {
                StatusCode = 306,
                Description = "Unused"
            });
            Add(307, new CustomStatusCodeResult
            {
                StatusCode = 307,
                Description = "Temporary Redirect",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "/redirected/index/307"}
                }
            });
            Add(308, new CustomStatusCodeResult
            {
                StatusCode = 308,
                Description = "Permanent Redirect",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Location", "/redirected/index/308"}
                }
            });
            #endregion
            #region 400region
            Add(400, new CustomStatusCodeResult
            {
                StatusCode = 400,
                Description = "Bad Request"
            });
            Add(401, new CustomStatusCodeResult
            {
                StatusCode = 401,
                Description = "Unauthorized",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"WWW-Authenticate", "Basic realm=\"Fake Realm\""}
                }
            });
            Add(402, new CustomStatusCodeResult
            {
                StatusCode = 402,
                Description = "Payment Required"
            });
            Add(403, new CustomStatusCodeResult
            {
                StatusCode = 403,
                Description = "Forbidden"
            });
            Add(404, new CustomStatusCodeResult
            {
                StatusCode = 404,
                Description = "Not Found"
            });
            Add(405, new CustomStatusCodeResult
            {
                StatusCode = 405,
                Description = "Method Not Allowed"
            });
            Add(406, new CustomStatusCodeResult
            {
                StatusCode = 406,
                Description = "Not Acceptable"
            });
            Add(407, new CustomStatusCodeResult
            {
                StatusCode = 407,
                Description = "Proxy Authentication Required",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Proxy-Authenticate", "Basic realm=\"Fake Realm\""}
                }
            });
            Add(408, new CustomStatusCodeResult
            {
                StatusCode = 408,
                Description = "Request Timeout"
            });
            Add(409, new CustomStatusCodeResult
            {
                StatusCode = 409,
                Description = "Conflict"
            });
            Add(410, new CustomStatusCodeResult
            {
                StatusCode = 410,
                Description = "Gone"
            });
            Add(411, new CustomStatusCodeResult
            {
                StatusCode = 411,
                Description = "Length Required"
            });
            Add(412, new CustomStatusCodeResult
            {
                StatusCode = 412,
                Description = "Precondition Required"
            });
            Add(413, new CustomStatusCodeResult
            {
                StatusCode = 413,
                Description = "Request Entry Too Large"
            });
            Add(414, new CustomStatusCodeResult
            {
                StatusCode = 414,
                Description = "Request-URI Too Long"
            });
            Add(415, new CustomStatusCodeResult
            {
                StatusCode = 415,
                Description = "Unsupported Media Type"
            });
            Add(416, new CustomStatusCodeResult
            {
                StatusCode = 416,
                Description = "Requested Range Not Satisfiable"
            });
            Add(417, new CustomStatusCodeResult
            {
                StatusCode = 417,
                Description = "Expectation Failed"
            });
            Add(418, new CustomStatusCodeResult
            {
                StatusCode = 418,
                Description = "I'm a teapot",
                Link = new Uri(uriBaseLocation+"/teapot")
            });
            Add(422, new CustomStatusCodeResult
            {
                StatusCode = 422,
                Description = "Unprocessable Entity"
            });
            Add(428, new CustomStatusCodeResult
            {
                StatusCode = 428,
                Description = "Precondition Required"
            });
            Add(429, new CustomStatusCodeResult
            {
                StatusCode = 429,
                Description = "Too Many Requests",
                IncludeHeaders = new Dictionary<string, string>
                {
                    {"Retry-After", "3600"}
                }
            });
            Add(431, new CustomStatusCodeResult
            {
                StatusCode = 431,
                Description = "Request Header Fields Too Large"
            });
            Add(451, new CustomStatusCodeResult
            {
                StatusCode = 451,
                Description = "Unavailable For Legal Reasons"
            });
            Add(499, new CustomStatusCodeResult
            {
                StatusCode = 499,
                Description = "Client Closed Request"
            });
            #endregion
            #region 500region
            Add(500, new CustomStatusCodeResult
            {
                StatusCode = 500,
                Description = "Internal Server Error"
            });
            Add(501, new CustomStatusCodeResult
            {
                StatusCode = 501,
                Description = "Not Implemented"
            });
            Add(502, new CustomStatusCodeResult
            {
                StatusCode = 502,
                Description = "Bad Gateway"
            });
            Add(503, new CustomStatusCodeResult
            {
                StatusCode = 503,
                Description = "Service Unavailable"
            });
            Add(504, new CustomStatusCodeResult
            {
                StatusCode = 504,
                Description = "Gateway Timeout"
            });
            Add(505, new CustomStatusCodeResult
            {
                StatusCode = 505,
                Description = "HTTP Version Not Supported"
            });
            Add(506, new CustomStatusCodeResult
            {
                StatusCode = 506,
                Description = "Variant Also Negotiates"
            });
            Add(507, new CustomStatusCodeResult
            {
                StatusCode = 507,
                Description = "Insufficient Storage"
            });
            Add(508, new CustomStatusCodeResult
            {
                StatusCode = 508,
                Description = "Loop Detected"
            });
            Add(510, new CustomStatusCodeResult
            {
                StatusCode = 510,
                Description = "Not Extended"
            });
            Add(511, new CustomStatusCodeResult
            {
                StatusCode = 511,
                Description = "Network Authentication Required"
            });
            Add(520, new CustomStatusCodeResult
            {
                StatusCode = 520,
                Description = "Web server is returning an unknown error"
            });
            Add(522, new CustomStatusCodeResult
            {
                StatusCode = 522,
                Description = "Connection timed out"
            });
            Add(524, new CustomStatusCodeResult
            {
                StatusCode = 524,
                Description = "A timeout occurred"
            });
            Add(599, new CustomStatusCodeResult
            {
                StatusCode = 599,
                Description = "Network Connect Timeout Error"
            });
            #endregion
        }
    }
}
