using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Tam.Control.BootstrapControls.Pagination
{
    public class OnlyPageNumberOptions : PagerOptions
    {

        //public OnlyPageNumberPagination(PagerOptions pagerOptions, RouteValueDictionary routeDictionary,
        //    ViewContext viewContext, AjaxHelper ajaxHelper = null, AjaxOptions ajaxOptions = null)
        //    :base(pagerOptions, routeDictionary, viewContext, ajaxHelper, ajaxOptions)
        //{
        //    this.pageOptions.IsShowPages = true;
        //    this.pageOptions.DisplayFirstPage = false;
        //    this.pageOptions.DisplayLastPage = false;
        //    this.pageOptions.DisplayNextPage = false;
        //    this.pageOptions.DisplayPreviousPage = false;
        //}

        public OnlyPageNumberOptions()
        {
            this.IsShowPages = true;
            this.DisplayFirstPage = false;
            this.DisplayLastPage = false;
            this.DisplayNextPage = false;
            this.DisplayPreviousPage = false;
        }
    }
}
