using System;
using System.Collections.Generic;

namespace Icon.Logging
{
    public static class Extensions
    {
        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {
            for (int index = 0; !String.IsNullOrEmpty(str) && index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }
    }
}
