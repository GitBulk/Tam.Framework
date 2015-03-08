using System.Web.Mvc;

namespace Tam.Compression
{
    public class DeflateCompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CompressionManager.GZipEncodePage();
            //base.OnActionExecuting(filterContext);
        }
    }
}