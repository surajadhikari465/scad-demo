using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestBulkImportNewItemModelBuilder
    {
        private string scanCode;
        private string brandName;
        private string brandLineage;
        private string brandClassId;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string foodStampEligible;
        private string posScaleTare;
        private string retailSize;
        private string retailUom;
        private string deliverySystem;
        private string merchandiseClassId;
        private string merchandiseLineage;
        private string taxClassId;
        private string alcoholByVolume;
        private string taxLineage;
        private string browsingClassId;
        private string browsingLineage;
        private string isValidated;
        private string nationalClassId;
        private string nationalLineage;
        private string animalWelfareRatingId;
        private string animalWelfareRating;
        private string biodynamic;
        private string milkTypeId;
        private string milkType;
        private string cheeseAttributeRaw;
        private string ecoScaleRatingId;
        private string ecoScaleRating;
        private string glutenFreeAgency;
        private string kosherAgency;
        private string nonGmoAgency;
        private string organicAgency;
        private string premiumBodyCare;
        private string seafoodFreshOrFrozenId;
        private string seafoodFreshOrFrozen;
        private string seafoodWildOrFarmRaisedId;
        private string seafoodWildOrFarmRaised;
        private string veganAgency;
        private string vegetarian;
        private string wholeTrade;
        private string grassFed;
        private string pastureRaised;
        private string freeRange;
        private string dryAged;
        private string airChilled;
        private string madeInHouse;
        private string error;
        private string msc;
        private string nutritionRequired;

        public TestBulkImportNewItemModelBuilder()
        {
            scanCode = "222222222222";
            brandName = "Test Brand";
            brandClassId = "1";
            brandLineage = "Test Brand|1";
            productDescription = "Test New Item";
            posDescription = "New Item";
            packageUnit = "1";
            foodStampEligible = "0";
            posScaleTare = "0";
            retailSize = "3";
            retailUom = "EA";
            deliverySystem = "CAP";
            merchandiseClassId = "1";
            merchandiseLineage = "Test Merchandise|1";
            taxClassId = "1";
            alcoholByVolume = "99.99";
            taxLineage = "Test Tax|1";
            browsingClassId = "0";
            isValidated = "0";
            nationalClassId = "5";
            nationalLineage = "Class1";
            animalWelfareRating = "NoStep";
            biodynamic = "1";
            milkType = "BuffaloMilk";
            cheeseAttributeRaw = "0";
            ecoScaleRating = "BaselineOrange";
            glutenFreeAgency = String.Empty;
            kosherAgency = String.Empty;
            nonGmoAgency = String.Empty;
            organicAgency = String.Empty;
            premiumBodyCare = "1";
            seafoodFreshOrFrozen = "PreviouslyFrozen";
            seafoodWildOrFarmRaised = "Wild";
            veganAgency = String.Empty;
            vegetarian = "1";
            wholeTrade = "0";
            msc = String.Empty;
            error = String.Empty;
            grassFed = String.Empty;
            pastureRaised = String.Empty;
            freeRange = String.Empty;
            dryAged = String.Empty;
            airChilled = String.Empty;
            madeInHouse = String.Empty;
            nutritionRequired = String.Empty;
        }

        public TestBulkImportNewItemModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithProductDescription(string productDescription)
        {
            this.productDescription = productDescription;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithFoodStampEligible(string foodStampEligible)
        {
            this.foodStampEligible = foodStampEligible;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithPosScaleTare(string posScaleTare)
        {
            this.posScaleTare = posScaleTare;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithRetailSize(string retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithDeliverySystem(string deliverySystem)
        {
            this.deliverySystem = deliverySystem;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithBrandName(string brandName)
        {
            this.brandName = brandName;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithBrandId(string brandClassId)
        {
            this.brandClassId = brandClassId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithBrandLineage(string brandLineage)
        {
            this.brandLineage = brandLineage;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithMerchandiseId(string merchandiseClassId)
        {
            this.merchandiseClassId = merchandiseClassId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithMerchandiseLineage(string merchandiseLineage)
        {
            this.merchandiseLineage = merchandiseLineage;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithTaxId(string taxClassId)
        {
            this.taxClassId = taxClassId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithTaxLineage(string taxLineage)
        {
            this.taxLineage = taxLineage;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithAlcoholByVolume(string alcoholByVolume)
        {
            this.alcoholByVolume = alcoholByVolume;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithNationalId(string nationalClassId)
        {
            this.nationalClassId = nationalClassId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithNationalLineage(string nationalLineage)
        {
            this.nationalLineage = nationalLineage;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithBrowsingId(string browsingClassId)
        {
            this.browsingClassId = browsingClassId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithBrowsingLineage(string browsingLineage)
        {
            this.browsingLineage = browsingLineage;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithIsValidated(bool isValidated)
        {
            this.isValidated = isValidated ? "1" : "0";
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithAnimalWelfareRatingId(string animalWelfareRatingId)
        {
            this.animalWelfareRatingId = animalWelfareRatingId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithAnimalWelfareRating(string animalWelfareRating)
        {
            this.animalWelfareRating = animalWelfareRating;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithMilkTypeId(string milkTypeId)
        {
            this.milkTypeId = milkTypeId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithMilkType(string milkType)
        {
            this.milkType = milkType;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithEcoScaleRatingId(string ecoScaleRatingId)
        {
            this.ecoScaleRatingId = ecoScaleRatingId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithEcoScaleRating(string ecoScaleRating)
        {
            this.ecoScaleRating = ecoScaleRating;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithSeafoodFreshOrFrozenId(string seafoodFreshOrFrozenId)
        {
            this.seafoodFreshOrFrozenId = seafoodFreshOrFrozenId;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithSeafoodFreshOrFrozen(string seafoodFreshOrFrozen)
        {
            this.seafoodFreshOrFrozen = seafoodFreshOrFrozen;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithSeafoodCatchType(string seafoodCatchType)
        {
            this.seafoodWildOrFarmRaised = seafoodCatchType;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithSeafoodCatchTypeId(string seafoodCatchTypeId)
        {
            this.seafoodWildOrFarmRaisedId = seafoodCatchTypeId;
            return this;
        }
        public TestBulkImportNewItemModelBuilder WithGrassFed(string grassFed)
        {
            this.grassFed = grassFed;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithPastureRaised(string pastureRaised)
        {
            this.pastureRaised = pastureRaised;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithFreeRange(string freeRange)
        {
            this.freeRange = freeRange;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithDryAged(string dryAged)
        {
            this.dryAged = dryAged;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithAirChilled(string airChilled)
        {
            this.airChilled = airChilled;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithMadeInHouse(string madeInHouse)
        {
            this.madeInHouse = madeInHouse;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithMsc(string msc)
        {
            this.msc = msc;
            return this;
        }

        public TestBulkImportNewItemModelBuilder WithNutritionRequired(string nutritionRequired)
        {
            this.nutritionRequired = nutritionRequired;
            return this;
        }

        public BulkImportNewItemModel Build()
        {
            return new BulkImportNewItemModel
            {
                ScanCode = scanCode,
                ProductDescription = productDescription,
                PosDescription = posDescription,
                PackageUnit = packageUnit,
                RetailSize = retailSize,
                RetailUom = retailUom,
                DeliverySystem = deliverySystem,
                BrandId = brandClassId,
                BrandName = brandName,
                BrandLineage = brandLineage,
                MerchandiseId = merchandiseClassId,
                MerchandiseLineage = merchandiseLineage,
                TaxId = taxClassId,
                TaxLineage = taxLineage,
                AlcoholByVolume = alcoholByVolume,
                NationalId = nationalClassId,
                NationalLineage = nationalLineage,
                BrowsingId = browsingClassId,
                BrowsingLineage = browsingLineage,
                FoodStampEligible = foodStampEligible,
                PosScaleTare = posScaleTare,
                IsValidated = isValidated,
                AnimalWelfareRating = animalWelfareRating,
                Biodynamic = biodynamic,
                CheeseAttributeMilkType = milkType,
                CheeseAttributeRaw = cheeseAttributeRaw,
                EcoScaleRating = ecoScaleRating,
                GlutenFreeAgency = glutenFreeAgency,
                KosherAgency = kosherAgency,
                NonGmoAgency = nonGmoAgency,
                OrganicAgency = organicAgency,
                PremiumBodyCare = premiumBodyCare,
                SeafoodFreshOrFrozen = seafoodFreshOrFrozen,
                SeafoodWildOrFarmRaised = seafoodWildOrFarmRaised,
                VeganAgency = veganAgency,
                Vegetarian = vegetarian,
                WholeTrade = wholeTrade,
                GrassFed = grassFed,
                PastureRaised = pastureRaised,
                FreeRange = freeRange,
                DryAged = dryAged,
                AirChilled = airChilled,
                MadeInHouse = madeInHouse,
                Msc = msc,
                NutritionRequired = nutritionRequired
            };
        }

        public static implicit operator BulkImportNewItemModel(TestBulkImportNewItemModelBuilder builder)
        {
            return builder.Build();
        }
    }
}
