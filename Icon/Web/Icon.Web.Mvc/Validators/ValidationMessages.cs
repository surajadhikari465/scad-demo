using Icon.Common;

namespace Icon.Web.Mvc.Validators
{
    public static class ValidationMessages
    {
        public const string TraitCodeUnique = "An attribute already exists with the same trait code. Trait codes must be unique.";
        public const string XmlTraitDescriptionUnique = "Entered display name cannot be used. It is utilized already by downstream systems.";
        public const string DisplayNameUnique = "An attribute already exists with the same display name. Display names must be unique.";
        public const string ValidNumberOfDigits = "Must have enough decimals for the specified maximum number value.";
        public const string SelectCharacterSets = "At least one option for allowable characters must be selected.";
        public const string PickListCollectionIsEmpty = "At least one pick list value must be entered.";
        public const string PickListValueIsEmpty = "Pick list values cannot be empty.";
        public const string PickListValueMaxLengthAllowed = "Pick list values cannot have a length greater than the maximum length allowed.";
        public const string PickListInvalidValue = "Invalid pick list values. Pick list values must be within allowable characters and not be more than 50 characters long.";
        public const string MinimumNumberValidDecimal = "Minimum Number must be a valid decimal.";
        public const string MaximumNumberValidDecimal = "Maximum Number must be a valid decimal.";
        public const string NumberOfDecimalsValidInteger = "Number of Decimals must be a valid integer.";
        public const string MaximumNumberGreaterThanMinimumNumber = "Maximum Number must be greater than Minimum Number.";
        public static string NumberOfDecimalsInclusiveBetween = $"Number of Decimals must be between {Constants.NumberOfDecimalsMin} and {Constants.NumberOfDecimalsMax}.";
        public static string MinimumNumberInclusiveBetween = $"Minimum Number must be between {Constants.MinimumNumberMin} and {Constants.MinimumNumberMax}.";
        public static string MaximumNumberInclusiveBetween = $"Maximum Number must be between {Constants.MaximumNumberMin} and {Constants.MaximumNumberMax}.";
        public const string CurrentMinimumNumberMustBeLessThanNewMinimumNumber = "Minimum Number must be less than or equal to the current Minimum Number.";
        public const string CurrentMaximumNumberIsGreaterThanNewMaximumNumber = "Maximum Number must be greater than or equal to the current Maximum Number.";
        public const string CurrentNumberOfDecimalsGreaterThanNewNumberOfDecimals = "Number of Decimals must be greater than or equal to the current Number of Decimals.";
        public const string CurrentMaxLengthLessThanNewMaxLength = "Max Length must be greater than or equal to the current Max Length.";
        public const string ValidatePickListValue = "Pick list value is associated with one or more items. Cannot delete.";
        public const string PickListMustHaveOneNonDeletedPickListValue = "Cannot delete pick list value. At least one pick list value must be entered.";
        public const string AttributeExistsOnItemsWhenSwitchingToPickListError = "Unable to switch to a PickList. Items exist that have values for this attribute.";
        public const string InvalidDefaultValue = "Invalid Default Value";
        public const string InvalidDefaultValueForDateAttribute = "Default date value format must be in the form yyyy-MM-dd and be between 2000-01-01 and 2100-12-31.";
        public const string ValidateItemCountOnAttribute = "Attribute is associated with one or more items. Cannot hide.";
        public const string ValidateIsRequiredAttribute = "Attribute is required. Cannot hide.";
    }
}