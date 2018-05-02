using System;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestItemSearchModelBuilder
    {
        private Int32 itemId;
        private string scanCode;
        private string brandName;
        private int? brandHierarchyClassId;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string foodStampEligible;
        private string posScaleTare;
        private string retailSize;
        private string retailUom;
        private string merchandiseHierarchyName;
        private int? merchandiseHierarchyClassId;
        private string taxHierarchyName;
        private int? taxHierarchyClassId;
        private string browsingHierarchyName;
        private int? browsingHierarchyClassId;
        private string isValidated;
        private string hiddenItem;
        private string departmentSale;
        private string nationalHierarchyName;
        private int? nationalHierarchyClassId;
        private string notes;
        private int? animalWelfareRatingId;
        private bool? biodynamic;
        private int? cheeseMilkTypeId;
        private bool? cheeseRaw;
        private int? ecoScaleRatingId;
        private string glutenFreeAgency;
        private string kosherAgency;
        private string nonGmoAgency;
        private string organicAgency;
        private bool? premiumBodyCare;
        private int? productionClaimsId;
        private int? seafoodFreshOrFrozenId;
        private int? seafoodCatchTypeId;
        private string veganAgency;
        private bool? vegetarian;
        private bool? wholeTrade;
        private string createdDate;
        private string lastModifiedDate;
        private string lastModifiedUser;

        public TestItemSearchModelBuilder()
        {
            this.itemId = 0;
            this.scanCode = null;
            this.brandName = null;
            this.brandHierarchyClassId = null;
            this.productDescription = null;
            this.posDescription = null;
            this.packageUnit = null;
            this.foodStampEligible = null;
            this.posScaleTare = null;
            this.retailSize = null;
            this.retailUom = null;
            this.merchandiseHierarchyName = null;
            this.merchandiseHierarchyClassId = null;
            this.taxHierarchyName = null;
            this.taxHierarchyClassId = null;
            this.browsingHierarchyName = null;
            this.browsingHierarchyClassId = null;
            this.isValidated = null;
            this.hiddenItem = null;
            this.departmentSale = null;
            this.nationalHierarchyName = null;
            this.nationalHierarchyClassId = null;
            this.notes = null;
            this.animalWelfareRatingId = null;
            this.biodynamic = null;
            this.cheeseMilkTypeId = null;
            this.cheeseRaw = null;
            this.ecoScaleRatingId= null;
            this.glutenFreeAgency = null;
            this.kosherAgency = null;
            this.nonGmoAgency = null;
            this.organicAgency = null;
            this.premiumBodyCare = null;
            this.productionClaimsId = null;
            this.seafoodFreshOrFrozenId = null;
            this.seafoodCatchTypeId = null;
            this.veganAgency = null;
            this.vegetarian = null;
            this.wholeTrade = null;
            this.createdDate = DateTime.Now.ToString();
            this.lastModifiedDate = null;
            this.lastModifiedUser = null;
        }

        public TestItemSearchModelBuilder WithItemId(Int32 itemId)
        {
            this.itemId = itemId;
            return this;
        }

        public TestItemSearchModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestItemSearchModelBuilder WithBrandName(string brandName)
        {
            this.brandName = brandName;
            return this;
        }

        public TestItemSearchModelBuilder WithBrandHierarchyClassId(int? brandHierarchyClassId)
        {
            this.brandHierarchyClassId = brandHierarchyClassId;
            return this;
        }

        public TestItemSearchModelBuilder WithProductDescription(string productDescription)
        {
            this.productDescription = productDescription;
            return this;
        }

        public TestItemSearchModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestItemSearchModelBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestItemSearchModelBuilder WithFoodStampEligible(string foodStampEligible)
        {
            this.foodStampEligible = foodStampEligible;
            return this;
        }

        public TestItemSearchModelBuilder WithPosScaleTare(string posScaleTare)
        {
            this.posScaleTare = posScaleTare;
            return this;
        }

        public TestItemSearchModelBuilder WithRetailSize(string retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public TestItemSearchModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }

        public TestItemSearchModelBuilder WithMerchandiseHierarchyName(string merchandiseHierarchyName)
        {
            this.merchandiseHierarchyName = merchandiseHierarchyName;
            return this;
        }

        public TestItemSearchModelBuilder WithMerchandiseHierarchyClassId(int? merchandiseHierarchyClassId)
        {
            this.merchandiseHierarchyClassId = merchandiseHierarchyClassId;
            return this;
        }

        public TestItemSearchModelBuilder WithTaxHierarchyName(string taxHierarchyName)
        {
            this.taxHierarchyName = taxHierarchyName;
            return this;
        }

        public TestItemSearchModelBuilder WithTaxHierarchyClassId(int? taxHierarchyClassId)
        {
            this.taxHierarchyClassId = taxHierarchyClassId;
            return this;
        }

        public TestItemSearchModelBuilder WithBrowsingHierarchyName(string browsingHierarchyName)
        {
            this.browsingHierarchyName = browsingHierarchyName;
            return this;
        }

        public TestItemSearchModelBuilder WithBrowsingHierarchyClassId(int? browsingHierarchyClassId)
        {
            this.browsingHierarchyClassId = browsingHierarchyClassId;
            return this;
        }

        public TestItemSearchModelBuilder WithIsValidated(string isValidated)
        {
            this.isValidated = isValidated;
            return this;
        }

        public TestItemSearchModelBuilder WithHiddenItem(string hiddenItem)
        {
            this.hiddenItem = hiddenItem;
            return this;
        }

        public TestItemSearchModelBuilder WithDepartmentSale(string departmentSale)
        {
            this.departmentSale = departmentSale;
            return this;
        }

        public TestItemSearchModelBuilder WithNationalHierarchyName(string nationalHierarchyName)
        {
            this.nationalHierarchyName = nationalHierarchyName;
            return this;
        }

        public TestItemSearchModelBuilder WithNationalHierarchyClassId(int? nationalHierarchyClassId)
        {
            this.nationalHierarchyClassId = nationalHierarchyClassId;
            return this;
        }

        public TestItemSearchModelBuilder WithNotes(string notes)
        {
            this.notes = notes;
            return this;
        }

        public TestItemSearchModelBuilder WithAnimalWelfareRatingId(int? animalWelfareRatingId)
        {
            this.animalWelfareRatingId = animalWelfareRatingId;
            return this;
        }

        public TestItemSearchModelBuilder WithBiodynamic(bool? biodynamic)
        {
            this.biodynamic = biodynamic;
            return this;
        }

        public TestItemSearchModelBuilder WithCheeseMilkTypeId(int? cheeseMilkTypeId)
        {
            this.cheeseMilkTypeId = cheeseMilkTypeId;
            return this;
        }

        public TestItemSearchModelBuilder WithCheeseRaw(bool? cheeseRaw)
        {
            this.cheeseRaw = cheeseRaw;
            return this;
        }

        public TestItemSearchModelBuilder WithEcoScaleRatingId(int? ecoScaleRatingId)
        {
            this.ecoScaleRatingId = ecoScaleRatingId;
            return this;
        }

        public TestItemSearchModelBuilder WithGlutenFreeAgencyId(string glutenFreeAgency)
        {
            this.glutenFreeAgency = glutenFreeAgency;
            return this;
        }

        public TestItemSearchModelBuilder WithKosherAgencyId(string kosherAgency)
        {
            this.kosherAgency = kosherAgency;
            return this;
        }

        public TestItemSearchModelBuilder WithNonGmoAgencyId(int? nonGmoAgencyId)
        {
            this.nonGmoAgency = nonGmoAgency;
            return this;
        }

        public TestItemSearchModelBuilder WithOrganicAgencyId(string organicAgency)
        {
            this.organicAgency = organicAgency;
            return this;
        }

        public TestItemSearchModelBuilder WithPremiumBodyCare(bool? premiumBodyCare)
        {
            this.premiumBodyCare = premiumBodyCare;
            return this;
        }

        public TestItemSearchModelBuilder WithProductionClaimsId(int? productionClaimsId)
        {
            this.productionClaimsId = productionClaimsId;
            return this;
        }

        public TestItemSearchModelBuilder WithSeafoodFreshOrFrozenId(int? seafoodFreshOrFrozenId)
        {
            this.seafoodFreshOrFrozenId = seafoodFreshOrFrozenId;
            return this;
        }

        public TestItemSearchModelBuilder WithSeafoodCatchTypeId(int? seafoodCatchTypeId)
        {
            this.seafoodCatchTypeId = seafoodCatchTypeId;
            return this;
        }

        public TestItemSearchModelBuilder WithVeganAgencyId(string veganAgencyId)
        {
            this.veganAgency = veganAgency;
            return this;
        }

        public TestItemSearchModelBuilder WithVegetarian(bool? vegetarian)
        {
            this.vegetarian = vegetarian;
            return this;
        }

        public TestItemSearchModelBuilder WithWholeTrade(bool? wholeTrade)
        {
            this.wholeTrade = wholeTrade;
            return this;
        }

        public TestItemSearchModelBuilder WithCreatedDate(string createdDate)
        {
            this.createdDate = createdDate;
            return this;
        }

        public TestItemSearchModelBuilder WithLastModifiedDate(string lastModifiedDate)
        {
            this.lastModifiedDate = lastModifiedDate;
            return this;
        }

        public TestItemSearchModelBuilder WithLastModifiedUser(string lastModifiedUser)
        {
            this.lastModifiedUser = lastModifiedUser;
            return this;
        }

        public ItemSearchModel Build()
        {
            ItemSearchModel itemSearchModel = new ItemSearchModel();

            itemSearchModel.ItemId = this.itemId;
            itemSearchModel.ScanCode = this.scanCode;
            itemSearchModel.BrandName = this.brandName;
            itemSearchModel.BrandHierarchyClassId = this.brandHierarchyClassId;
            itemSearchModel.ProductDescription = this.productDescription;
            itemSearchModel.PosDescription = this.posDescription;
            itemSearchModel.PackageUnit = this.packageUnit;
            itemSearchModel.FoodStampEligible = this.foodStampEligible;
            itemSearchModel.PosScaleTare = this.posScaleTare;
            itemSearchModel.RetailSize = this.retailSize;
            itemSearchModel.RetailUom = this.retailUom;
            itemSearchModel.MerchandiseHierarchyName = this.merchandiseHierarchyName;
            itemSearchModel.MerchandiseHierarchyClassId = this.merchandiseHierarchyClassId;
            itemSearchModel.TaxHierarchyName = this.taxHierarchyName;
            itemSearchModel.TaxHierarchyClassId = this.taxHierarchyClassId;
            itemSearchModel.BrowsingHierarchyName = this.browsingHierarchyName;
            itemSearchModel.BrowsingHierarchyClassId = this.browsingHierarchyClassId;
            itemSearchModel.ValidatedDate = this.isValidated;
            itemSearchModel.HiddenItem = this.hiddenItem;
            itemSearchModel.DepartmentSale = this.departmentSale;
            itemSearchModel.NationalHierarchyName = this.nationalHierarchyName;
            itemSearchModel.NationalHierarchyClassId = this.nationalHierarchyClassId;
            itemSearchModel.Notes = this.notes;
            itemSearchModel.AnimalWelfareRatingId = this.animalWelfareRatingId;
            itemSearchModel.Biodynamic = this.biodynamic;
            itemSearchModel.CheeseMilkTypeId = this.cheeseMilkTypeId;
            itemSearchModel.CheeseRaw = this.cheeseRaw;
            itemSearchModel.EcoScaleRatingId = this.ecoScaleRatingId;
            itemSearchModel.GlutenFreeAgency = this.glutenFreeAgency;
            itemSearchModel.KosherAgency = this.kosherAgency;
            itemSearchModel.NonGmoAgency = this.nonGmoAgency;
            itemSearchModel.OrganicAgency = this.organicAgency;
            itemSearchModel.PremiumBodyCare = this.premiumBodyCare;
            itemSearchModel.SeafoodFreshOrFrozenId = this.seafoodFreshOrFrozenId;
            itemSearchModel.SeafoodCatchTypeId = this.seafoodCatchTypeId;
            itemSearchModel.VeganAgency = this.veganAgency;
            itemSearchModel.Vegetarian = this.vegetarian;
            itemSearchModel.WholeTrade = this.wholeTrade;
            itemSearchModel.CreatedDate = this.createdDate;
            itemSearchModel.LastModifiedDate = this.lastModifiedDate;
            itemSearchModel.LastModifiedUser = this.lastModifiedUser;

            return itemSearchModel;
        }

        public static implicit operator ItemSearchModel(TestItemSearchModelBuilder builder)
        {
            return builder.Build();
        }
    }
}

