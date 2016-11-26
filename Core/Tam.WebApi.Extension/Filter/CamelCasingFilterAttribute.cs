using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace Tam.WebApi.Extension.Filter
{
    public class CamelCasingFilterAttribute : ActionFilterAttribute
    {
        // http://stackoverflow.com/questions/14528779/use-camel-case-serialization-only-for-specific-actions


        private JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
        public CamelCasingFilterAttribute()
        {
            this.formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            ObjectContent content = actionExecutedContext.Response.Content as ObjectContent;
            if (content != null)
            {
                if (content.Formatter is JsonMediaTypeFormatter)
                {
                    actionExecutedContext.Response.Content = new ObjectContent(content.ObjectType, content.Value, this.formatter);
                }
            }
        }
    }
}