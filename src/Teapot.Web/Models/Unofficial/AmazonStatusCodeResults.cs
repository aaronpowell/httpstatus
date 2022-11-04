using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial
{
    public class AmazonStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
    {
        public AmazonStatusCodeResults()
        {
            Add(460, new TeapotStatusCodeResult
            {
                Description = "Client closed the connection with AWS Elastic Load Balancer"
            });

            Add(463, new TeapotStatusCodeResult
            {
                Description = "The load balancer received an X-Forwarded-For request header with more than 30 IP addresses"
            });

            Add(561, new TeapotStatusCodeResult
            {
                Description = "Unauthorized (AWS Elastic Load Balancer)"
            });
        }
    }
}
