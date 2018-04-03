using Newtonsoft.Json;

namespace Mammoth.Price.Controller
{
    public static class LoggingExtensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
