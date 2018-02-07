using Newtonsoft.Json;

namespace Mammoth.PrimeAffinity.Library
{
    internal static class Extensions
    {
        internal static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
