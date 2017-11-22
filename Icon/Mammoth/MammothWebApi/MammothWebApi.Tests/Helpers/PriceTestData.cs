using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Tests.Helpers
{
    internal static class PriceTestData
    {
        internal static class Valid
        {
            public const int FiveDigitBusinessUnit = 12345;
            public const string BananasScanCode = "4011";
            public const string ThirteenDigitScanCode = "2061424933821";
            public static readonly DateTime DateIn2018April = new DateTime(2018, 4, 24, 20, 20, 20);
        }

        internal static class Bad
        {
            public const int NegativeBusinessUnit = -1;
            public const string FourteenDigitScanCode = "34567890123456";
            public static readonly DateTime DateIn2009June = new DateTime(2009, 6, 21, 8, 45, 0);
        }
    }
}
