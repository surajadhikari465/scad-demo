using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Web.Mvc.Models;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestIrmaItemViewModelBuilder
    {
        private int irmaItemId;
        private string region;
        private string identifier;
        private bool defaultIdentifier;
        private string itemDescription;
        private string posDescription;
        private string brandName;
        private int packageUnit;
        private decimal? retailSize;
        private string retailUom;
        private bool foodStamp;
        private decimal posScaleTare;
        private string merchandiseHierarchyClassName;
        private int? merchandiseHierarchyClassId;
        private string taxHierarchyClassName;
        private int? taxHierarchyClassId;
        private bool isNewBrand;
        private string nationaClassName;
        private int? nationalId;
        private int brandId;

        public TestIrmaItemViewModelBuilder()
        {
            this.irmaItemId = 1;
            this.region = "SW";
            this.identifier = "12345123451";
            this.defaultIdentifier = true;
            this.itemDescription = "IrmaItem Controller Test Desc";
            this.posDescription = "IrmaItem Controller Test PosDesc";
            this.brandName = "IrmaItem ControllerBrand";
            this.brandId = 0;
            this.packageUnit = 6;
            this.retailSize = 1;
            this.retailUom = "EACH";
            this.foodStamp = true;
            this.posScaleTare = 0;
            this.merchandiseHierarchyClassId = 11;
            this.merchandiseHierarchyClassName = "Test Merchandise Class Name";
            this.taxHierarchyClassId = 11;
            this.taxHierarchyClassName = "Test Tax Class Name";
            this.isNewBrand = false;
            nationalId = 5;
            nationaClassName = "Class1|5";
        }

        public TestIrmaItemViewModelBuilder WithIrmaItemId(int irmaItemId)
        {
            this.irmaItemId = irmaItemId;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithRegion(string region)
        {
            this.region = region;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithIdentifier(string identifier)
        {
            this.identifier = identifier;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithDefaultIdentifier(bool defaultIdentifier)
        {
            this.defaultIdentifier = defaultIdentifier;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithItemDescription(string itemDescription)
        {
            this.itemDescription = itemDescription;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithBrandName(string brandName)
        {
            this.brandName = brandName;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithPackageUnit(int packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithRetailSize(decimal? retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithFoodStampEligible(bool foodStamp)
        {
            this.foodStamp = foodStamp;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithPosScaleTare(decimal posScaleTare)
        {
            this.posScaleTare = posScaleTare;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithMerchandiseClassName(string merchandiseClassName)
        {
            this.merchandiseHierarchyClassName = merchandiseClassName;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithMerchandiseClassId(int merchandiseClassId)
        {
            this.merchandiseHierarchyClassId = merchandiseClassId;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithTaxClassName(string taxClassName)
        {
            this.taxHierarchyClassName = taxClassName;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithTaxClassId(int taxClassId)
        {
            this.taxHierarchyClassId = taxClassId;
            return this;
        }


        public TestIrmaItemViewModelBuilder WithNationalClassName(string nationalClassName)
        {
            this.nationaClassName = nationalClassName;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithNationalClassId(int nationalClassId)
        {
            this.nationalId = nationalClassId;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithIsNewBrand(bool isNewBrand)
        {
            this.isNewBrand = isNewBrand;
            return this;
        }

        public TestIrmaItemViewModelBuilder WithBrandId(int brandId)
        {
            this.brandId = brandId;
            return this;
        }

        public IrmaItemViewModel Build()
        {
            var viewModel = new IrmaItemViewModel
            {
                IrmaItemId = this.irmaItemId,
                Identifier = this.identifier,
                DefaultIdentifier = this.defaultIdentifier,
                ItemDescription = this.itemDescription,
                PosDescription = this.posDescription,
                BrandName = this.brandName,
                BrandId = this.brandId,
                PackageUnit = this.packageUnit,
                RetailSize = this.retailSize,
                RetailUom = this.retailUom,
                FoodStamp = this.foodStamp,
                PosScaleTare = this.posScaleTare,
                MerchandiseHierarchyClassId = this.merchandiseHierarchyClassId,
                MerchandiseHierarchyClassName = this.merchandiseHierarchyClassName,
                TaxHierarchyClassId = this.taxHierarchyClassId,
                TaxHierarchyClassName = this.taxHierarchyClassName,
                IsNewBrand = this.isNewBrand,
                NationalHierarchyClassId = this.nationalId,
                NationalHierarchyClassName = this.nationaClassName
            };

            return viewModel;
        }

        public static implicit operator IrmaItemViewModel(TestIrmaItemViewModelBuilder builder)
        {
            return builder.Build();
        }
    }
}
