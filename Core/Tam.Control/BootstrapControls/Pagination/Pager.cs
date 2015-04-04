using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Tam.Control.BootstrapControls.Pagination
{
    public class Pager
    {
        protected int pageSize;

        /// <summary>
        /// number of total results
        /// </summary>
        protected int totalResult;

        protected PagerOption pageOptions;

        protected RouteValueDictionary routeDictionary;

        protected ViewContext viewContext;

        protected AjaxHelper ajaxHelper;
        protected AjaxOptions ajaxOptions;

        public Pager(PagerOption pagerOptions, RouteValueDictionary routeDictionary,
            ViewContext viewContext, AjaxHelper ajaxHelper = null, AjaxOptions ajaxOptions = null)
        {
            this.pageSize = pagerOptions.PageSize;
            this.pageOptions = pagerOptions;
            this.totalResult = pagerOptions.TotalItems;
            this.routeDictionary = routeDictionary;
            this.viewContext = viewContext;
            this.ajaxHelper = ajaxHelper;
            this.ajaxOptions = ajaxOptions;
        }

        public virtual string BuildLinks()
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("<div class=\"text-{0}\">", this.pageOptions.Alignment.ToString().ToLower()));
            builder.AppendLine(string.Format("<ul class=\"pagination{0}\">", GetPagerSize(this.pageOptions.Size)));
            int currentPage = this.pageOptions.CurentPage;
            int numberOfPage = this.pageOptions.NumberOfPage;

            int totalPage = (int)Math.Ceiling((double)this.pageOptions.TotalItems / (double)this.pageOptions.PageSize);

            if (totalPage == 1)
            {
                builder.AppendLine(GetCurrentPageLink());
            }
            else
            {
                #region first, previous link
                if (this.pageOptions.DisplayFirstLastPage)
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
                if (this.pageOptions.DisplayPreviousNextPage)
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

                if (this.pageOptions.IsShowPages)
                {
                    #region Goto Previous Fragment
                    if (this.pageOptions.Goto)
                    {
                        int previousFragmentPage = currentPage - this.pageOptions.NumberOfPagesLeftSide - 1;
                        if (previousFragmentPage > 0)
                        {
                            builder.AppendLine(GetEnableLink("...", previousFragmentPage));
                        }
                    }
                    #endregion

                    #region left side of current page

                    int numberOfPagesLeftSide = this.pageOptions.NumberOfPagesLeftSide;
                    int numberOfPagesRightSide = this.pageOptions.NumberOfPagesRightSide;
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

                    #region Goto Next Fragment
                    if (this.pageOptions.Goto)
                    {
                        int nextFragmentPage = currentPage + this.pageOptions.NumberOfPagesRightSide + 1;
                        if (nextFragmentPage < totalPage)
                        {
                            builder.AppendLine(GetEnableLink("...", nextFragmentPage));
                        }
                    }
                    #endregion
                }

                #endregion pages

                #region next, last link

                if (this.pageOptions.DisplayPreviousNextPage)
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
                if (this.pageOptions.DisplayFirstLastPage)
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
            builder.AppendLine("</div>");
            return builder.ToString();
        }

        private string GetPagerSize(PagerSize size)
        {
            if (size == PagerSize.Large)
            {
                return " pagination-lg";
            }
            else if (size == PagerSize.Small)
            {
                return " pagination-sm";
            }
            return "";
        }

        private string CreateHtmlLink(int page, string linkText)
        {
            string pageQueryString = this.pageOptions.Page;
            //var currentRouteValue = this.viewContext.RequestContext.RouteData.Values;
            var routeContext = new RouteValueDictionary(this.viewContext.RequestContext.RouteData.Values);
            routeContext.Add(pageQueryString, page);
            if (this.routeDictionary != null && this.routeDictionary.Count > 0)
            {
                foreach (var item in this.routeDictionary)
                {
                    if (routeContext.ContainsKey(item.Key) == false)
                    {
                        routeContext.Add(item.Key, item.Value);
                    }
                }
            }
            VirtualPathData virtualPath = RouteTable.Routes.GetVirtualPathForArea(this.viewContext.RequestContext, routeContext);
            string tempUrl = this.viewContext.Controller.ControllerContext.HttpContext.Request.RawUrl;
            //string url = string.Format("{0}?{1}={2}", tempUrl, pageQueryString, 2);
            string url = (virtualPath == null ? "" : virtualPath.VirtualPath);
            return string.Format("<li><a href=\"{0}\">{1}</a></li>", url, linkText);
        }

        private string CreateAjaxLink(int page, string linkText)
        {
            var routeContext = new RouteValueDictionary(this.viewContext.RequestContext.RouteData.Values);
            string actionName = "";
            if (this.routeDictionary["action"] != null)
            {
                actionName = this.routeDictionary["action"].ToString();
            }
            else
            {
                actionName = routeContext["action"].ToString();
            }
            string pageQueryString = this.pageOptions.Page;

            routeContext.Add(pageQueryString, page);
            string ajaxLink = "";
            if (this.routeDictionary["controller"] == null)
            {
                ajaxLink = this.ajaxHelper.ActionLink(HttpUtility.HtmlDecode(linkText), actionName, routeContext, this.ajaxOptions).ToString();
            }
            else
            {
                string controllerName = this.routeDictionary["controller"].ToString();
                ajaxLink = this.ajaxHelper.ActionLink(HttpUtility.HtmlDecode(linkText), actionName, controllerName, routeContext, this.ajaxOptions).ToString();
            }
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

            return CreateAjaxLink(page, linkText);
        }
    }
}