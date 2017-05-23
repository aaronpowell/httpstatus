using System;
using System.Collections.Generic;

namespace Teapot.Web.Models
{
    public class CustomStatusCodeResult
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> IncludeHeaders { get; set; }
        public bool ExcludeBody { get; set; }
        public Uri Link { get; set; }

        public string data
        {
            get
            {
                return StatusCode.ToString() + " " + Description;
            }
        }
    }
}