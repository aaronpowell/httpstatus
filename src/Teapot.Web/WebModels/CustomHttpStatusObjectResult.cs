using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teapot.Web.Models;

namespace Teapot.Web.WebModels
{
    public class CustomHttpStatusObjectResult : ObjectResult
    {
        private CustomStatusCodeResult _statusObject;
        public CustomHttpStatusObjectResult(CustomStatusCodeResult custom) : base(custom.data)
        {
            this.StatusCode = custom.StatusCode;
            this._statusObject = custom;
        }
        
        public override Task ExecuteResultAsync(ActionContext context)
        {
            if ((_statusObject.IncludeHeaders != null) && _statusObject.IncludeHeaders.Count > 0)
            {
                foreach(KeyValuePair<string, string> keyvalue in _statusObject.IncludeHeaders)
                    context.HttpContext.Response.Headers.Add(keyvalue.Key, keyvalue.Value);
            }
            if (_statusObject.Link != null)
            {
                Value = string.Format( _statusObject.Link.OriginalString.Replace("http://localhost:58447",
                                                        context.HttpContext.Request.Scheme + "://" + context.HttpContext.Request.Host).ToString()
                                                        + " " + StatusCode.ToString() + " " + _statusObject.Description);
            }
            return base.ExecuteResultAsync(context); 
        }
    }
}
