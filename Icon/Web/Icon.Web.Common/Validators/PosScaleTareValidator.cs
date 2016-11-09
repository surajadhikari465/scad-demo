using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Icon.Web.Common.Validators
{
    public class PosScaleTareValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string posScaleTare)
        {
            if (posScaleTare == null)
            {
                return false;
            }

            bool emptyString = String.IsNullOrEmpty(posScaleTare);

            // POS Scale Tare should be empty, or contain only numbers and decimal points.
            if (!emptyString && !Regex.IsMatch(posScaleTare, "^[0-9.]+$"))
            {
                return false;
            }

            // POS Scale Tare must be a valid decimal value.
            decimal conversion;
            bool isDecimal = decimal.TryParse(posScaleTare, NumberStyles.Float, CultureInfo.InvariantCulture, out conversion);

            if (!emptyString && !isDecimal)
            {
                return false;
            }

            if (!emptyString && conversion >= Constants.PosScaleTareMax)
            {
                return false;
            }

            // And it must contain no more than 4 digits plus the decimal point.
            if (!emptyString && conversion.ToString().Length > Constants.PosScaleTareMaxLength)
            {
                return false;
            }

            return true;
        }
    }
}