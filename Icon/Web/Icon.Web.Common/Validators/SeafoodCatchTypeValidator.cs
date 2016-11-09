using Icon.Framework;
using System;
using System.Linq;

namespace Icon.Web.Common.Validators
{
    public class SeafoodCatchTypeValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string value)
        {
            bool emptyString = String.IsNullOrEmpty(value);

            if (!emptyString && value != Constants.ExcelImportRemoveValueKeyword && !SeafoodCatchTypes.Descriptions.AsArray.Contains(value, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}