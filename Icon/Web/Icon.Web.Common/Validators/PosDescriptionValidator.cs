
namespace Icon.Web.Common.Validators
{
    public class PosDescriptionValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string posDescription)
        {
            if (posDescription == null)
            {
                return false;
            }

            if (posDescription.Length > Constants.PosDescriptionMaxLength)
            {
                return false;
            }

            return true;
        }
    }
}