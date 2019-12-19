using System;

namespace Icon.Common.Validators
{
    public class ValueRequiredValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string value)
        {
            if (String.IsNullOrEmpty(value) || value.Trim() == String.Empty)
            {
                return false;
            }

            return true;
        }
    }
}