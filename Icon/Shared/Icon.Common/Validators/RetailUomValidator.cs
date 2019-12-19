using System;
using System.Linq;
using Icon.Framework;

namespace Icon.Common.Validators
{
    public class RetailUomValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string retailUom)
        {
            if (String.IsNullOrEmpty(retailUom))
            {
                return false;
            }

            return UomCodes.ByName.Values.Contains(retailUom);
        }
    }
}