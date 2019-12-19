using System;

namespace Icon.Services.ItemPublisher.Infrastructure
{
    public static class Extensions
    {
        public static Decimal ToDecimal(this double? doubleValue)
        {
            return doubleValue.HasValue ? (decimal)doubleValue.Value : Decimal.Zero;
        }

        public static Decimal ToDecimal(this int? intValue)
        {
            return intValue.HasValue ? (decimal)intValue.Value : Decimal.Zero;
        }

        public static Decimal ToDecimal(this short? shortValue)
        {
            return shortValue.HasValue ? (decimal)shortValue.Value : Decimal.Zero;
        }

        public static Decimal ToDecimal(this decimal? nullableDecimalValue)
        {
            return nullableDecimalValue.HasValue ? nullableDecimalValue.Value : Decimal.Zero;
        }

        public static Decimal ToDecimal(this string stringValue)
        {
            //this one should fail the message incase of wrong format.. once Bug is fixed we can remove tryparsing.
            //Bug: Not removign 'InProcessBy" by value when marking failed events
            Decimal decimalValue = Decimal.Zero;
            if (!string.IsNullOrEmpty(stringValue))
            {
                Decimal.TryParse(stringValue, out decimalValue);
            }
            return decimalValue;
        }

        public static bool ToBooleanFromYesOrNo(this string stringValue)
        {
            if (stringValue.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}