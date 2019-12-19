using FluentValidation;

namespace BulkItemUploadProcessor.Service
{
    internal sealed class ValidateNothingDecorator<T> : AbstractValidator<T>
    {

    }
}