using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuentos.Lib.Helpers
{
    public static class ViewHelpers
    {

        public static MvcHtmlString ImageLabel(this HtmlHelper helper, string text, int x, int y,
             string include_content = "", string container_classes = "")
        {
            string html = String.Format("<div class='{0}'>", container_classes);

            //size
            if (x > 0 && y > 0)
            {
                html = String.Concat(html, "<div class='topper-label'>");
                html = String.Concat(html, String.Format("{0} <span class='label label-info'>{1} x {2}</span>", text, x, y));
                html = String.Concat(html, "</div>");
            }

            html = String.Concat(html, MvcHtmlString.Create(include_content));
            html = String.Concat(html, "</div>");
            return MvcHtmlString.Create(html);
        }
    }
}