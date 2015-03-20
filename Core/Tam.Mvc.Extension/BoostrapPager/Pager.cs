using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Tam.Mvc.Extension.BoostrapPager
{
    public class Pager
    {
        private int pageSize;

        /// <summary>
        /// number of total results
        /// </summary>
        private int totalResult;

        private PagerOptions pageOptions;

        private RouteValueDictionary routeDictionary;

        private ViewContext viewContext;

        private HtmlHelper htmlHelper;
        private AjaxHelper ajaxHelper;
        private AjaxOptions ajaxOptions;

        public Pager(PagerOptions pagerOptions, RouteValueDictionary routeDictionary,
            ViewContext viewContext, AjaxHelper ajaxHelper = null, AjaxOptions ajaxOptions = null)
        {
            this.pageSize = pagerOptions.PageSize;
            this.pageOptions = pagerOptions;
            this.totalResult = pagerOptions.TotalResult;
            this.routeDictionary = routeDictionary;
            this.viewContext = viewContext;
            this.ajaxHelper = ajaxHelper;
            this.ajaxOptions = ajaxOptions;
        }

        public virtual string BuildLinks()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<nav>");
            builder.AppendLine(string.Format("<ul class=\"pagination{0}\">", GetPagerSize(this.pageOptions.Size)));
            int currentPage = this.pageOptions.CurentPage;
            int numberOfPage = this.pageOptions.NumberOfPage;

            int totalPage = (int)Math.Ceiling((double)this.pageOptions.TotalResult / (double)this.pageOptions.PageSize);

            if (totalPage == 1)
            {
                builder.AppendLine(GetCurrentPageLink());
            }
            else
            {

                #region first, previous link
                if (this.pageOptions.DisplayFirstPage)
                {
                    if (currentPage <= 1)
                    {
                        builder.AppendLine(GetDisableLink(this.pageOptions.TextFirstPage));
                    }
                    else
                    {
                        //builder.AppendLine(GetEnableLink(this.pageOption.TextFirstPage, CreateHtmlLink(1)));
                        builder.AppendLine(GetEnableLink(this.pageOptions.TextFirstPage, 1));
                    }
                }
                if (this.pageOptions.DisplayPreviousPage)
                {
                    if (currentPage > 1)
                    {
                        //builder.AppendLine(GetEnableLink(this.pageOption.TextPreviousPage, CreateHtmlLink(currentPage - 1)));
                        builder.AppendLine(GetEnableLink(this.pageOptions.TextPreviousPage, currentPage - 1));
                    }
                    else
                    {
                        builder.AppendLine(GetDisableLink(this.pageOptions.TextPreviousPage));
                    }
                }

                #endregion first, previous link

                #region pages

                int numberOfPagesLeftSide = this.pageOptions.NumberOfPagesLeftSide;
                int numberOfPagesRightSide = this.pageOptions.NumberOfPagesRightSide;
                if (this.pageOptions.IsShowPages)
                {
                    #region left side of current page

                    int startPageOfLeftSide = currentPage - numberOfPagesLeftSide;
                    if (startPageOfLeftSide <= 0)
                    {
                        startPageOfLeftSide = 1;
                    }
                    for (int i = startPageOfLeftSide; i < currentPage; i++)
                    {
                        //builder.AppendLine(GetEnableLink(i.ToString(), CreateHtmlLink(i)));
                        builder.AppendLine(GetEnableLink(i.ToString(), i));
                    }

                    #endregion left side of current page

                    #region current page

                    builder.AppendLine(GetCurrentPageLink());

                    #endregion current page

                    #region right side of current page

                    int endPageOfRightSide = currentPage + numberOfPagesRightSide;
                    //if (startPageOfRightSide > this.pageOption.NumberOfPage)
                    //{
                    //    startPageOfRightSide = this.pageOption.NumberOfPage;
                    //}

                    if (endPageOfRightSide > totalPage)
                    {
                        endPageOfRightSide = totalPage;
                    }
                    for (int i = currentPage + 1; i <= endPageOfRightSide; i++)
                    {
                        //builder.AppendLine(GetEnableLink(i.ToString(), CreateHtmlLink(i)));
                        builder.AppendLine(GetEnableLink(i.ToString(), i));
                    }

                    #endregion right side of current page
                }

                #endregion pages

                #region next, last link

                if (this.pageOptions.DisplayNextPage)
                {
                    if (currentPage < totalPage)
                    {
                        builder.AppendLine(GetEnableLink(this.pageOptions.TextNextPage, currentPage + 1));
                    }
                    else
                    {
                        builder.AppendLine(GetDisableLink(this.pageOptions.TextNextPage));
                    }
                }
                if (this.pageOptions.DisplayLastPage)
                {
                    if (currentPage < totalPage)
                    {
                        builder.AppendLine(GetEnableLink(this.pageOptions.TextLastPage, totalPage));
                    }
                    else
                    {
                        builder.AppendLine(GetDisableLink(this.pageOptions.TextLastPage));
                    }
                }

                #endregion next, last link
            }
            builder.AppendLine("</ul>");
            builder.AppendLine("</nav>");
            return builder.ToString();
        }

        public virtual string GetPagerSize(Size size)
        {
            if (size == Size.Large)
            {
                return " pagination-lg";
            }
            else if (size == Size.Small)
            {
                return " pagination-sm";
            }
            return "";
        }

        private string CreateHtmlLink(int page, string linkText)
        {
            string actionName = this.routeDictionary["action"].ToString();
            string controllerName = this.routeDictionary["controller"].ToString();
            string pageQueryString = this.pageOptions.Page;
            var routeCollection = new RouteValueDictionary(this.viewContext.RequestContext.RouteData.Values);
            routeCollection.Add(pageQueryString, page);
            VirtualPathData virtualPath = RouteTable.Routes.GetVirtualPath(this.viewContext.RequestContext, routeCollection);
            //string result;
            //var queryString = this.viewContext.RequestContext.HttpContext.Request.QueryString;
            //if (queryString.Count > 0)
            //{
            //    foreach (string key in queryString.AllKeys)
            //    {
            //        if (string.Equals(key, pageQueryString, StringComparison.OrdinalIgnoreCase))
            //        {
            //            routeCollection[pageQueryString] = queryString[key];
            //        }
            //    }
            //}
            string tempUrl = this.viewContext.Controller.ControllerContext.HttpContext.Request.RawUrl;

            //string url = string.Format("{0}?{1}={2}", tempUrl, pageQueryString, 2);
            string ajaxLink = this.ajaxHelper.ActionLink(page.ToString(), actionName, routeCollection, this.ajaxOptions).ToHtmlString();
            string url = virtualPath.VirtualPath;
            return string.Format("<a href=\"{0}\">{1}</a>", url, linkText);
        }

        private string CraeteAjaxLink(int page, string linkText)
        {
            string actionName = this.routeDictionary["action"].ToString();
            string pageQueryString = this.pageOptions.Page;
            var routeCollection = new RouteValueDictionary(this.viewContext.RequestContext.RouteData.Values);
            routeCollection.Add(pageQueryString, page);
            string ajaxLink = this.ajaxHelper.ActionLink(HttpUtility.HtmlDecode(linkText), actionName, routeCollection, this.ajaxOptions).ToString();
            return string.Format("<li>{0}</li>", ajaxLink);
        }

        private string GetCurrentPageLink()
        {
            //return string.Format("<li class=\"active\"><a href=\"#\">{0}</a></li>", this.pageOption.CurentPage);
            return string.Format("<li class=\"active\"><span>{0}</span></li>", this.pageOptions.CurentPage);
        }

        private string GetDisableLink(string textLink)
        {
            //return string.Format("<li class=\"disabled\"><a href=\"#\"><span>{0}</span></a></li>", textLink);
            //return string.Format("<li class=\"disabled\"><a href=\"#\">{0}</a></li>", textLink);
            return string.Format("<li class=\"disabled\"><span>{0}</span></li>", textLink);
        }

        private string GetEnableLink(string linkText, int page)
        {
            // GetEnableLink(this.pageOption.TextPreviousPage, CreateHtmlLink(currentPage - 1))
            if (this.ajaxHelper == null)
            {
                return CreateHtmlLink(page, linkText);
            }

            return CraeteAjaxLink(page, linkText);
        }
    }
}