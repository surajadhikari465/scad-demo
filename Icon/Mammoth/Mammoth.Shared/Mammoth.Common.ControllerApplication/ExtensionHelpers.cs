using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mammoth.Common.ControllerApplication
{
    public static class ExtensionHelpers
    {
        public static bool IsJsonString(this string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return false;
            }

            bool isJsonString = false;
            jsonString = jsonString.Trim();

            if (jsonString.StartsWith("{") && jsonString.EndsWith("}")
                || jsonString.StartsWith("[") && jsonString.EndsWith("]"))
            {
                try
                {
                    var obj = JToken.Parse(jsonString);
                    isJsonString = true;
                }
                catch (JsonReaderException)
                {
                    isJsonString = false;
                }
            }
            else
            {
                isJsonString = false;
            }

            return isJsonString;
        }
    }
}
