using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Helpers
{
    public static class DashboardExtensions
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumItem)
            where TEnum: struct, IComparable, IFormattable, IConvertible
        {
            var selectListItems = from TEnum e in Enum.GetValues(typeof(TEnum))
                                  select new { Id = e, Name = e.ToString() };
            return new SelectList(selectListItems, "Id", "Name", enumItem);
        }
    }
}