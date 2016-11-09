using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Tests
{
    public static class TestExtensions
    {
        public static string GetFormattedValidationMessage(this string message, string propertyName, string propertyValue)
        {
            string propertyNameWithSpaces = Regex.Replace(propertyName, "([a-z])([A-Z])", "$1 $2");

            return message
                .Replace("{PropertyName}", propertyNameWithSpaces)
                .Replace("{PropertyValue}", propertyValue);
        }
    }
}
