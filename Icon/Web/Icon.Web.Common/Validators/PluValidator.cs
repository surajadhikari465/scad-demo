using System;
using System.Text.RegularExpressions;

namespace Icon.Web.Common.Validators
{
    public class PluValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string plu)
        {
            if (plu == null)
            {
                return false;
            }

            if (plu.Length != Constants.ScalePluLength && plu.Length > Constants.PosPluMaxLength)
            {
                return false;
            }

            if (!(plu == String.Empty) && !Regex.IsMatch(plu, "^[0-9]+$"))
            {
                return false;
            }

            return true;
        }
    }
}