using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace MammothWebApi.Tests.DataAccess.ModelBuilders
{
    internal class TestStagingItemModelBuilder
    {
        private int itemId;
        private int? itemTypeId;
        private string scanCode;
        private int? subBrickId;
        private int? hierarchyNationalClassId;
        private int? brandId;
        private int? taxClassId;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string retailSize;
        private string retailUom;
        private int? subTeamId;
        private bool foodStampEligible;
        private DateTime? timeStamp;

        public TestStagingItemModelBuilder()
        {
            this.itemId = 1;
            this.itemTypeId = 1;
            this.scanCode = "888877778881";
            this.subBrickId = 1;
            this.hierarchyNationalClassId = 1;
            this.brandId = 1;
            this.taxClassId = 1;
            this.productDescription = "Test Item 1";
            this.posDescription = "Test Item 1";
            this.packageUnit = "1";
            this.retailSize = "1";
            this.retailUom = "EA";
            this.subTeamId = 1;
            this.foodStampEligible = true;
            this.timeStamp = DateTime.UtcNow;
        }

        internal TestStagingItemModelBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }

        internal TestStagingItemModelBuilder WithItemTypeId(int? itemTypeId)
        {
            this.itemTypeId = itemTypeId;
            return this;
        }

        internal TestStagingItemModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        internal TestStagingItemModelBuilder WithMerchandiseId(int? subBrickId)
        {
            this.subBrickId = subBrickId;
            return this;
        }

        internal TestStagingItemModelBuilder WithNationalClassId(int? nationalClassId)
        {
            this.hierarchyNationalClassId = nationalClassId;
            return this;
        }

        internal TestStagingItemModelBuilder WithBrandId(int? brandId)
        {
            this.brandId = brandId;
            return this;
        }

        internal TestStagingItemModelBuilder WithTaxClassId(int? taxClassId)
        {
            this.taxClassId = taxClassId;
            return this;
        }

        internal TestStagingItemModelBuilder WithProductDescription(string description)
        {
            this.productDescription = description;
            return this;
        }

        internal TestStagingItemModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        internal TestStagingItemModelBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        internal TestStagingItemModelBuilder WithRetailSzie(string retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        internal TestStagingItemModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }

        internal TestStagingItemModelBuilder WithSubTeam(int? subTeamId)
        {
            this.subTeamId = subTeamId;
            return this;
        }

        internal TestStagingItemModelBuilder WithTimestamp(DateTime? timeStamp)
        {
            this.timeStamp = timeStamp;
            return this;
        }

        internal TestStagingItemModelBuilder WithFoodStamp(bool foodStamp)
        {
            this.foodStampEligible = foodStamp;
            return this;
        }

        internal StagingItemModel Build()
        {
            var model = new StagingItemModel
            {
                ItemID = this.itemId,
                ItemTypeID = this.itemTypeId,
                ScanCode = this.scanCode,
                BrandHCID = this.brandId,
                SubBrickID = this.subBrickId,
                NationalClassID = this.hierarchyNationalClassId,
                TaxClassHCID = this.taxClassId,
                Desc_Product = this.productDescription,
                Desc_POS = this.posDescription,
                PackageUnit = this.packageUnit,
                RetailSize = this.retailSize,
                RetailUOM = this.retailUom,
                PSNumber = this.subTeamId,
                FoodStampEligible = this.foodStampEligible,
                Timestamp = this.timeStamp
            };

            return model;
        }

        internal List<StagingItemModel> BuildList(int numberOfItems, DateTime timestamp)
        {
            var items = new List<StagingItemModel>();
            for (int i = 0; i < numberOfItems; i++)
            {
                items.Add(new TestStagingItemModelBuilder()
                    .WithItemId(i + 1)
                    .WithScanCode(String.Format("777777{0}", i))
                    .WithBrandId(1)
                    .WithTaxClassId(1)
                    .WithNationalClassId(null)
                    .WithMerchandiseId(1)
                    .WithSubTeam(1)
                    .WithProductDescription(String.Format("Test Prod Desc {0}", i))
                    .WithPosDescription(String.Format("Test Pos Desc {0}", i))
                    .WithTimestamp(timestamp)
                    .WithFoodStamp(true)
                    .Build());
            }

            return items;
        }

        internal List<StagingItemModel> BuildList(int numberOfItems, int maxItemId, DateTime timestamp)
        {
            var items = new List<StagingItemModel>();
            for (int i = 1; i <= numberOfItems; i++)
            {
                items.Add(new TestStagingItemModelBuilder()
                    .WithItemId(maxItemId + i)
                    .WithScanCode(String.Format("777777{0}", i))
                    .WithBrandId(1)
                    .WithTaxClassId(1)
                    .WithNationalClassId(null)
                    .WithMerchandiseId(1)
                    .WithSubTeam(1)
                    .WithProductDescription(String.Format("Test Prod Desc {0}", i))
                    .WithPosDescription(String.Format("Test Pos Desc {0}", i))
                    .WithTimestamp(timestamp)
                    .WithFoodStamp(true)
                    .Build());
            }

            return items;
        }
    }
}
