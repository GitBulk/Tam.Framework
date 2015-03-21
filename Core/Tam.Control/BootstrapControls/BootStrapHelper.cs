using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Tam.Control.BootstrapControls
{
    public class BootStrapHelper
    {
        public MvcHtmlString Label(string text, LabelStyle type = LabelStyle.Default)
        {
            var builder = new TagBuilder("span");
            builder.MergeAttribute("class", "label label-" + type.ToString().ToLower());
            builder.InnerHtml = text;
            return MvcHtmlString.Create(builder.ToString());
        }

        #region ActionButton

        public MvcHtmlString ActionButton(string text, string actionName, string controllerName, ButtonStyle style = ButtonStyle.Default)
        {
            return ActionButton(text, actionName, controllerName, null, null, style);
        }

        public MvcHtmlString ActionButton(string text, string actionName, string controllerName, object routeValues, ButtonStyle style = ButtonStyle.Default)
        {
            return ActionButton(text, actionName, controllerName, routeValues, null, style);
        }

        public MvcHtmlString ActionButton(string text, string actionName, string controllerName, object routeValues, object htmlAttributes,
            ButtonStyle style = ButtonStyle.Default)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new Exception("Action name is null or empty");
            }
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new Exception("Controller name is null or empty");
            }
            if (text == null)
            {
                text = "";
            }
            var builder = new TagBuilder("a");
            builder.SetInnerText(text);
            if (htmlAttributes != null)
            {
                builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            builder.MergeAttribute("class", "btn btn-" + style.ToString().ToLower());

            // http://stackoverflow.com/questions/5453901/urlhelper-generateurl
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (routeValues != null)
            {
                builder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));
            }
            else
            {
                builder.MergeAttribute("href", urlHelper.Action(actionName, controllerName));
            }
            //var url = UrlHelper.GenerateUrl(null, "action", "controller", null, RouteTable.Routes, HttpContext.Current.Request.RequestContext, false);
            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion

        public MvcHtmlString Button(string text, ButtonStyle style = ButtonStyle.Default)
        {
            return Button(text, "", style, ButtonType.Button);
        }

        #region Button

        public MvcHtmlString Button(string text, string glyphicon, ButtonStyle style = ButtonStyle.Default)
        {
            return Button(text, glyphicon, style, ButtonType.Button);
        }

        public MvcHtmlString SearchButton(string text, ButtonStyle style = ButtonStyle.Default, ButtonType type = ButtonType.Button)
        {
            return Button(text, GlyphIcons.SearchGlyphicon, style, type);
        }

        private MvcHtmlString Button(string text, string glyphicon, ButtonStyle style = ButtonStyle.Default, ButtonType type = ButtonType.Button)
        {
            var builder = new TagBuilder("button");
            builder.MergeAttribute("class", "btn btn-" + style.ToString().ToLower());
            builder.MergeAttribute("type", type.ToString().ToLower());
            if (string.IsNullOrWhiteSpace(glyphicon))
            {
                builder.InnerHtml = text;
            }
            else
            {
                var glyphiconBuilder = new TagBuilder("span");
                glyphiconBuilder.MergeAttribute("class", glyphicon);
                builder.InnerHtml = glyphiconBuilder.ToString() + " " + text;
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion

        public MvcHtmlString Image(string source, string alt = "", ImageStyle type = ImageStyle.Thumbnail)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("class", "img-" + type.ToString().ToLower());
            builder.MergeAttribute("src", source);
            if (string.IsNullOrWhiteSpace(alt))
            {
                builder.MergeAttribute("alt", type.ToString() + " image");
            }
            else
            {
                builder.MergeAttribute("alt", alt);
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        #region Panel

        public MvcHtmlString Panel(string text, string style = "panel panel-default")
        {
            var builder = new TagBuilder("div");
            builder.MergeAttribute("class", style);
            var builderBody = new TagBuilder("div");
            builderBody.MergeAttribute("class", "panel-body");
            builderBody.InnerHtml = text;
            builder.InnerHtml = builderBody.ToString();
            return MvcHtmlString.Create(builder.ToString());
        }

        public MvcHtmlString Panel(string content, string heading, string style = "panel panel-default")
        {
            var panel = new TagBuilder("div");
            panel.MergeAttribute("class", style);
            var builderHeading = new TagBuilder("div");
            if (string.IsNullOrWhiteSpace(heading))
            {
                heading = "Heading";
            }
            builderHeading.MergeAttribute("class", "panel-heading");
            builderHeading.InnerHtml = heading;
            var builderBody = new TagBuilder("div");
            builderBody.MergeAttribute("class", "panel-body");
            builderBody.InnerHtml = content;
            panel.InnerHtml = builderHeading.ToString() + builderBody.ToString();
            return MvcHtmlString.Create(panel.ToString());
        }

        #endregion
    }

}
