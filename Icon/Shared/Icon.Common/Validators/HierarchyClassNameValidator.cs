
namespace Icon.Common.Validators
{
    public class HierarchyClassNameValidator : IObjectValidator<string>
    {
        public ObjectValidationResult Validate(string name)
        {
            if (name == null)
            {
                return false;
            }

            if (name.Length > Constants.IconBrandNameMaxLength)
            {
                return false;
            }

            return true;
        }
    }
}