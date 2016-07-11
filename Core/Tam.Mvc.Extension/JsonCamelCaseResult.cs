using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tam.Mvc.Extension
{
    public class JsonCamelCaseResult : ActionResult
    {
        public object Data { get; set; }
        public string ContentType { get; set; }
        public Encoding ContentEncoding { get; set; }
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        public JsonCamelCaseResult(object data, JsonRequestBehavior jsonRequestBehavior)
        {
            this.Data = data;
            this.JsonRequestBehavior = jsonRequestBehavior;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet
                && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }
            var response = context.HttpContext.Response;
            response.ContentType = (!String.IsNullOrEmpty(ContentType) ? ContentType : "application/json");
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data == null)
            {
                return;
            }
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            response.Write(JsonConvert.SerializeObject(Data, jsonSerializerSettings));
        }
    }
}
