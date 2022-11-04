using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial
{
    public class EsriStatusCodeResults : Dictionary<int, TeapotStatusCodeResult>
    {
        public EsriStatusCodeResults()
        {
            Add(498, new TeapotStatusCodeResult
            {
                Description = "Invalid Token (Esri)"
            });

            Add(499, new TeapotStatusCodeResult
            {
                Description = "Token Required (Esri)"
            });
        }
    }
}
