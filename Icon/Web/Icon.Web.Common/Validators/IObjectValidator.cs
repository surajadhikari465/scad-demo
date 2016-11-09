
namespace Icon.Web.Common.Validators
{
    public interface IObjectValidator<T> where T: class
    {
        ObjectValidationResult Validate(T t);
    }
}
