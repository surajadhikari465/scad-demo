using MammothWebApi.Models;
using System;

namespace MammothWebApi.Tests.ModelBuilders
{
    internal class TestItemModelBuilder
    {
        private int itemId;
        private int itemTypeId;
        private string scanCode;
        private int? subBrickId;
        private int? nationalClassId;
        private int brandId;
        private int? taxClassId;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string retailSize;
        private string retailUom;
        private int? subTeamId;
        private string foodStamp;

        public TestItemModelBuilder()
        {
            this.itemId = 1;
            this.itemTypeId = 1;
            this.scanCode = "888877778881";
            this.subBrickId = 1;
            this.nationalClassId = 1;
            this.brandId = 1;
            this.taxClassId = 1;
            this.productDescription = "Test Item 1";
            this.posDescription = "Test Item 1";
            this.packageUnit = "1";
            this.retailSize = "1";
            this.retailUom = "EA";
            this.subBrickId = 1;
            this.foodStamp = "1";
        }

        internal TestItemModelBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        internal TestItemModelBuilder WithItemTypeId(int? itemTypeId)
        {
            this.itemTypeId = itemTypeId.Value;
            return this;
        }

        internal TestItemModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestItemModelBuilder WithSubBrickId(int? subBrickId)
        {
            this.subBrickId = subBrickId;
            return this;
        }

        internal TestItemModelBuilder WithNationalClassId(int? nationalClassId)
        {
            this.nationalClassId = nationalClassId;
            return this;
        }

        internal TestItemModelBuilder WithBrandId(int? brandId)
        {
            this.brandId = brandId.Value;
            return this;
        }

        internal TestItemModelBuilder WithTaxClassId(int? taxClassId)
        {
            this.taxClassId = taxClassId;
            return this;
        }

        internal TestItemModelBuilder WithProductDescription(string description)
        {
            this.productDescription = description;
            return this;
        }

        internal TestItemModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        internal TestItemModelBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        internal TestItemModelBuilder WithRetailSize(string retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        internal TestItemModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }

        internal TestItemModelBuilder WithSubTeam(int subTeamId)
        {
            this.subTeamId = subTeamId;
            return this;
        }
        
        internal TestItemModelBuilder WithFoodStamp(string foodStamp)
        {
            this.foodStamp = foodStamp;
            return this;
        }

        internal ItemModel Build()
        {
            var model = new ItemModel
            {
                ItemId = this.itemId,
                ItemTypeId = this.itemTypeId,
                ScanCode = this.scanCode,
                BrandId = this.brandId,
                SubBrickId = this.subBrickId,
                NationalClassId = this.nationalClassId,
                TaxClassId = this.taxClassId,
                ProductDescription = this.productDescription,
                PosDescription = this.posDescription,
                PackageUnit = this.packageUnit,
                RetailSize = this.retailSize,
                RetailUom = this.retailUom,
                SubTeamId = this.subTeamId,
                FoodStamp = this.foodStamp
            };

            return model;
        }
    }
}
