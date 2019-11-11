using Agency.Models;
using Agency.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace Agency.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString SortLink(this HtmlHelper html,
            string linkText,
            string sortExpression,
            string actionName,
            string controllerName,
            RouteValueDictionary routeValues)
        {
            routeValues = routeValues ?? new RouteValueDictionary();
            System.Web.UI.WebControls.SortDirection? sort = null;
            var sortDirectionStr = html.ViewContext.HttpContext.Request["SortDirection"];
            if (!string.IsNullOrEmpty(sortDirectionStr)
                && html.ViewContext.HttpContext.Request["SortExpression"] == sortExpression)
            {
                System.Web.UI.WebControls.SortDirection s;
                if (Enum.TryParse(sortDirectionStr, out s))
                {
                    sort = s;
                }
            }
            routeValues["SortExpression"] = sortExpression;
            routeValues["SortDirection"] = sort.HasValue && sort.Value == SortDirection.Ascending ?
                System.Web.UI.WebControls.SortDirection.Descending :
                System.Web.UI.WebControls.SortDirection.Ascending;
            return html.Partial("SortLink", new SortLinkModel
            {
                ActionName = actionName,
                ControllerName = controllerName,
                SortDirection = sort,
                RouteValues = routeValues,
                LinkText = linkText
            });
        }

        public static bool IsImage(this HtmlHelper html, BinaryFile file)
        {
            if (string.IsNullOrEmpty(file.ContentType))
            {
                return false;
            }
            return file.ContentType.Contains("bmp") ||
                file.ContentType.Contains("jpeg") ||
                file.ContentType.Contains("png");
        }
    }
}