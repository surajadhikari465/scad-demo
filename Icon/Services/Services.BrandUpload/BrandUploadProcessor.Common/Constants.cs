using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace BrandUploadProcessor.Common
{
    public static class Constants
    {
        public const string BrandWorksheetName = "Brands";
        public const string RemoveExcelValue = "REMOVE";
        public const string BrandIdColumnHeader = "Brand ID";
        public const string BrandNameColumnHeader = "Brand Name";
        public const string BrandAbbreviationColumnHeader = "Brand Abbreviation";
        public const string DesignationColumnHeader = "Designation";
        public const string ZipCodeColumnHeader = "Zip Code";
        public const string LocalityColumnHeader = "Locality";
        public const string ParentCompanyColumnHeader = "Parent Company";
        public const int IrmaBrandNameMaxLength = 25;

        public static class ErrorMessages
        {
            public const string RequiredBrandName = "Brand Name is a required field and must be present for all records.";
            public const string RequiredBrandAbbreviation = "Brand Abbreviation is a required field and must be present for all records.";
            public const string RequiredBrandId = "Brand Id is a required field when updating existing brands and must be present for all records.";
            public const string InvalidBrandIdDataType = "Brand Id has an invalid value and should be an integer that represents a HierarchyClassId for an existing brand.";
            public const string InvalidbrandId = "Brand Id has a value that does not match an existing brand.";
            public const string CreateNewBrandIdNotAllowed = "Brand Id must be empty when creating new brands.";
            public const string InvalidRemoveBrandName = "Brand Name cannot have a value of REMOVE.";
            public const string InvalidRemoveBrandAbbreviation = "Brand Abbreviation cannot have a value of REMOVE.";
        }
    }
}
