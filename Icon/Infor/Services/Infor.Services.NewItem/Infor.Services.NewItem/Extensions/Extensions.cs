using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Extensions
{
    public static class Extensions
    {
        public static string ToTaxCode(this string taxName)
        {
            return taxName.Substring(0, 7);
        }

        public static string ToMessageBoolString(this bool value)
        {
            return value ? "0" : "1";
        }
    }
}
