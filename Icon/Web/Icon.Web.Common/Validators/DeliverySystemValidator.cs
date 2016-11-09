using Icon.Framework;
using System;
using System.Linq;

namespace Icon.Web.Common.Validators
{
    public class DeliverySystemValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string deliverySystem)
        {
            if (deliverySystem == null)
            {
                return false;
            }

            if (deliverySystem == String.Empty)
            {
                return true;
            }

            return DeliverySystems.AsDictionary.Values.Contains(deliverySystem);
        }
    }
}