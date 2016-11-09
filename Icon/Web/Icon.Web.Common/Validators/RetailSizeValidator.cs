using System;
using System.Text.RegularExpressions;

namespace Icon.Web.Common.Validators
{
    public class RetailSizeValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string retailSize)
        {
            if (retailSize == null)
            {
                return false;
            }

            bool emptyString = String.IsNullOrEmpty(retailSize);

            if (emptyString)
            {
                return false;
            }

            // Retail Size should be empty, or contain only numbers and decimal points.
            if (!emptyString && !Regex.IsMatch(retailSize, "^[0-9.]+$"))
            {
                return false;
            }

            // Retail Size must be a valid decimal value.
            decimal convertedRetailSize;
            bool isDecimal = decimal.TryParse(retailSize, out convertedRetailSize);

            if (!isDecimal)
            {
                return false;
            }

            // It must be greater than zero
            if (convertedRetailSize <= 0)
            {
                return false;
            }

            // It must be 5 or less digits to the left of the decimal.
            if (convertedRetailSize > (decimal)Constants.RetailSizeMax)
            {
                return false;
            }

            // And it must contain no more than 4 digits to the right of the decimal point.
            if (convertedRetailSize.ToString().Contains("."))
            {
                if (convertedRetailSize.ToString().Split('.')[1].Length > 4)
                {
                    return false;
                }
            }

            return true;
        }
    }
}