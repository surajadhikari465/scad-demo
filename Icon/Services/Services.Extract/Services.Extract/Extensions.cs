﻿using System;
using System.Text.RegularExpressions;

namespace Services.Extract
{
    public static class Extensions
    {
        public static string RemoveChar(this string input, string characterToRemove)
        {
            if (input == null) return "";
            return input.Replace(characterToRemove, "");
        }

        public static string BoolToInt(this string input, bool boolToInt = true)
        {
            var lowerValue = input.ToLower();
            if (!boolToInt) return input;
            if (lowerValue.Equals("true")) return "1";
            return lowerValue.Equals("false") ? "0" : input;
        }

        public static string TransformDateTimeStamp(this string input)
        {
            // {date:<format>}
            var regex = new Regex("{date:(.*?)}");


            var dateMatches = regex.Match(input);
            if (dateMatches.Success)
            {
                string dateFormat = string.Empty;
                if (dateMatches.Groups.Count > 1)
                {
                    dateFormat = dateMatches.Groups[1].Value;
                }

                return regex.Replace(input, DateTime.Now.ToString(dateFormat));
            }
            else
            {
                return input;
            }
        }


        public static string TransformSource(this string input, string sourceValue)
        {
            return input.Replace("{source}", sourceValue);
        }

        public static T ConvertToEnum<T>(this string text) where T : struct, IComparable, IFormattable, IConvertible
        {
            T enumSetting = default(T);
            if (Enum.TryParse<T>(text, out enumSetting))
            {
                return enumSetting;
            }
            else
            {
                throw new InvalidOperationException(
                    String.Format("[{0}] is not a valid value for [{1}].  Valid values for [{1}] are : [{2}].",
                        text,
                        typeof(T),
                        String.Join(", ", Enum.GetNames(typeof(T)
                ))));
            }
        }
    }
}