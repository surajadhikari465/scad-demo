using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public static class PagingConstants
    {
        static PagingConstants()
        {
            DefaultRecentTimeSpan = new TimeSpan(1, 0, 0, 0);
        }

        public const int DefaultPageSize = 20;

        public const int DefaultPage = 1;

        public const int NumberOfQuickLinks = 10;

        public static readonly TimeSpan DefaultRecentTimeSpan;
    }
}
