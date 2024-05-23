﻿using System.Collections.Generic;

namespace Teapot.Web.Models.Unofficial;

public class LaravelStatusCodeMetadata : Dictionary<int, TeapotStatusCodeMetadata>
{
    public LaravelStatusCodeMetadata()
    {
        Add(419, new TeapotStatusCodeMetadata
        {
            Description = "CSRF Token Missing or Expired",
            IsNonStandard = true,
        });
    }
}
