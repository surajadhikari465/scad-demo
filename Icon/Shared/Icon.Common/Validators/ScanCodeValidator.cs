using System;
using System.Text.RegularExpressions;

namespace Icon.Common.Validators
{
    public class ScanCodeValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string scanCode)
        {
            // Scan code is a required field.
            if (String.IsNullOrEmpty(scanCode))
            {
                return false;
            }

            // Scan code length must not exceed maximum allowed.
            if (scanCode.Length > Constants.ScanCodeMaxLength)
            {
                return false;
            }

            // Scan code must be numeric.
            if (!Regex.IsMatch(scanCode, "^[1-9][0-9]{0,12}$"))
            {
                return false;
            }

            return true;
        }
    }
}