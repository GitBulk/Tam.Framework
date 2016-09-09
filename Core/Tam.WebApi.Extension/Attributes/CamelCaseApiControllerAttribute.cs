using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;

namespace Tam.WebApi.Extension.Attributes
{
    public class CamelCaseApiControllerAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            var jsonMediaTypeFormatter = controllerSettings.Formatters.OfType<JsonMediaTypeFormatter>().SingleOrDefault();
            if (jsonMediaTypeFormatter != null)
            {
                controllerSettings.Formatters.Remove(jsonMediaTypeFormatter);
                jsonMediaTypeFormatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings =
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                };
                controllerSettings.Formatters.Add(jsonMediaTypeFormatter);
            }
        }
    }

    //[CamelCaseApiController]
    //public class SomeController : ApiController
    //{
    //...
    //}
}