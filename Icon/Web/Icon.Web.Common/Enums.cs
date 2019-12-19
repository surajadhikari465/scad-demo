using System;

namespace Icon.Web.Common
{
    public static class Enums
    {
        [Flags]
        public enum WriteAccess
        {
            None = 0,
            Full = 1
        }
    }
}