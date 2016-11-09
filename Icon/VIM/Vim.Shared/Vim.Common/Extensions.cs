using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vim.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Convert boolean to a string of "1" or "0"
        /// </summary>
        /// <param name="value">any boolean</param>
        /// <returns>"1" or "0"</returns>
        public static string BoolToString(this bool value)
        {
            return value ? "1" : "0";
        }
    }
}
