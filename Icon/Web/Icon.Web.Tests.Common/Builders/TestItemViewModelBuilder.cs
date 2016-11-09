using Icon.Web.Mvc.Models;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestItemViewModelBuilder
    {
        private string scanCode;
        private string brandName;
        private int brandHierarchyClassId;
        private string brandLineage;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private bool foodStampEligible;
        private string posScaleTare;
        private string retailSize;
        private string retailUom;
        private string deliverySystem;
        private string merchandiseHierarchyName;
        private int? merchandiseHierarchyClassId;
        private string merchandiseLineage;
        private string taxHierarchyName;
        private int? taxHierarchyClassId;
        private string taxHierarchyLineage;
        private string browsingHierarchyName;
        private int? browsingHierarchyClassId;
        private string browsingLineage;
        private bool isValidated;
        private bool departmentSale;
        private bool hiddenItem;
        private string nationalLineage;
        private int? nationalId;

        public TestItemViewModelBuilder()
        {
            scanCode = "12345";
            brandName = "Test Brand";
            brandHierarchyClassId = 1;
            brandLineage = "Test Brand";
            productDescription = "Test Product Description";
            posDescription = "Test POS Description";
            packageUnit = "2";
            foodStampEligible = false;
            posScaleTare = "1";
            retailSize = "1";
            retailUom = "EA";
            deliverySystem = "CAP";
            merchandiseHierarchyName = "Test Merch";
            merchandiseHierarchyClassId = 2;
            merchandiseLineage = "Test Merch";
            taxHierarchyName = "Test Tax";
            taxHierarchyClassId = 3;
            taxHierarchyLineage = "Test Tax";
            browsingHierarchyName = "Test Browsing";
            browsingHierarchyClassId = 4;
            browsingLineage = "Browsing";
            isValidated = false;
            departmentSale = false;
            hiddenItem = false;
            nationalId = 5;
            nationalLineage = "Class1|5";
        }

        public TestItemViewModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public ItemViewModel Build()
        {
            return new ItemViewModel
            {
                ScanCode = scanCode,
                BrandName = brandName,
                BrandHierarchyClassId = brandHierarchyClassId,
                BrandLineage = brandLineage,
                ProductDescription = productDescription,
                PosDescription = posDescription,
                PackageUnit = packageUnit,
                FoodStampEligible = foodStampEligible,
                PosScaleTare = posScaleTare,
                RetailSize = retailSize,
                RetailUom = retailUom,
                DeliverySystem = deliverySystem,
                MerchandiseHierarchyName = merchandiseHierarchyName,
                MerchandiseHierarchyClassId = merchandiseHierarchyClassId,
                MerchandiseLineage = merchandiseLineage,
                TaxHierarchyClassId = taxHierarchyClassId,
                TaxHierarchyName = taxHierarchyName,
                TaxLineage = taxHierarchyLineage,
                BrowsingHierarchyName = browsingHierarchyName,
                BrowsingHierarchyClassId = browsingHierarchyClassId,
                BrowsingLineage = browsingLineage,
                IsValidated = isValidated,
                DepartmentSale = departmentSale,
                HiddenItem = hiddenItem,
                NationalHierarchyClassId = nationalId,
                NationalLineage = nationalLineage
            };
        }

        public static implicit operator ItemViewModel(TestItemViewModelBuilder builder)
        {
            return builder.Build();
        }
    }
}
