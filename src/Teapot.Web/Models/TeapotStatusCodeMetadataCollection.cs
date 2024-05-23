using System;
using System.Collections.Generic;
using Teapot.Web.Models.Unofficial;

namespace Teapot.Web.Models;

public class TeapotStatusCodeMetadataCollection : Dictionary<int, TeapotStatusCodeMetadata>
{
    public TeapotStatusCodeMetadataCollection(
        AmazonStatusCodeMetadata amazon,
        CloudflareStatusCodeMetadata cloudflare,
        EsriStatusCodeMetadata esri,
        LaravelStatusCodeMetadata laravel,
        MicrosoftStatusCodeMetadata microsoft,
        NginxStatusCodeMetadata nginx,
        TwitterStatusCodeMetadata twitter)
    {
        // 1xx range
        Add(100, new TeapotStatusCodeMetadata
        {
            Description = "Continue",
            ExcludeBody = true
        });
        Add(101, new TeapotStatusCodeMetadata
        {
            Description = "Switching Protocols",
            ExcludeBody = true
        });
        Add(102, new TeapotStatusCodeMetadata
        {
            Description = "Processing",
            ExcludeBody = true
        });
        Add(103, new TeapotStatusCodeMetadata
        {
            Description = "Early Hints",
            ExcludeBody = true,
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Link", "</css/main.css>; rel=preload"}
            }
        });

        // 2xx range
        Add(200, new TeapotStatusCodeMetadata
        {
            Description = "OK",
        });
        Add(201, new TeapotStatusCodeMetadata
        {
            Description = "Created",
        });
        Add(202, new TeapotStatusCodeMetadata
        {
            Description = "Accepted"
        });
        Add(203, new TeapotStatusCodeMetadata
        {
            Description = "Non-Authoritative Information"
        });
        Add(204, new TeapotStatusCodeMetadata
        {
            Description = "No Content",
            ExcludeBody = true
        });
        Add(205, new TeapotStatusCodeMetadata
        {
            Description = "Reset Content",
            ExcludeBody = true
        });
        Add(206, new TeapotStatusCodeMetadata
        {
            Description = "Partial Content",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Content-Range", "0-30"}
            }
        });
        Add(207, new TeapotStatusCodeMetadata
        {
            Description = "Multi-Status",
            IncludeHeaders = new Dictionary<string, string>
            {
                { "Content-Type", "application/xml; charset=\"utf-8\"" }
            },
            Link = new Uri("https://tools.ietf.org/html/rfc4918"),
            Body = """
            <?xml version="1.0" encoding="utf-8"?>
            <d:multistatus xmlns:d="DAV:">
            <d:response>
                <d:href>http://www.example.com/container/resource3</d:href>
                <d:status>HTTP/1.1 423 Locked</d:status>
                <d:error><d:lock-token-submitted/></d:error>
            </d:response>
            </d:multistatus>
            """
        });
        Add(208, new TeapotStatusCodeMetadata
        {
            Description = "Already Reported"
        });
        Add(226, new TeapotStatusCodeMetadata
        {
            Description = "IM Used",
            Link = new Uri("https://tools.ietf.org/html/rfc3229#section-10.4.1")
        });

        // 3xx range
        Add(300, new TeapotStatusCodeMetadata
        {
            Description = "Multiple Choices"
        });
        Add(301, new TeapotStatusCodeMetadata
        {
            Description = "Moved Permanently",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Location", "https://httpstat.us"}
            }
        });
        Add(302, new TeapotStatusCodeMetadata
        {
            Description = "Found",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Location", "https://httpstat.us"}
            }
        });
        Add(303, new TeapotStatusCodeMetadata
        {
            Description = "See Other",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Location", "https://httpstat.us"}
            }
        });
        Add(304, new TeapotStatusCodeMetadata
        {
            Description = "Not Modified",
            ExcludeBody = true
        });
        Add(305, new TeapotStatusCodeMetadata
        {
            Description = "Use Proxy",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Location", "https://httpstat.us"}
            }
        });
        Add(306, new TeapotStatusCodeMetadata
        {
            Description = "Switch Proxy"
        });
        Add(307, new TeapotStatusCodeMetadata
        {
            Description = "Temporary Redirect",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Location", "https://httpstat.us"}
            }
        });
        Add(308, new TeapotStatusCodeMetadata
        {
            Description = "Permanent Redirect",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Location", "https://httpstat.us"}
            }
        });

        // 4xx
        Add(400, new TeapotStatusCodeMetadata
        {
            Description = "Bad Request"
        });
        Add(401, new TeapotStatusCodeMetadata
        {
            Description = "Unauthorized",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"WWW-Authenticate", "Basic realm=\"Fake Realm\""}
            }
        });
        Add(402, new TeapotStatusCodeMetadata
        {
            Description = "Payment Required"
        });
        Add(403, new TeapotStatusCodeMetadata
        {
            Description = "Forbidden"
        });
        Add(404, new TeapotStatusCodeMetadata
        {
            Description = "Not Found"
        });
        Add(405, new TeapotStatusCodeMetadata
        {
            Description = "Method Not Allowed"
        });
        Add(406, new TeapotStatusCodeMetadata
        {
            Description = "Not Acceptable"
        });
        Add(407, new TeapotStatusCodeMetadata
        {
            Description = "Proxy Authentication Required",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Proxy-Authenticate", "Basic realm=\"Fake Realm\""}
            }
        });
        Add(408, new TeapotStatusCodeMetadata
        {
            Description = "Request Timeout"
        });
        Add(409, new TeapotStatusCodeMetadata
        {
            Description = "Conflict"
        });
        Add(410, new TeapotStatusCodeMetadata
        {
            Description = "Gone"
        });
        Add(411, new TeapotStatusCodeMetadata
        {
            Description = "Length Required"
        });
        Add(412, new TeapotStatusCodeMetadata
        {
            Description = "Precondition Failed"
        });
        Add(413, new TeapotStatusCodeMetadata
        {
            Description = "Request Entity Too Large"
        });
        Add(414, new TeapotStatusCodeMetadata
        {
            Description = "Request-URI Too Long"
        });
        Add(415, new TeapotStatusCodeMetadata
        {
            Description = "Unsupported Media Type"
        });
        Add(416, new TeapotStatusCodeMetadata
        {
            Description = "Requested Range Not Satisfiable"
        });
        Add(417, new TeapotStatusCodeMetadata
        {
            Description = "Expectation Failed"
        });
        Add(418, new TeapotStatusCodeMetadata
        {
            Description = "I'm a teapot",
            Link = new Uri("https://www.ietf.org/rfc/rfc2324.txt")
        });
        Add(421, new TeapotStatusCodeMetadata
        {
            Description = "Misdirected Request"
        });
        Add(422, new TeapotStatusCodeMetadata
        {
            Description = "Unprocessable Entity"
        });
        Add(423, new TeapotStatusCodeMetadata
        {
            Description = "Locked"
        });
        Add(424, new TeapotStatusCodeMetadata
        {
            Description = "Failed Dependency"
        });
        Add(425, new TeapotStatusCodeMetadata
        {
            Description = "Too Early"
        });
        Add(426, new TeapotStatusCodeMetadata
        {
            Description = "Upgrade Required"
        });
        Add(428, new TeapotStatusCodeMetadata
        {
            Description = "Precondition Required"
        });
        Add(429, new TeapotStatusCodeMetadata
        {
            Description = "Too Many Requests",
            IncludeHeaders = new Dictionary<string, string>
            {
                {"Retry-After", "5"}
            }
        });
        Add(431, new TeapotStatusCodeMetadata
        {
            Description = "Request Header Fields Too Large"
        });
        Add(451, new TeapotStatusCodeMetadata
        {
            Description = "Unavailable For Legal Reasons"
        });

        // 5xx
        Add(500, new TeapotStatusCodeMetadata
        {
            Description = "Internal Server Error"
        });
        Add(501, new TeapotStatusCodeMetadata
        {
            Description = "Not Implemented"
        });
        Add(502, new TeapotStatusCodeMetadata
        {
            Description = "Bad Gateway"
        });
        Add(503, new TeapotStatusCodeMetadata
        {
            Description = "Service Unavailable"
        });
        Add(504, new TeapotStatusCodeMetadata
        {
            Description = "Gateway Timeout"
        });
        Add(505, new TeapotStatusCodeMetadata
        {
            Description = "HTTP Version Not Supported"
        });
        Add(506, new TeapotStatusCodeMetadata
        {
            Description = "Variant Also Negotiates"
        });
        Add(507, new TeapotStatusCodeMetadata
        {
            Description = "Insufficient Storage"
        });
        Add(508, new TeapotStatusCodeMetadata
        {
            Description = "Loop Detected",
            Link = new Uri("https://tools.ietf.org/html/rfc5842")
        });
        Add(510, new TeapotStatusCodeMetadata
        {
            Description = "Not Extended",
            Link = new Uri("https://tools.ietf.org/html/rfc2774")
        });
        Add(511, new TeapotStatusCodeMetadata
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

    private void AddNonStandardStatusCodes(IDictionary<int, TeapotStatusCodeMetadata> codes)
    {
        foreach (var item in codes)
        {
            Add(item.Key, item.Value);
        }
    }
}
