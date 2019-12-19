namespace Icon.Common.Validators.ItemAttributes
{
    public interface IItemAttributesValidator
    {
        ItemAttributesValidationResult Validate(string value);
    }
}