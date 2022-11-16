using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class NginxStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
{
    public NginxStatusCodeResults()
    {
        Add(444, new TeapotStatusCodeResult
        {
            Description = "No Response",
            IsNonStandard = true,
        });

        Add(494, new TeapotStatusCodeResult
        {
            Description = "Request header too large",
            IsNonStandard = true,
        });

        Add(495, new TeapotStatusCodeResult
        {
            Description = "SSL Certificate Error",
            IsNonStandard = true,
        });

        Add(496, new TeapotStatusCodeResult
        {
            Description = "SSL Certificate Required",
            IsNonStandard = true,
        });

        Add(497, new TeapotStatusCodeResult
        {
            Description = "HTTP Request Sent to HTTPS Port",
            IsNonStandard = true,
        });

        Add(499, new TeapotStatusCodeResult
        {
            Description = "Client Closed Request",
            IsNonStandard = true,
        });
    }
}
