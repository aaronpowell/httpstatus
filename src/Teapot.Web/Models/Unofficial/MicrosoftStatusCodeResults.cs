using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial
{
    public class MicrosoftStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
    {
        public MicrosoftStatusCodeResults()
        {
            Add(440, new TeapotStatusCodeResult
            {
                Description = "Login Time-out"
            });

            Add(449, new TeapotStatusCodeResult
            {
                Description = "Retry With"
            });

            Add(450, new TeapotStatusCodeResult
            {
                Description = "Blocked by Windows Parental Controls"
            });

            Add(451, new TeapotStatusCodeResult
            {
                Description = "Redirect"
            });
        }
    }
}
