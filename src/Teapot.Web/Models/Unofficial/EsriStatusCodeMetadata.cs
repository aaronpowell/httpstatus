using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class EsriStatusCodeMetadata : Dictionary<int, TeapotStatusCodeMetadata>
{
    public EsriStatusCodeMetadata()
    {
        Add(498, new TeapotStatusCodeMetadata
        {
            Description = "Invalid Token (Esri)",
            IsNonStandard = true,
        });
    }
}
