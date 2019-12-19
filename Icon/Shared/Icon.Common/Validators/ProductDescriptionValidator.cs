
namespace Icon.Common.Validators
{
    public class ProductDescriptionValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string productDescription)
        {
            if (productDescription == null)
            {
                return false;
            }

            if (productDescription.Length > Constants.ProductDescriptionMaxLength)
            {
                return false;
            }

            return true;
        }
    }
}