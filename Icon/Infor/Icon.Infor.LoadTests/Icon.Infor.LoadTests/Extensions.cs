using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests
{
    internal static class Extensions
    {
        internal static string CreateTableForEmail(this object o)
        {
            var properties = o.GetType().GetProperties().Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute), false));

            StringBuilder builder = new StringBuilder();
            builder.Append("<table>");
            foreach (var property in properties)
            {
                builder.Append("<tr>");
                builder.Append("<td>").Append(property.Name).Append("</td>");
                builder.Append("<td>").Append(property.GetValue(o)).Append("</td>");
                builder.Append("</tr>");
            }
            builder.Append("</table>");

            return builder.ToString();
        }
    }
}
