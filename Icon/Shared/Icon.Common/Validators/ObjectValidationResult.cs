
namespace Icon.Common.Validators
{
    public class ObjectValidationResult
    {
        public bool IsValid { get; set; }
        public string Error { get; set; }

        public ObjectValidationResult() { }

        public ObjectValidationResult(bool isValid, string error)
        {
            IsValid = isValid;
            Error = error;
        }

        public static ObjectValidationResult InvalidResult(string error)
        {
            return new ObjectValidationResult
            {
                IsValid = false,
                Error = error
            };
        }

        public static ObjectValidationResult ValidResult()
        {
            return new ObjectValidationResult
            {
                IsValid = true,
                Error = null
            };
        }

        public static implicit operator bool(ObjectValidationResult validationResult)
        {
            return validationResult.IsValid;
        }

        public static implicit operator ObjectValidationResult(bool isValid)
        {
            return new ObjectValidationResult(isValid, null);
        }
    }
}
