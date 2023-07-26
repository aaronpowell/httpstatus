using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class CloudflareStatusCodeMetadata : Dictionary<int, TeapotStatusCodeMetadata>
{
    public CloudflareStatusCodeMetadata()
    {
        Add(520, new TeapotStatusCodeMetadata
        {
            Description = "Web Server Returned an Unknown Error",
            IsNonStandard = true,
        });
        Add(521, new TeapotStatusCodeMetadata
        {
            Description = "Web Server Is Down",
            IsNonStandard = true,
        });
        Add(522, new TeapotStatusCodeMetadata
        {
            Description = "Connection Timed out",
            IsNonStandard = true,
        });
        Add(523, new TeapotStatusCodeMetadata
        {
            Description = "Origin Is Unreachable",
            IsNonStandard = true,
        });
        Add(524, new TeapotStatusCodeMetadata
        {
            Description = "A Timeout Occurred",
            IsNonStandard = true,
        });
        Add(525, new TeapotStatusCodeMetadata
        {
            Description = "SSL Handshake Failed",
            IsNonStandard = true,
        });
        Add(526, new TeapotStatusCodeMetadata
        {
            Description = "Invalid SSL Certificate",
            IsNonStandard = true,
        });
        Add(527, new TeapotStatusCodeMetadata
        {
            Description = "Railgun Error",
            IsNonStandard = true,
        });
        Add(530, new TeapotStatusCodeMetadata
        {
            Description = "Origin DNS Error",
            IsNonStandard = true,
        });
    }
}
