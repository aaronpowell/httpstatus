using System;
using System.Collections.Generic;
using System.Linq;

using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Models;

public class TeapotStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
{
    public TeapotStatusCodeResults(
        AmazonStatusCodeResults amazon,
        CloudflareStatusCodeResults cloudflare,
        EsriStatusCodeResults esri,
        LaravelStatusCodeResults laravel,
        MicrosoftStatusCodeResults microsoft,
        NginxStatusCodeResults nginx,
        TwitterStatusCodeResults twitter)
    {
        // 1xx range
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

        // 2xx range
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
        Add(207, new TeapotStatusCodeResult
        {
            Description = "Multi-Status",
            IncludeHeaders = new Dictionary<string, string>
            {
                { "Content-Type", "application/xml; charset=\"utf-8\"" }
            },
            Link = new Uri("https://tools.ietf.org/html/rfc4918"),
            Body = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<d:multistatus xmlns:d=""DAV:"">
<d:response>
    <d:href>http://www.example.com/container/resource3</d:href>
    <d:status>HTTP/1.1 423 Locked</d:status>
    <d:error><d:lock-token-submitted/></d:error>
</d:response>
</d:multistatus>"
        });
        Add(208, new TeapotStatusCodeResult
        {
            Description = "Already Reported"
        });
        Add(226, new TeapotStatusCodeResult
        {
            Description = "IM Used",
            Link = new Uri("https://tools.ietf.org/html/rfc3229#section-10.4.1")
        });

        // 3xx range
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
            Description = "Switch Proxy"
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

        // 4xx
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
        Add(424, new TeapotStatusCodeResult
        {
            Description = "Failed Dependency"
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
            GetRequestSpecificHeaders = (query, headers) =>
            {
                var responseHeaders = new Dictionary<string, string>();

                int retryAfterSeconds;

                if (query.ContainsKey("seconds") && int.TryParse(query["seconds"], out int seconds))
                    retryAfterSeconds = seconds;
                else if (headers.TryGetValue("X-Retry-After-Seconds", out var retryAfterHeader) && int.TryParse(retryAfterHeader.First(), out seconds))
                    retryAfterSeconds = seconds;
                else
                    retryAfterSeconds = 5;

                string? format;
                if (query.ContainsKey("format"))
                    format = query["format"].First();
                else if (headers.TryGetValue("X-Retry-After-Format", out var formatValues))
                    format = formatValues.First();
                else
                    format = "seconds";

                string retryAfter;

                if("date".Equals(format, StringComparison.InvariantCultureIgnoreCase)) //Date format: https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Date
                    retryAfter = DateTime.UtcNow.AddSeconds(retryAfterSeconds).ToString(@"ddd, dd MMM yyyy HH:mm:ss \G\M\T", System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
                else
                    retryAfter = retryAfterSeconds.ToString();

                responseHeaders.Add("Retry-After", retryAfter);

                return responseHeaders;
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

        // 5xx
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
        Add(507, new TeapotStatusCodeResult
        {
            Description = "Insufficient Storage"
        });
        Add(508, new TeapotStatusCodeResult
        {
            Description = "Loop Detected",
            Link = new Uri("https://tools.ietf.org/html/rfc5842")
        });
        Add(510, new TeapotStatusCodeResult
        {
            Description = "Not Extended",
            Link = new Uri("https://tools.ietf.org/html/rfc2774")
        });
        Add(511, new TeapotStatusCodeResult
        {
            Description = "Network Authentication Required"
        });

        AddNonStandardStatusCodes(amazon);
        AddNonStandardStatusCodes(cloudflare);
        AddNonStandardStatusCodes(esri);
        AddNonStandardStatusCodes(laravel);
        AddNonStandardStatusCodes(microsoft);
        AddNonStandardStatusCodes(nginx);
        AddNonStandardStatusCodes(twitter);
    }

    private void AddNonStandardStatusCodes(IDictionary<int, TeapotStatusCodeResult> codes)
    {
        foreach (var item in codes)
        {
            Add(item.Key, item.Value);
        }
    }
}
