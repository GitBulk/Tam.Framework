using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Tam.NetCore.Filters.Validation
{
    public class ValidationModelFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new ContentResult
                {
                    Content = "Model is invalid"
                };
            }
            base.OnActionExecuting(context);
        }
    }
}
