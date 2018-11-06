using System;
using Icon.Framework;
using System.Collections.Generic;

namespace KitBuilder.ESB.Listeners.Item.Service.Constants.ItemValidation
{
    public static class ValidationErrorMessages
    {
        private const string InvalidValue = "{PropertyName} has invalid value '{PropertyValue}'.";
        public const string InvalidRefrigeratedEnum = "was expected to be 'Refrigerated' or 'Shelf Stable'";
        public const string InvalidYesNoExpected = "was expected to be 'Yes' or 'No' (case-insensitive)";


        private static string InvalidProperty(IEnumerable<string> collection, bool isRequired = false)
        {
            if (isRequired)
            {
                return InvalidValue + " {PropertyName} is required and must be one of the following: " + string.Join(",", collection);
            }
            return InvalidValue + " {PropertyName} must be empty or one of the following: " + string.Join(",", collection);
        }

        private static string InvalidLengthOrPattern(int maxLength, string traitRegExPattern)
        {
            var msg = " {PropertyName} " + InvalidLength(maxLength) + " and " + InvalidRegEx(traitRegExPattern);
            return msg;
        }

        private static string InvalidLength(int maxLength)
        {
            var msg = " {PropertyName} cannot be more than " + maxLength + " characters";
            return msg;
        }

        private static string InvalidRegEx(string traitRegExPattern)
        {
            var msg = String.Empty;
            if (traitRegExPattern.Contains(RegularExpressions.BooleanYesNo))
            {
                msg += InvalidYesNoExpected + "-";
            }
            else if (traitRegExPattern.Equals(TraitPatterns.Refrigerated))
            {
                msg += InvalidRefrigeratedEnum + "-";
            }
            msg += " {PropertyName} can contain any character following the regular expression: '" + traitRegExPattern + "'.'";
            return msg;
        }
    }
}

