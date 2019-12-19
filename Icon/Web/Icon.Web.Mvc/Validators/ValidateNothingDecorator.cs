using FluentValidation;

namespace Icon.Web.Mvc.Validators
{
    /// <summary>
    /// Adds an unregistered type resolution for objects missing an IValidator.
    /// </summary>
    internal sealed class ValidateNothingDecorator<T> : AbstractValidator<T>
    {

    }
}