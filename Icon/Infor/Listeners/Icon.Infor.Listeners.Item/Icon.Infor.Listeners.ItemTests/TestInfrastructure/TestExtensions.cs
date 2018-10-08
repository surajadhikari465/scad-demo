using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Tests.TestInfrastructure
{
    public static class TestExtensions
    {
        public static string ToBoolString(this bool b)
        {
            return b ? "1" : "0";
        }

        public static bool ParseBoolString(this string s)
        {
            if (s == "1")
                return true;
            else if (s == "0")
                return false;
            else
                throw new ArgumentException("String is not in the expected format of a boolean message. Must be 1 or 0.", nameof(s));
        }

        public static bool ParseBoolStringAdvanced(this string s) => trueBoolValues.Contains(s);

        public static bool? ParseBoolStringAdvancedNullable(this string s)
        {
            if (trueBoolValues.Contains(s)) return true;
            if (falseBoolValues.Contains(s)) return false;
            return (bool?)null;
        }

        private static HashSet<string> trueBoolValues = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase)
        {
            "1",
            "true",
            "yes",
            "y"
         };

        private static HashSet<string> falseBoolValues = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase)
        {
            "0",
            "false",
            "no",
            "n"
        };
    }
}
