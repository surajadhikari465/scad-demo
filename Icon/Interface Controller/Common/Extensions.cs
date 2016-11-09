using System;

namespace InterfaceController.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Parses string into Enum of type T.  If Parsing does not work an argument exception will be thrown.
        /// </summary>
        /// <typeparam name="T">Specified Enum</typeparam>
        /// <param name="enumString">the string that needs to be converted to Enum of type T</param>
        /// <returns>Enum of type T</returns>
        public static TEnum ToEnum<TEnum>(this string enumString) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            TEnum status;
            if (Enum.TryParse<TEnum>(enumString, true, out status))
            {
                return status;
            }
            else
            {
                throw new ArgumentException("String cannot be parsed into the enum");
            }
        }

        public static bool IsTransientError(this Exception ex)
        {
            return SqlAzureRetriableExceptionDetector.ShouldRetryOn(ex);
        }
    }
}
