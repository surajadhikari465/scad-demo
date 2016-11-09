using System;
using System.Text.RegularExpressions;

namespace Icon.Web.Common.Validators
{
    public class YesNoValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string value)
        {
            if (value == null)
            {
                return false;
            }

            bool emptyString = String.IsNullOrEmpty(value);

            if (!emptyString && !Regex.IsMatch(value, "^[01]$"))
            {
                return false;
            }

            return true;
        }
    }
}