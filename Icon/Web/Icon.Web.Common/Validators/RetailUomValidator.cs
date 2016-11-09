using Icon.Framework;
using System;
using System.Linq;

namespace Icon.Web.Common.Validators
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