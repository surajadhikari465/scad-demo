using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Testing.Common
{
    public class TestIconItemLastChangeModelBuilder
    {
        private int itemId;
        private string validationDate;
        private string scanCode;
        private string scanCodeType;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string foodStamp;
        private string tare;
        private int brandId;
        private string brandName;
        private string taxClassName;
        private bool areNutriFactsUpdated;
        private int subTeamNo;
        private bool subTeamNotAligned;
        private string retailUom;
        private decimal retailSize;

        public TestIconItemLastChangeModelBuilder()
        {
            this.itemId = 1;
            this.validationDate = DateTime.Now.ToString();
            this.scanCode = "99881122771";
            this.scanCodeType = "UPC";
            this.productDescription = "Validated Item Test";
            this.posDescription = "Validated Item Test";
            this.packageUnit = "1";
            this.foodStamp = "1";
            this.tare = "0";
            this.brandId = 1;
            this.brandName = "Validated Test Brand";
            this.taxClassName = "0000033 TEST TAX CLASS";
            this.retailUom = "EA";
            this.retailSize = 2.31M;
        }

        public TestIconItemLastChangeModelBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithValidationDate(string validationDate)
        {
            this.validationDate = validationDate;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithScanCodeType(string scanCodeType)
        {
            this.scanCodeType = scanCodeType;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithProductDesccription(string productDescription)
        {
            this.productDescription = productDescription;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithFoodStamp(string foodStamp)
        {
            this.foodStamp = foodStamp;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithTare(string tare)
        {
            this.tare = tare;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithBrandId(int brandId)
        {
            this.brandId = brandId;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithBrandName(string brandName)
        {
            this.brandName = brandName;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithTaxClass(string taxClass)
        {
            this.taxClassName = taxClass;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithAreNutriFactChanged(bool areNutriFactsUpdated)
        {
            this.areNutriFactsUpdated = areNutriFactsUpdated;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithSubTeamNo(int subTeamNo)
        {
            this.subTeamNo = subTeamNo;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithSubTeamNotAligned(bool subTeamNotAligned)
        {
            this.subTeamNotAligned = subTeamNotAligned;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }

        public TestIconItemLastChangeModelBuilder WithRetailSize(decimal retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public IconItemLastChangeModel Build()
        {
            IconItemLastChangeModel iconItemLastChangeItem = new IconItemLastChangeModel
            {
                ItemId = this.itemId,
                ValidationDate = this.validationDate,
                ScanCode = this.scanCode,
                ScanCodeType = this.scanCodeType,
                ProductDescription = this.productDescription,
                PosDescription = this.posDescription,
                PackageUnit = this.packageUnit,
                FoodStampEligible = this.foodStamp,
                Tare = this.tare,
                BrandId = this.brandId,
                BrandName = this.brandName,
                TaxClassName = this.taxClassName,
                AreNutriFactsUpdated = this.areNutriFactsUpdated,
                SubTeamNo = this.subTeamNo,
                SubTeamNotAligned = this.subTeamNotAligned,
                RetailUom = this.retailUom,
                RetailSize = this.retailSize,
                NationalClassCode = "40360",                
            };

            return iconItemLastChangeItem;
        }
    }
}
