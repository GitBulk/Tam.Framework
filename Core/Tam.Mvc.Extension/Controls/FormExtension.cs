using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Tam.Mvc.Extension.Controls
{
    public static class FormExtension
    {
        public static MvcForm BeginFormUpload(this HtmlHelper helper, string actionName, string controllerName)
        {
            return helper.BeginForm(actionName, controllerName, FormMethod.Post, new { enctype = "multipart/form-data" });
        }
    }
}
