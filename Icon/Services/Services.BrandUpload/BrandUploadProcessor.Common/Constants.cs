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


    }
}
