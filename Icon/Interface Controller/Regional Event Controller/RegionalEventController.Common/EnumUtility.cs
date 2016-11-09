
using System;
using System.Collections.Generic;
using System.Linq;

namespace RegionalEventController.Common
{
    public static class EnumUtility
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
