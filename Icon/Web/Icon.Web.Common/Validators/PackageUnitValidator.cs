using System;
using System.Text.RegularExpressions;

namespace Icon.Web.Common.Validators
{
    public class PackageUnitValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string packageUnit)
        {
            if (packageUnit == null)
            {
                return false;
            }

            if (packageUnit == String.Empty)
            {
                return true;
            }

            if (packageUnit.Length > Constants.PackageUnitMaxLength)
            {
                return false;
            }

            // Package Unit should be numeric.
            if (!Regex.IsMatch(packageUnit, "^[0-9]+$"))
            {
                return false;
            }

            return true;
        }
    }
}