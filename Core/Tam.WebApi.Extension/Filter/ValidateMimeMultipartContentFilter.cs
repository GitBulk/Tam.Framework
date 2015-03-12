using System.Net;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Net.Http;

namespace Tam.WebApi.Extension.Filter
{
    public class ValidateMimeMultipartContentFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Check the correct MIME type is sent. This can then be used for all uploads methods.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!actionContext.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            base.OnActionExecuting(actionContext);
        }
    }
}