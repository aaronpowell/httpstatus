using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial
{
    public class LaravelStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
    {
        public LaravelStatusCodeResults()
        {
            Add(419, new TeapotStatusCodeResult
            {
                Description = "CSRF Token Missong or Expired"
            });
        }
    }
}
