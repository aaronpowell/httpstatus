using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class AmazonStatusCodeMetadata : Dictionary<int, TeapotStatusCodeMetadata>
{
    public AmazonStatusCodeMetadata()
    {
        Add(460, new TeapotStatusCodeMetadata
        {
            Description = "Client closed the connection with AWS Elastic Load Balancer",
            IsNonStandard = true,
        });

        Add(463, new TeapotStatusCodeMetadata
        {
            Description = "The load balancer received an X-Forwarded-For request header with more than 30 IP addresses",
            IsNonStandard = true,
        });

        Add(561, new TeapotStatusCodeMetadata
        {
            Description = "Unauthorized (AWS Elastic Load Balancer)",
            IsNonStandard = true,
        });
    }
}
