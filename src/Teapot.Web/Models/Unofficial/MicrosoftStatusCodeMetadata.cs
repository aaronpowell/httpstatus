using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class MicrosoftStatusCodeMetadata : Dictionary<int, TeapotStatusCodeMetadata>
{
    public MicrosoftStatusCodeMetadata()
    {
        Add(440, new TeapotStatusCodeMetadata
        {
            Description = "Login Time-out",
            IsNonStandard = true,
        });

        Add(449, new TeapotStatusCodeMetadata
        {
            Description = "Retry With",
            IsNonStandard = true,
        });

        Add(450, new TeapotStatusCodeMetadata
        {
            Description = "Blocked by Windows Parental Controls",
            IsNonStandard = true,
        });
    }
}
