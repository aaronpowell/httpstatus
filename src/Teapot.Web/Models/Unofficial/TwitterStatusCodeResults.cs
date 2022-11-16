using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class TwitterStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
{
    public TwitterStatusCodeResults()
    {
        Add(420, new TeapotStatusCodeResult
        {
            Description = "Enhance Your Calm",
            IsNonStandard = true,
        });
    }
}