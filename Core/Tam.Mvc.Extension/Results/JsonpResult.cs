using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Tam.Mvc.Extension.Results
{
    public class JsonpResult : JsonResult
    {
        public JsonpResult(object data)
        {
            this.Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            HttpRequestBase request = context.HttpContext.Request;
            string callbackFunction = request.QueryString["callback"];
            if (string.IsNullOrEmpty(callbackFunction))
            {
                throw new Exception("Callback function name must be provided in the request.");
            }
            if (!string.IsNullOrEmpty(ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/javascript";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var serializer = new JavaScriptSerializer();
                string ser = serializer.Serialize(Data);
                response.Write(callbackFunction + "(" + ser + ");");
            }
        }
    }
}
