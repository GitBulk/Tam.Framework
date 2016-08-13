using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tam.Mvc.Extension.Results;

namespace Tam.Mvc.Extension
{
    public class TamController : Controller
    {
        public ViewResult PageBadRequest()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View("Error");
        }

        public ContentResult ContentError(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Content(message);
        }

        public JsonpResult Jsonp(object data)
        {
            return Jsonp(data, null);
        }

        public JsonpResult Jsonp(object data, string contentType)
        {
            return Jsonp(data, contentType, null);
        }

        public JsonpResult Jsonp(object data, string contentType, Encoding contentEncoding)
        {
            return new JsonpResult(data)
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

        public JsonResult JsonError(object toSerialize)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(toSerialize);
        }

        public JsonResult JsonError(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { ErrorMessage = message });
        }

        /// <summary>
        /// Answers a null object w/ content type as "application/json" and a response code 404.
        /// </summary>
        public JsonResult JsonNotFound()
        {
            return JsonNotFound(null);
        }

        public JsonResult JsonNotFound(object toSerialize)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return Json(toSerialize);
        }

        /// <summary>
        /// Answers the string "404" with response code 404.
        /// </summary>
        public ContentResult TextPlainNotFound()
        {
            return TextPlainNotFound("404");
        }

        public ContentResult TextPlainNotFound(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return TextPlain(message);
        }


        /// <summary>
        /// returns ContentResult with the parameter 'content' as its payload and "text/plain" as media type.
        /// </summary>
        public ContentResult TextPlain(object content)
        {
            return new ContentResult { Content = content.ToString(), ContentType = "text/plain" };
        }
    }
}
