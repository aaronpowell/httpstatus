using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class StatusCodeResults : Dictionary<int, StatusCodeResult>
    {
        public StatusCodeResults()
        {
            Add(200, new StatusCodeResult
            {
                Description = "OK",
            });
            Add(201, new StatusCodeResult
            {
                Description = "Created",
            });
            Add(202, new StatusCodeResult
            {
                Description = "Accepted"
            });
            Add(203, new StatusCodeResult
            {
                Description = "Non-Authoritative Information"
            });
            Add(204, new StatusCodeResult
            {
                Description = "No Content",
                ExcludeBody = true
            });
        }
    }
}