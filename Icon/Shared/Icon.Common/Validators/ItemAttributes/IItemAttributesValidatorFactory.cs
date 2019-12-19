namespace Icon.Common.Validators.ItemAttributes
{
    public interface IItemAttributesValidatorFactory
    {
        IItemAttributesValidator CreateItemAttributesJsonValidator(string attributeName);
    }
}