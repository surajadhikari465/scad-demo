using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using BulkItemUploadProcessor.Service.Validation.Interfaces;
using Icon.Framework;
using System;
using static BulkItemUploadProcessor.Common.Constants;

namespace BulkItemUploadProcessor.Service.Validation
{
    public class HierarchyValidator : IHierarchyValidator
    {
        private readonly IHierarchyCache hierarchyCache;

        public HierarchyValidator(IHierarchyCache hierarchyCache)
        {
            this.hierarchyCache = hierarchyCache;
        }

        public ValidationResponse Validate(string hierarchyName, string value, bool allowEmptyString = false, bool allowRemove = false)
        {
            try
            {
                if (allowEmptyString && value == string.Empty)
                    return new ValidationResponse { IsValid = true };
                else if (allowRemove && hierarchyName == ManufacturerHierarchyName && value == RemoveExcelValue)
                    return new ValidationResponse { IsValid = true };
                else
                {
                    var parsedValue = int.Parse(value.ParseClassId());

                    switch (hierarchyName)
                    {
                        case HierarchyNames.Merchandise:
                            if (hierarchyCache.IsValidMerchandiseHierarchyClassId(parsedValue))
                                return new ValidationResponse { IsValid = true };
                            else
                                return new ValidationResponse { IsValid = false, Error = BuildInvalidHierarchyError(hierarchyName, parsedValue) };
                        case HierarchyNames.Brands:
                            if (hierarchyCache.IsValidBrandHierarchyClassId(parsedValue))
                                return new ValidationResponse { IsValid = true };
                            else
                                return new ValidationResponse { IsValid = false, Error = BuildInvalidHierarchyError(hierarchyName, parsedValue) };
                        case HierarchyNames.Tax:
                            if (hierarchyCache.IsValidTaxHierarchyClassId(parsedValue))
                                return new ValidationResponse { IsValid = true };
                            else
                                return new ValidationResponse { IsValid = false, Error = BuildInvalidHierarchyError(hierarchyName, parsedValue) };
                        case HierarchyNames.National:
                            if (hierarchyCache.IsValidNationalHierarchyClassId(parsedValue))
                                return new ValidationResponse { IsValid = true };
                            else
                                return new ValidationResponse { IsValid = false, Error = BuildInvalidHierarchyError(hierarchyName, parsedValue) };
                        case ManufacturerHierarchyName:
                            if (hierarchyCache.IsValidManufacturerHierarchyClassId(parsedValue))
                                return new ValidationResponse { IsValid = true };
                            else
                                return new ValidationResponse { IsValid = false, Error = BuildInvalidHierarchyError(hierarchyName, parsedValue) };
                        default: throw new InvalidOperationException($"Unable to validate hierarchy {hierarchyName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ValidationResponse { IsValid = false, Error = $"Unexpected error occurred while validating '{hierarchyName}' value. '{hierarchyName}' must have a valid integer ID at end of the name." };
            }
        }

        private string BuildInvalidHierarchyError(string hierarchyName, int value)
        {
            return $"Invalid '{hierarchyName}' value: '{value}'. '{value}' does not exist under '{hierarchyName}'.";
        }
    }
}
