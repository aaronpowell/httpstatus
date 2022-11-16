using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class CloudflareStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
{
    public CloudflareStatusCodeResults()
    {
        Add(520, new TeapotStatusCodeResult
        {
            Description = "Web Server Returned an Unknown Error",
            IsNonStandard = true,
        });
        Add(521, new TeapotStatusCodeResult
        {
            Description = "Web Server Is Down",
            IsNonStandard = true,
        });
        Add(522, new TeapotStatusCodeResult
        {
            Description = "Connection Timed out",
            IsNonStandard = true,
        });
        Add(523, new TeapotStatusCodeResult
        {
            Description = "Origin Is Unreachable",
            IsNonStandard = true,
        });
        Add(524, new TeapotStatusCodeResult
        {
            Description = "A Timeout Occurred",
            IsNonStandard = true,
        });
        Add(525, new TeapotStatusCodeResult
        {
            Description = "SSL Handshake Failed",
            IsNonStandard = true,
        });
        Add(526, new TeapotStatusCodeResult
        {
            Description = "Invalid SSL Certificate",
            IsNonStandard = true,
        });
        Add(527, new TeapotStatusCodeResult
        {
            Description = "Railgun Error",
            IsNonStandard = true,
        });
        Add(530, new TeapotStatusCodeResult
        {
            IsNonStandard = true,
        });
    }
}
