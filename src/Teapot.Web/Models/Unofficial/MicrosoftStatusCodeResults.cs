using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class MicrosoftStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
{
    public MicrosoftStatusCodeResults()
    {
        Add(440, new TeapotStatusCodeResult
        {
            Description = "Login Time-out",
            IsNonStandard = true,
        });

        Add(449, new TeapotStatusCodeResult
        {
            Description = "Retry With",
            IsNonStandard = true,
        });

        Add(450, new TeapotStatusCodeResult
        {
            Description = "Blocked by Windows Parental Controls",
            IsNonStandard = true,
        });
    }
}
