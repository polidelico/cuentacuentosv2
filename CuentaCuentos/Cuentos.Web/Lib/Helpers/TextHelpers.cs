using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Cuentos.Lib.Helpers
{
    public static class TextHelpers
    {
        public enum TitleCase
        {
            First,
            All
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        private static CultureInfo ci = new CultureInfo("en-US");
        //Convert all first latter
        public static string ToTitleCase(this string str)
        {
            str = str.ToLower();
            var strArray = str.Split(' ');
            if (strArray.Length > 1)
            {
                strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                return string.Join(" ", strArray);
            }
            return ci.TextInfo.ToTitleCase(str);
        }

        public static string ToTitleCase(this string str, TitleCase tcase)
        {
            str = str.ToLower();
            switch (tcase)
            {
                case TitleCase.First:
                    var strArray = str.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;
                case TitleCase.All:
                    return ci.TextInfo.ToTitleCase(str);
                default:
                    break;
            }
            return ci.TextInfo.ToTitleCase(str);
        }

        public static string Truncate(string yourString, int maxLength)
        {
            // If the string isn't null or empty
            if (!String.IsNullOrEmpty(yourString))
            {
                // Return the appropriate string size
                return (yourString.Length <= maxLength) ? yourString : yourString.Substring(0, maxLength) + "...";
            }
            else
            {
                // Otherwise return the empty string
                return "";
            }
        }

        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}