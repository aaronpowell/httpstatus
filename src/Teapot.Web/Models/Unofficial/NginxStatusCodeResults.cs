using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial
{
    public class NginxStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
    {
        public NginxStatusCodeResults()
        {
            Add(444, new TeapotStatusCodeResult
            {
                Description = "No Response"
            });

            Add(494, new TeapotStatusCodeResult
            {
                Description = "Request header too large"
            });

            Add(495, new TeapotStatusCodeResult
            {
                Description = "SSL Certificate Error"
            });

            Add(496, new TeapotStatusCodeResult
            {
                Description = "SSL Certificate Required"
            });

            Add(497, new TeapotStatusCodeResult
            {
                Description = "HTTP Request Sent to HTTPS Port"
            });

            Add(499, new TeapotStatusCodeResult
            {
                Description = "Client Closed Request"
            });
        }
    }
}
