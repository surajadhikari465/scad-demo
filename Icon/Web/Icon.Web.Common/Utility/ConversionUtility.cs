namespace Icon.Web.Common.Utility
{
    using System;

    public static class ConversionUtility
    {
        /// <summary>
        /// Converts a string 0 or 1 value to a Boolean value.
        /// </summary>
        /// <param name="value">String 0 or 1.</param>
        /// <returns>Boolean conversion.</returns>
        /// <remarks>
        /// Non-parseable 0's or 1's return false.
        /// </remarks>
        public static bool ToBool(string value)
        {
            int result;

            return int.TryParse(value, out result) && Convert.ToBoolean(result);
        }

        /// <summary>
        /// Converts a string 0 or 1 value to a Boolean value.
        /// </summary>
        /// <param name="value">String 0 or 1.</param>
        /// <returns>Boolean conversion.</returns>
        /// <remarks>
        /// Non-parseable 0's or 1's return false.
        /// </remarks>
        public static bool? ToNullableBool(string value)
        {            
            if(value == "1")
            {
                return true;
            }
            else if(value == "0")
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        public static decimal? ToNullableDecimal(string value)
        {
            decimal result;

            if (decimal.TryParse(value, out result))
                return result;
            else
                return null;
        }

        public static string ToFormattedDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
        }

        /// <summary>
        /// Converts a nullable bool to a bit in string format. Used commonly for 
        /// Item Traits that are in the database.
        /// </summary>
        /// <param name="value">Nullable bool</param>
        /// <returns>1, 0, or null for true, false, or undetermined.</returns>
        public static string ConvertToItemTraitDbValue(bool? value)
        {
            if(value.HasValue)
            {
                if (value.Value)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return null;
            }
        }

        public static string ConvertToItemTraitDbValue(bool value)
        {
            return value ? "1" : "0";
        }

        public static string ConvertToItemTraitDbValue(decimal? value)
        {
            return value?.ToString();
        }

        public static string ConvertYesNoToDatabaseValue(this string value)
        {
            if (value == "Y" || value == "Yes")
            {
                return "1";
            }
            else if(value == "N" || value == "No")
            {
                return "0";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ConvertBitStringToYesNo(string value)
        {
            if (value == "1")
                return "Y";
            else if (value == "0")
                return "N";
            else
                return value;
        }

        public static string ConvertBoolToYesNo(bool value)
        {
            return value ? "Y" : "N";
        }

        public static string ConvertNullableBoolToYesNo(bool? value)
        {
            if (value.HasValue)
            {
                if (value.Value)
                    return "Y";
                else
                    return "N";
            }
            else
            {
                return null;
            }
        }

        public static string ConvertNullableBoolToYesNoFull(bool? value)
        {
            if (value.HasValue)
            {
                if (value.Value)
                    return "Yes";
                else
                    return "No";
            }
            else
            {
                return null;
            }
        }

        public static bool ConvertBitStringToBool(string value)
        {
            if (value == "1")
                return true;
            else
                return false;
        }
    }
}
