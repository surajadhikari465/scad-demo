using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Common.Validators
{
    public class SubBrickCodeValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string subBrickCode)
        {
            long subBrickCodeLong = 0;
            if (string.IsNullOrWhiteSpace(subBrickCode))
            {
                return new ObjectValidationResult(false, string.Format("Sub-Brick Code {0} is required for Sub-Bricks.", subBrickCode));
            }
            else if (subBrickCode.Contains(" "))
            {
                return new ObjectValidationResult(false, "Sub-Brick Code cannot contain whitespace.");
            }
            else if (subBrickCode.Length > 9 || subBrickCode.StartsWith("0"))
            {
                return new ObjectValidationResult(false, "Sub-Brick Code must be less than 10 characters and cannot start with a 0.");
            }
            else if (!long.TryParse(subBrickCode, out subBrickCodeLong))
            {
                return new ObjectValidationResult(false, string.Format("Sub-Brick Code must be a number. {0} is not a number.", subBrickCode));
            }
            else
            {
                return true;
            }
        }
    }
}
