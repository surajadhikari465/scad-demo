using Icon.Common.Models;

namespace Icon.Common.Validators.ItemAttributes
{
    public abstract class AbstractItemAttributesValidator : IItemAttributesValidator
    {
        protected AttributeModel attribute;

        public AbstractItemAttributesValidator(AttributeModel attribute)
        {
            this.attribute = attribute;
        }

        public abstract ItemAttributesValidationResult Validate(string value);
    }
}