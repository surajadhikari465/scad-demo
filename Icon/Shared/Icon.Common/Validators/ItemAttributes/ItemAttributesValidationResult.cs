using System.Collections.Generic;

namespace Icon.Common.Validators.ItemAttributes
{
    public class ItemAttributesValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}