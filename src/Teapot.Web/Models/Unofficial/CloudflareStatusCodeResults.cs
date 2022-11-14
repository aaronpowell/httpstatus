using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class CloudflareStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
{
    public CloudflareStatusCodeResults()
    {
        Add(520, new TeapotStatusCodeResult
        {
            Description = "Web Server Returned an Unknown Error"
        });
        Add(521, new TeapotStatusCodeResult
        {
            Description = "Web Server Is Down"
        });
        Add(522, new TeapotStatusCodeResult
        {
            Description = "Connection Timed out"
        });
        Add(523, new TeapotStatusCodeResult
        {
            Description = "Origin Is Unreachable"
        });
        Add(524, new TeapotStatusCodeResult
        {
            Description = "A Timeout Occurred"
        });
        Add(525, new TeapotStatusCodeResult
        {
            Description = "SSL Handshake Failed"
        });
        Add(526, new TeapotStatusCodeResult
        {
            Description = "Invalid SSL Certificate"
        });
        Add(527, new TeapotStatusCodeResult
        {
            Description = "Railgun Error"
        });
        Add(530, new TeapotStatusCodeResult
        {
            Description = ""
        });
    }
}
