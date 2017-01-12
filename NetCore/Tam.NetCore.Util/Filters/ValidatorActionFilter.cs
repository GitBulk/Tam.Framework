using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;

namespace Tam.NetCore.Util.Filters
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (string.Equals(context.HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    context.Result = new BadRequestResult();
                }
                else
                {
                    var result = new ContentResult();
                    string content = JsonConvert.SerializeObject(context,
                        new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    result.Content = content;
                    result.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = 400;
                    context.Result = result;
                }
            }
        }
    }
}
