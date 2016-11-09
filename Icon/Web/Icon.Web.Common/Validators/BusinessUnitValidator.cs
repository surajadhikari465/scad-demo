using System;
using System.Text.RegularExpressions;

namespace Icon.Web.Common.Validators
{
    public class BusinessUnitValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string businessUnit)
        {
            if (businessUnit == null) 
            {
                return false;
            }

            bool emptyString = String.IsNullOrEmpty(businessUnit);

            if (!emptyString && businessUnit.Length != Constants.BusinessUnitLength)
            {
                return false;
            }

            if (!emptyString && !Regex.IsMatch(businessUnit, "^[0-9]+$"))
            {
                return false;
            }

            return true;
        }
    }
}