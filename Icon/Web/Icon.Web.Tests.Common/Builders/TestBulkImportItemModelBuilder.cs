using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System;
using System.Linq;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestBulkImportItemModelBuilder
    {

        private string scanCode;
        private string brandName;
        private string brandLineage;
        private string brandId;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string retailSize;
        private string retailUom;
        private string deliverySystem;
        private string foodStampEligible;
        private string posScaleTare;
        private string merchandiseLineage;
        private string merchandiseId;
        private string taxLineage;
        private string taxId;
        private string browsingLineage;
        private string browsingId;
        private string isValidated;
        private string departmentSale;
        private string note;
        private string hiddenItem;
        private string nationalLineage;
        private string nationalId;
        private string animalWelfareRatingId;
        private string animalWelfareRating;
        private string biodynamic;
        private string milkTypeId;
        private string milkType;
        private string cheeseAttributeRaw;
        private string ecoScaleRatingId;
        private string ecoScaleRating;
        private string glutenFreeAgencyId;
        private string kosherAgencyId;
        private string nonGmoAgencyId;
        private string organicAgencyId;
        private string premiumBodyCare;
        private string seafoodFreshOrFrozenId;
        private string seafoodFreshOrFrozen;
        private string seafoodWildOrFarmRaisedId;
        private string seafoodWildOrFarmRaised;
        private string veganAgencyId;
        private string vegetarian;
        private string wholeTrade;
        private string grassFed;
        private string pastureRaised;
        private string freeRange;
        private string dryAged;
        private string airChilled;
        private string madeInHouse;
        private string msc;
        private string paleo;
        private string productFlavorType;
        private string organicPersonalCare;
        private string nutritionRequried;
        private string mainProductName;
        private string localeLoanProducer;
        private string hemp;
        private string fairTradeCertified;
        private string drainedWeightUom;
        private string drainedWeight;
        private string alcoholByVolume;
        private string caseineFree;

        public TestBulkImportItemModelBuilder()
        {
            scanCode = "222222222222";
            brandName = "TestBrand";
            brandLineage = "TestBrand|1";
            brandId = "1";
            productDescription = "Test Prod Desc";
            posDescription = "Test POS Desc";
            packageUnit = "1";
            retailSize = "1";
            retailUom = "EA";
            deliverySystem = "CAP";
            foodStampEligible = "0";
            posScaleTare = "0";
            merchandiseLineage = "Test Merch|1";
            merchandiseId = "1";
            taxLineage = "Test Tax|1";
            taxId = "1";
            browsingLineage = "Test Browsing|1";
            browsingId = "1";
            isValidated = String.Empty;
            departmentSale = String.Empty;
            note = "test note";
            hiddenItem = String.Empty;
            nationalId = "5";
            nationalLineage = "Class1|5";
            animalWelfareRatingId = String.Empty;
            biodynamic = String.Empty;
            milkTypeId = String.Empty;
            milkType = String.Empty;
            cheeseAttributeRaw = String.Empty;
            ecoScaleRatingId = String.Empty;
            ecoScaleRating = String.Empty;
            glutenFreeAgencyId = String.Empty;
            kosherAgencyId = String.Empty;
            nonGmoAgencyId = String.Empty;
            organicAgencyId = String.Empty;
            premiumBodyCare = String.Empty;
            seafoodFreshOrFrozenId = String.Empty;
            seafoodFreshOrFrozen = String.Empty;
            seafoodWildOrFarmRaisedId = String.Empty;
            seafoodWildOrFarmRaised = String.Empty;
            veganAgencyId = String.Empty;
            vegetarian = String.Empty;
            wholeTrade = String.Empty;
            grassFed = String.Empty;
            pastureRaised = String.Empty;
            freeRange = String.Empty;
            dryAged = String.Empty;
            airChilled = String.Empty;
            madeInHouse = String.Empty;
            msc = String.Empty;
        }

        public TestBulkImportItemModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestBulkImportItemModelBuilder WithBrandName(string brandName)
        {
            this.brandName = brandName;
            return this;
        }

        public TestBulkImportItemModelBuilder WithBrandLineage(string brandLineage)
        {
            this.brandLineage = brandLineage;
            return this;
        }

        public TestBulkImportItemModelBuilder WithBrandId(string brandId)
        {
            this.brandId = brandId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithProductDescription(string productDescription)
        {
            this.productDescription = productDescription;
            return this;
        }

        public TestBulkImportItemModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestBulkImportItemModelBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestBulkImportItemModelBuilder WithRetailSize(string retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public TestBulkImportItemModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }
        public TestBulkImportItemModelBuilder WithDeliverySystem(string deliverySystem)
        {
            this.deliverySystem = deliverySystem;
            return this;
        }
        public TestBulkImportItemModelBuilder WithFoodStampEligible(string foodStampEligible)
        {
            this.foodStampEligible = foodStampEligible;
            return this;
        }

        public TestBulkImportItemModelBuilder WithPosScaleTare(string posScaleTare)
        {
            this.posScaleTare = posScaleTare;
            return this;
        }

        public TestBulkImportItemModelBuilder WithMerchandiseLineage(string merchandiseLineage)
        {
            this.merchandiseLineage = merchandiseLineage;
            return this;
        }

        public TestBulkImportItemModelBuilder WithMerchandiseId(string merchandiseId)
        {
            this.merchandiseId = merchandiseId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithTaxLineage(string taxLineage)
        {
            this.taxLineage = taxLineage;
            return this;
        }

        public TestBulkImportItemModelBuilder WithTaxId(string taxId)
        {
            this.taxId = taxId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithNationalLineage(string nationalLineage)
        {
            this.nationalLineage = nationalLineage;
            return this;
        }

        public TestBulkImportItemModelBuilder WithNationalId(string nationalId)
        {
            this.nationalId = nationalId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithBrowsingLineage(string browsingLineage)
        {
            this.browsingLineage = browsingLineage;
            return this;
        }

        public TestBulkImportItemModelBuilder WithBrowsingId(string browsingId)
        {
            this.browsingId = browsingId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithIsValidated(string isValidated)
        {
            this.isValidated = isValidated;
            return this;
        }

        public TestBulkImportItemModelBuilder WithDepartmentSale(string departmentSale)
        {
            this.departmentSale = departmentSale;
            return this;
        }

        public TestBulkImportItemModelBuilder WithHiddenItem(string hiddenItem)
        {
            this.hiddenItem = hiddenItem;
            return this;
        }

        public TestBulkImportItemModelBuilder WithNote(string note)
        {
            this.note = note;
            return this;
        }

        public TestBulkImportItemModelBuilder WithAnimalWelfareRatingId(string animalWelfareRatingId)
        {
            this.animalWelfareRatingId = animalWelfareRatingId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithAnimalWelfareRating(string animalWelfareRating)
        {
            this.animalWelfareRating = animalWelfareRating;
            return this;
        }

        public TestBulkImportItemModelBuilder WithBiodynamic(string biodynamic)
        {
            this.biodynamic = biodynamic;
            return this;
        }

        public TestBulkImportItemModelBuilder WithMilkTypeId(string milkTypeId)
        {
            this.milkTypeId = milkTypeId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithMilkType(string milkType)
        {
            this.milkType = milkType;
            return this;
        }

        public TestBulkImportItemModelBuilder WithCheeseRaw(string cheeseRaw)
        {
            this.cheeseAttributeRaw = cheeseRaw;
            return this;
        }

        public TestBulkImportItemModelBuilder WithEcoScaleRatingId(string ecoScaleRatingId)
        {
            this.ecoScaleRatingId = ecoScaleRatingId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithEcoScaleRating(string ecoScaleRating)
        {
            this.ecoScaleRating = ecoScaleRating;
            return this;
        }

        public TestBulkImportItemModelBuilder WithGlutenFreeAgency(string glutenFreeAgencyId)
        {
            this.glutenFreeAgencyId = glutenFreeAgencyId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithKosherAgency(string kosherAgencyId)
        {
            this.kosherAgencyId = kosherAgencyId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithNonGmoAgency(string nonGmoAgencyId)
        {
            this.nonGmoAgencyId = nonGmoAgencyId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithOrganicAgency(string organicAgencyId)
        {
            this.organicAgencyId = organicAgencyId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithPremiumBodyCare(string premiumBodyCare)
        {
            this.premiumBodyCare = premiumBodyCare;
            return this;
        }

        public TestBulkImportItemModelBuilder WithSeafoodFreshOrFrozenId(string seafoodFreshOrFrozenId)
        {
            this.seafoodFreshOrFrozenId = seafoodFreshOrFrozenId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithSeafoodFreshOrFrozen(string seafoodFreshOrFrozen)
        {
            this.seafoodFreshOrFrozen = seafoodFreshOrFrozen;
            return this;
        }

        public TestBulkImportItemModelBuilder WithSeafoodCatchType(string seafoodCatchType)
        {
            this.seafoodWildOrFarmRaised = seafoodCatchType;
            return this;
        }

        public TestBulkImportItemModelBuilder WithSeafoodCatchTypeId(string seafoodCatchTypeId)
        {
            this.seafoodWildOrFarmRaisedId = seafoodCatchTypeId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithVeganAgency(string veganAgencyId)
        {
            this.veganAgencyId = veganAgencyId;
            return this;
        }

        public TestBulkImportItemModelBuilder WithVegetarian(string vegetarian)
        {
            this.vegetarian = vegetarian;
            return this;
        }

        public TestBulkImportItemModelBuilder WithWholeTrade(string wholeTrade)
        {
            this.wholeTrade = wholeTrade;
            return this;
        }

        public TestBulkImportItemModelBuilder WithGrassFed(string grassFed)
        {
            this.grassFed = grassFed;
            return this;
        }

        public TestBulkImportItemModelBuilder WithPastureRaised(string pastureRaised)
        {
            this.pastureRaised = pastureRaised;
            return this;
        }

        public TestBulkImportItemModelBuilder WithFreeRange(string freeRange)
        {
            this.freeRange = freeRange;
            return this;
        }

        public TestBulkImportItemModelBuilder WithDryAged(string dryAged)
        {
            this.dryAged = dryAged;
            return this;
        }

        public TestBulkImportItemModelBuilder WithAirChilled(string airChilled)
        {
            this.airChilled = airChilled;
            return this;
        }

        public TestBulkImportItemModelBuilder WithMadeInHouse(string madeInHouse)
        {
            this.madeInHouse = madeInHouse;
            return this;
        }

        public TestBulkImportItemModelBuilder WithMsc(string msc)
        {
            this.msc = msc;
            return this;
        }

        public TestBulkImportItemModelBuilder WithHierarchyClassIds(string brandId, string merchandiseId, string taxId, string browsingId, string nationalId)
        {
            this.brandId = brandId;
            this.merchandiseId = merchandiseId;
            this.taxId = taxId;
            this.browsingId = browsingId;
            this.nationalId = nationalId;

            return this;
        }

        public TestBulkImportItemModelBuilder Empty()
        {
            this.scanCode = String.Empty;
            this.productDescription = String.Empty;
            this.posDescription = String.Empty;
            this.packageUnit = String.Empty;
            this.retailSize = String.Empty;
            this.retailUom = String.Empty;
            this.deliverySystem = String.Empty;
            this.foodStampEligible = String.Empty;
            this.posScaleTare = String.Empty;
            this.isValidated = String.Empty;
            this.departmentSale = String.Empty;
            this.brandName = String.Empty;
            this.brandId = String.Empty;
            this.brandLineage = String.Empty;
            this.merchandiseId = String.Empty;
            this.merchandiseLineage = String.Empty;
            this.taxId = String.Empty;
            this.taxLineage = String.Empty;
            this.browsingId = String.Empty;
            this.browsingLineage = String.Empty;
            this.hiddenItem = String.Empty;
            this.nationalId = String.Empty;
            this.nationalLineage = String.Empty;
            this.departmentSale = String.Empty;
            this.note = String.Empty;
            this.hiddenItem = String.Empty;
            
            return this;
        }

        public TestBulkImportItemModelBuilder FromItem(Item item)
        {
            this.scanCode = item.ScanCode.FirstOrDefault() == null ? null : item.ScanCode.First().scanCode;
            this.productDescription = GetItemTrait(item, Traits.ProductDescription);
            this.posDescription = GetItemTrait(item, Traits.PosDescription);
            this.packageUnit = GetItemTrait(item, Traits.PackageUnit);
            this.retailSize = GetItemTrait(item, Traits.RetailSize);
            this.retailUom = GetItemTrait(item, Traits.RetailUom);
            this.deliverySystem = GetItemTrait(item, Traits.DeliverySystem);
            this.foodStampEligible = GetItemTrait(item, Traits.FoodStampEligible);
            this.posScaleTare = GetItemTrait(item, Traits.PosScaleTare);
            this.isValidated = GetItemTrait(item, Traits.ValidationDate);
            this.departmentSale = GetItemTrait(item, Traits.DepartmentSale);
            this.hiddenItem = GetItemTrait(item, Traits.HiddenItem);

            var brand = GetItemHierarchyClass(item, Hierarchies.Brands);
            this.brandId = brand.Item1;
            this.brandLineage = brand.Item2;

            var merch = GetItemHierarchyClass(item, Hierarchies.Merchandise);
            this.merchandiseId = merch.Item1;
            this.merchandiseLineage = merch.Item2;

            var tax = GetItemHierarchyClass(item, Hierarchies.Tax);
            this.taxId = tax.Item1;
            this.taxLineage = tax.Item2;

            var browsing = GetItemHierarchyClass(item, Hierarchies.Browsing);
            this.browsingId = browsing.Item1;
            this.browsingLineage = browsing.Item2;

            var national = GetItemHierarchyClass(item, Hierarchies.National);
            this.nationalId = national.Item1;
            this.nationalLineage = national.Item2;

            return this;
        }

        public TestBulkImportItemModelBuilder FromItemModel(ItemSearchModel item)
        {
            this.scanCode = item.ScanCode;
            this.productDescription = item.ProductDescription;
            this.posDescription = item.PosDescription;
            this.packageUnit = item.PackageUnit;
            this.retailSize = item.RetailSize;
            this.retailUom = item.RetailUom;
            this.deliverySystem = item.DeliverySystem;
            this.foodStampEligible = item.FoodStampEligible;
            this.posScaleTare = item.PosScaleTare;
            this.isValidated = item.ValidatedDate;
            this.departmentSale = item.DepartmentSale;
            this.hiddenItem = item.HiddenItem;

            this.brandId = item.BrandHierarchyClassId.HasValue ? item.BrandHierarchyClassId.Value.ToString() : null;
            this.brandLineage = item.BrandName;
            this.taxId = item.TaxHierarchyClassId.HasValue ? item.TaxHierarchyClassId.Value.ToString() : null;
            this.taxLineage = item.TaxHierarchyName;
            this.merchandiseId = item.MerchandiseHierarchyClassId.HasValue ? item.MerchandiseHierarchyClassId.Value.ToString() : null;
            this.merchandiseLineage = item.MerchandiseHierarchyName;
            this.browsingId = item.BrowsingHierarchyClassId.HasValue ? item.BrowsingHierarchyClassId.Value.ToString() : null;
            this.browsingLineage = item.BrowsingHierarchyName;

            this.nationalId = item.NationalHierarchyClassId.HasValue ? item.NationalHierarchyClassId.Value.ToString() : null;
            this.nationalLineage = item.NationalHierarchyName;

            return this;
        }

        public BulkImportItemModel Build()
        {
            var bulkImportItemModel = new BulkImportItemModel();

            bulkImportItemModel.ScanCode = scanCode;
            bulkImportItemModel.BrandName = brandName;
            bulkImportItemModel.BrandLineage = brandLineage;
            bulkImportItemModel.BrandId = brandId;
            bulkImportItemModel.ProductDescription = productDescription;
            bulkImportItemModel.PosDescription = posDescription;
            bulkImportItemModel.PackageUnit = packageUnit;
            bulkImportItemModel.RetailSize = retailSize;
            bulkImportItemModel.RetailUom = retailUom;
            bulkImportItemModel.DeliverySystem = deliverySystem;
            bulkImportItemModel.FoodStampEligible = foodStampEligible;
            bulkImportItemModel.PosScaleTare = posScaleTare;
            bulkImportItemModel.MerchandiseLineage = merchandiseLineage;
            bulkImportItemModel.MerchandiseId = merchandiseId;
            bulkImportItemModel.TaxLineage = taxLineage;
            bulkImportItemModel.TaxId = taxId;
            bulkImportItemModel.BrowsingLineage = browsingLineage;
            bulkImportItemModel.BrowsingId = browsingId;
            bulkImportItemModel.IsValidated = isValidated;
            bulkImportItemModel.DepartmentSale = departmentSale;
            bulkImportItemModel.HiddenItem = hiddenItem;
            bulkImportItemModel.Notes = note;
            bulkImportItemModel.NationalId = nationalId;
            bulkImportItemModel.NationalLineage = nationalLineage;
            bulkImportItemModel.AnimalWelfareRatingId = animalWelfareRatingId;
            bulkImportItemModel.AnimalWelfareRating = animalWelfareRating;
            bulkImportItemModel.Biodynamic = biodynamic;
            bulkImportItemModel.CheeseAttributeMilkType = milkType;
            bulkImportItemModel.CheeseAttributeMilkTypeId = milkTypeId;
            bulkImportItemModel.CheeseAttributeRaw = cheeseAttributeRaw;
            bulkImportItemModel.EcoScaleRating = ecoScaleRating;
            bulkImportItemModel.EcoScaleRatingId = ecoScaleRatingId;
            bulkImportItemModel.GlutenFreeAgencyId = glutenFreeAgencyId;
            bulkImportItemModel.KosherAgencyId = kosherAgencyId;
            bulkImportItemModel.NonGmoAgencyId = nonGmoAgencyId;
            bulkImportItemModel.OrganicAgencyId = organicAgencyId;
            bulkImportItemModel.PremiumBodyCare = premiumBodyCare;
            bulkImportItemModel.SeafoodFreshOrFrozenId = seafoodFreshOrFrozenId;
            bulkImportItemModel.SeafoodFreshOrFrozen = seafoodFreshOrFrozen;
            bulkImportItemModel.SeafoodWildOrFarmRaisedId = seafoodWildOrFarmRaisedId;
            bulkImportItemModel.SeafoodWildOrFarmRaised = seafoodWildOrFarmRaised;
            bulkImportItemModel.VeganAgencyId = veganAgencyId;
            bulkImportItemModel.Vegetarian = vegetarian;
            bulkImportItemModel.WholeTrade = wholeTrade;
            bulkImportItemModel.GrassFed = grassFed;
            bulkImportItemModel.PastureRaised = pastureRaised;
            bulkImportItemModel.FreeRange = freeRange;
            bulkImportItemModel.DryAged = dryAged;
            bulkImportItemModel.AirChilled = airChilled;
            bulkImportItemModel.MadeInHouse = madeInHouse;
            bulkImportItemModel.Msc = msc;
            bulkImportItemModel.AlcoholByVolume = alcoholByVolume;
            bulkImportItemModel.CaseinFree = caseineFree;
            bulkImportItemModel.DrainedWeight = drainedWeight;
            bulkImportItemModel.DrainedWeightUom = drainedWeightUom;
            bulkImportItemModel.FairTradeCertified = fairTradeCertified;
            bulkImportItemModel.Hemp = hemp;
            bulkImportItemModel.LocalLoanProducer = localeLoanProducer;
            bulkImportItemModel.MainProductName = mainProductName;
            bulkImportItemModel.NutritionRequired = nutritionRequried;
            bulkImportItemModel.OrganicPersonalCare = organicPersonalCare;
            bulkImportItemModel.Paleo = paleo;
            bulkImportItemModel.ProductFlavorType = productFlavorType;

            return bulkImportItemModel;
        }

        public static implicit operator BulkImportItemModel(TestBulkImportItemModelBuilder builder)
        {
            return builder.Build();
        }

        private string GetItemTrait(Item item, int traitId)
        {
            if (item.ItemTrait == null)
            {
                return String.Empty;
            }

            return item.ItemTrait.FirstOrDefault(it => it.traitID == traitId) == null ? String.Empty : item.ItemTrait.First(it => it.traitID == traitId).traitValue;
        }

        /// <summary>
        /// Returns the Item's associated HierarchyClass' ID and Name given a Hierarchy ID.
        /// </summary>
        /// <param name="item">The Item to get the associated HierarchyClass from.</param>
        /// <param name="hierarchyId">The Hierarchy ID of the associated HierarchyClass to search for.</param>
        /// <returns>A Tuple with HierarchyClassId as the first value and HierarchyClassName as the second value or a Tuple with two empty strings if no matching HierarchyClass is found.</returns>
        private Tuple<string, string> GetItemHierarchyClass(Item item, int hierarchyId)
        {
            if (item.ItemHierarchyClass != null)
            {
                var hierarchyClasses = item.ItemHierarchyClass.Select(ihc => ihc.HierarchyClass);

                if (hierarchyClasses.Any())
                {
                    var hierarchyClass = hierarchyClasses.FirstOrDefault(hc => hc.hierarchyID == hierarchyId);

                    if (hierarchyClass != null)
                    {
                        //Returns a Tuple with HierarchyClassId as the first value and HierarchyClassName as the second value
                        return new Tuple<string, string>(hierarchyClass.hierarchyClassID.ToString(), hierarchyClass.hierarchyClassName);
                    }
                }
            }

            //Return a Tuble of empty strings when no matching hierarchy Class can be found
            return new Tuple<string, string>(String.Empty, String.Empty);
        }

        public TestBulkImportItemModelBuilder WithCaseineFree(string caseineFree)
        {
            this.caseineFree = caseineFree;
            return this;
        }

        public TestBulkImportItemModelBuilder WithAlcoholByVolume(string alcoholByVolume)
        {
            this.alcoholByVolume = alcoholByVolume;
            return this;
        }

        public TestBulkImportItemModelBuilder WithDrainedWeight(string drainedWeight)
        {
            this.drainedWeight = drainedWeight;
            return this;
        }

        public TestBulkImportItemModelBuilder WithDrainedWeightUom(string drainedWeightUom)
        {
            this.drainedWeightUom = drainedWeightUom;
            return this;
        }

        public TestBulkImportItemModelBuilder WithFairTradeCertified(string fairTradeCertified)
        {
            this.fairTradeCertified = fairTradeCertified;
            return this;
        }

        public TestBulkImportItemModelBuilder WithHemp(string hemp)
        {
            this.hemp = hemp;
            return this;
        }

        public TestBulkImportItemModelBuilder WithLocalLoanProducer(string localeLoanProducer)
        {
            this.localeLoanProducer = localeLoanProducer;
            return this;
        }

        public TestBulkImportItemModelBuilder WithMainProductName(string mainProductName)
        {
            this.mainProductName = mainProductName;
            return this;
        }

        public TestBulkImportItemModelBuilder WithNutritionRequired(string nutritionRequried)
        {
            this.nutritionRequried = nutritionRequried;
            return this;
        }

        public TestBulkImportItemModelBuilder WithOrganicPersonalCare(string organicPersonalCare)
        {
            this.organicPersonalCare = organicPersonalCare;
            return this;
        }

        public TestBulkImportItemModelBuilder WithProductFlavorType(string productFlavorType)
        {
            this.productFlavorType = productFlavorType;
            return this;
        }

        public TestBulkImportItemModelBuilder WithPaleo(string paleo)
        {
            this.paleo = paleo;
            return this;
        }
    }
}
