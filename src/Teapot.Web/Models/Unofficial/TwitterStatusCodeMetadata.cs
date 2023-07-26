using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class TwitterStatusCodeMetadata : Dictionary<int, TeapotStatusCodeMetadata>
{
    public TwitterStatusCodeMetadata()
    {
        Add(420, new TeapotStatusCodeMetadata
        {
            Description = "Enhance Your Calm",
            IsNonStandard = true,
        });
    }
}