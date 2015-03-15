using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tam.Mvc.Extension
{
    public static class SelectListItemHelper
    {
        public static SelectListItem EmptySelectList(string text, string value = "")
        {
            return new SelectListItem
            {
                Text = text,
                Value = value
            };
        }
    }
}
