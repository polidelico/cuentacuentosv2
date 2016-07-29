using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuentos.Lib.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum e)
        {
            var attributes = (DisplayAttribute[])e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}