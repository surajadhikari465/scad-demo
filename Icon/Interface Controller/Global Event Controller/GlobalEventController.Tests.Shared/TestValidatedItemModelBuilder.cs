using GlobalEventController.Common;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Tests.Shared
{
    public class TestValidatedItemModelBuilder
    {
        private int itemId;
        private int deptNo;
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
        private string nationalClassCode;
        private int eventTypeId;
        private string animalWelfareRating;
        private bool? biodynamic;
        private string cheeseMilkType;
        private bool? cheeseRaw;
        private string ecoScaleRating;
        private bool? glutenFree;
        private bool? kosher;
        private bool? nonGmo;
        private bool? organic;
        private bool? premiumBodyCare;
        private string freshOrFrozen;
        private string seafoodCatchType;
        private bool? vegan;
        private bool? vegetarian;
        private bool? wholeTrade;
        private bool? msc;
        private bool? grassFed;
        private bool? pastureRaised;
        private bool? freeRange;
        private bool? dryAged;
        private bool? airChilled;
        private bool? madeInHouse;
        private bool hasItemSignAttributes;
        private decimal retailSize;
        private string retailUom;

        public TestValidatedItemModelBuilder()
        {
            this.itemId = 1;
            this.deptNo = 42;
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
            this.eventTypeId = EventTypes.ItemUpdate;
            this.nationalClassCode = "5";
            this.hasItemSignAttributes = false;
            this.retailSize = 13.1M;
            this.retailUom = "OZ";
        }

        public TestValidatedItemModelBuilder WithItemId(int itemId)
        {
            this.itemId = itemId;
            return this;
        }
        public TestValidatedItemModelBuilder WithDeptNo(int deptNo)
        {
            this.deptNo = deptNo;
            return this;
        }

        public TestValidatedItemModelBuilder WithValidationDate(string validationDate)
        {
            this.validationDate = validationDate;
            return this;
        }

        public TestValidatedItemModelBuilder WithScanCode(string scanCode)
        {
            this.scanCode = scanCode;
            return this;
        }

        public TestValidatedItemModelBuilder WithScanCodeType(string scanCodeType)
        {
            this.scanCodeType = scanCodeType;
            return this;
        }

        public TestValidatedItemModelBuilder WithProductDesccription(string productDescription)
        {
            this.productDescription = productDescription;
            return this;
        }

        public TestValidatedItemModelBuilder WithPosDescription(string posDescription)
        {
            this.posDescription = posDescription;
            return this;
        }

        public TestValidatedItemModelBuilder WithPackageUnit(string packageUnit)
        {
            this.packageUnit = packageUnit;
            return this;
        }

        public TestValidatedItemModelBuilder WithFoodStamp(string foodStamp)
        {
            this.foodStamp = foodStamp;
            return this;
        }

        public TestValidatedItemModelBuilder WithTare(string tare)
        {
            this.tare = tare;
            return this;
        }

        public TestValidatedItemModelBuilder WithBrandId(int brandId)
        {
            this.brandId = brandId;
            return this;
        }

        public TestValidatedItemModelBuilder WithBrandName(string brandName)
        {
            this.brandName = brandName;
            return this;
        }

        public TestValidatedItemModelBuilder WithTaxClass(string taxClass)
        {
            this.taxClassName = taxClass;
            return this;
        }

        public TestValidatedItemModelBuilder WithNationalClassCode(string nationalClassCode)
        {
            this.nationalClassCode = nationalClassCode;
            return this;
        }

        public TestValidatedItemModelBuilder WithEventTypeId(int eventTypeId)
        {
            this.eventTypeId = eventTypeId;
            return this;
        }

        public TestValidatedItemModelBuilder WithAnimalWelfareRating(string animalWelfareRating)
        {
            this.animalWelfareRating = animalWelfareRating;
            return this;
        }

        public TestValidatedItemModelBuilder WithBiodynamic(bool? biodynamic)
        {
            this.biodynamic = biodynamic;
            return this;
        }

        public TestValidatedItemModelBuilder WithCheeseMilkType(string cheeseMilkType)
        {
            this.cheeseMilkType = cheeseMilkType;
            return this;
        }

        public TestValidatedItemModelBuilder WithCheeseRaw(bool? cheeseRaw)
        {
            this.cheeseRaw = cheeseRaw;
            return this;
        }

        public TestValidatedItemModelBuilder WithEcoScaleRating(string ecoScaleRating)
        {
            this.ecoScaleRating = ecoScaleRating;
            return this;
        }

        public TestValidatedItemModelBuilder WithGlutenFree(bool? glutenFree)
        {
            this.glutenFree = glutenFree;
            return this;
        }

        public TestValidatedItemModelBuilder WithKosher(bool? kosher)
        {
            this.kosher = kosher;
            return this;
        }

        public TestValidatedItemModelBuilder WithNonGmo(bool? nonGmo)
        {
            this.nonGmo = nonGmo;
            return this;
        }

        public TestValidatedItemModelBuilder WithOrganic(bool? organic)
        {
            this.organic = organic;
            return this;
        }

        public TestValidatedItemModelBuilder WithPremiumBodyCare(bool? premiumBodyCare)
        {
            this.premiumBodyCare = premiumBodyCare;
            return this;
        }

        public TestValidatedItemModelBuilder WithFreshOrFrozen(string freshOrFrozen)
        {
            this.freshOrFrozen = freshOrFrozen;
            return this;
        }

        public TestValidatedItemModelBuilder WithSeafoodCatchType(string seafoodCatchType)
        {
            this.seafoodCatchType = seafoodCatchType;
            return this;
        }

        public TestValidatedItemModelBuilder WithVegan(bool? vegan)
        {
            this.vegan = vegan;
            return this;
        }

        public TestValidatedItemModelBuilder WithVegetarian(bool? vegetarian)
        {
            this.vegetarian = vegetarian;
            return this;
        }

        public TestValidatedItemModelBuilder WithWholeTrade(bool? wholeTrade)
        {
            this.wholeTrade = wholeTrade;
            return this;
        }

        public TestValidatedItemModelBuilder WithMsc(bool? msc)
        {
            this.msc = msc;
            return this;
        }

        public TestValidatedItemModelBuilder WithGrassFed(bool? grassFed)
        {
            this.grassFed = grassFed;
            return this;
        }

        public TestValidatedItemModelBuilder WithPastureRaised(bool? pastureRaised)
        {
            this.pastureRaised = pastureRaised;
            return this;
        }

        public TestValidatedItemModelBuilder WithFreeRange(bool? freeRange)
        {
            this.freeRange = freeRange;
            return this;
        }

        public TestValidatedItemModelBuilder WithDryAged(bool? dryAged)
        {
            this.dryAged = dryAged;
            return this;
        }

        public TestValidatedItemModelBuilder WithAirChilled(bool? airChilled)
        {
            this.airChilled = airChilled;
            return this;
        }

        public TestValidatedItemModelBuilder WithMadeInHouse(bool? madeInHouse)
        {
            this.madeInHouse = madeInHouse;
            return this;
        }

        public TestValidatedItemModelBuilder WithHasItemSignAttributes(bool hasItemSignAttributes)
        {
            this.hasItemSignAttributes = hasItemSignAttributes;
            return this;
        }

        public TestValidatedItemModelBuilder WithRetailSize(decimal retailSize)
        {
            this.retailSize = retailSize;
            return this;
        }

        public TestValidatedItemModelBuilder WithRetailUom(string retailUom)
        {
            this.retailUom = retailUom;
            return this;
        }

        public ValidatedItemModel Build()
        {
            ValidatedItemModel validatedItem = new ValidatedItemModel
            {
                ItemId = this.itemId,
                DeptNo = this.deptNo,
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
                NationalClassCode = this.nationalClassCode,
                EventTypeId = this.eventTypeId,
                HasItemSignAttributes = this.hasItemSignAttributes,
                AirChilled = this.airChilled,
                AnimalWelfareRating = this.animalWelfareRating,
                Biodynamic = this.biodynamic,
                CheeseMilkType = this.cheeseMilkType,
                CheeseRaw = this.cheeseRaw,
                DryAged = this.dryAged,
                EcoScaleRating = this.ecoScaleRating,
                FreeRange = this.freeRange,
                FreshOrFrozen = this.freshOrFrozen,
                GlutenFree = this.glutenFree,
                GrassFed = this.grassFed,
                Kosher = this.kosher,
                MadeInHouse = this.madeInHouse,
                Msc = this.msc,
                NonGmo = this.nonGmo,
                Organic = this.organic,
                PastureRaised = this.pastureRaised,
                PremiumBodyCare = this.premiumBodyCare,
                SeafoodCatchType = this.seafoodCatchType,
                Vegan = this.vegan,
                Vegetarian = this.vegetarian,
                WholeTrade = this.wholeTrade,
                RetailSize = this.retailSize,
                RetailUom = this.retailUom
            };

            return validatedItem;
        }

        public static implicit operator ValidatedItemModel(TestValidatedItemModelBuilder builder)
        {
            return builder.Build();
        }
    }
}
