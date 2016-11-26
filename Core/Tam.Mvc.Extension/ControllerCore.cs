using System.Net;
using System.Text;
using System.Web.Mvc;
using Tam.Mvc.Extension.Results;

namespace Tam.Mvc.Extension
{
    public class ControllerCore : Controller
    {
        protected ViewResult PageBadRequest()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View("Error");
        }

        protected ContentResult ContentError(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Content(message);
        }

        protected JsonpResult Jsonp(object data)
        {
            return Jsonp(data, null);
        }

        protected JsonpResult Jsonp(object data, string contentType)
        {
            return Jsonp(data, contentType, null);
        }

        protected JsonpResult Jsonp(object data, string contentType, Encoding contentEncoding)
        {
            return new JsonpResult(data)
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

        protected JsonResult JsonError(object toSerialize)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(toSerialize);
        }

        protected JsonResult JsonError(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { ErrorMessage = message });
        }

        /// <summary>
        /// Answers a null object w/ content type as "application/json" and a response code 404.
        /// </summary>
        protected JsonResult JsonNotFound()
        {
            return JsonNotFound(null);
        }
        protected JsonResult JsonNotFound(object toSerialize)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(toSerialize);
        }

        /// <summary>
        /// Answers the string "404" with response code 404.
        /// </summary>
        protected ContentResult TextPlainNotFound()
        {
            return TextPlainNotFound("404");
        }

        protected ContentResult TextPlainNotFound(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return TextPlain(message);
        }

        /// <summary>
        /// returns ContentResult with the parameter 'content' as its payload and "text/plain" as media type.
        /// </summary>
        protected ContentResult TextPlain(object content)
        {
            return new ContentResult { Content = content.ToString(), ContentType = "text/plain" };
        }
    }
}
