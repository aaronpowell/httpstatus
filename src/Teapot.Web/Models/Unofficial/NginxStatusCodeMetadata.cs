using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class NginxStatusCodeMetadata : Dictionary<int, TeapotStatusCodeMetadata>
{
    public NginxStatusCodeMetadata()
    {
        Add(444, new TeapotStatusCodeMetadata
        {
            Description = "No Response",
            IsNonStandard = true,
        });

        Add(494, new TeapotStatusCodeMetadata
        {
            Description = "Request header too large",
            IsNonStandard = true,
        });

        Add(495, new TeapotStatusCodeMetadata
        {
            Description = "SSL Certificate Error",
            IsNonStandard = true,
        });

        Add(496, new TeapotStatusCodeMetadata
        {
            Description = "SSL Certificate Required",
            IsNonStandard = true,
        });

        Add(497, new TeapotStatusCodeMetadata
        {
            Description = "HTTP Request Sent to HTTPS Port",
            IsNonStandard = true,
        });

        Add(499, new TeapotStatusCodeMetadata
        {
            Description = "Client Closed Request",
            IsNonStandard = true,
        });
    }
}
