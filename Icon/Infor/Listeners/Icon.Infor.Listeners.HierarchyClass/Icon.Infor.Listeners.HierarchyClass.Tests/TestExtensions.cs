using System.Text.RegularExpressions;

namespace Icon.Infor.Listeners.HierarchyClass.Tests
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
