using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using System.Data.Entity;
using GlobalEventController.Common;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Tests.Common
{
    [TestClass]
    public class ValidatedItemModelTests
    {
        private ValidatedItemModel model;
        private IconContext context;
        private DbContextTransaction transaction;

        private string validationDate;
        private string productDescription;
        private string posDescription;
        private string packageUnit;
        private string foodStampEligible;
        private string posScaleTare;
        private string taxAbbreviation;
        private string subTeamName;
        private string nationalClassCode;
        private string retailSize;
        private string retailUom;
        private ScanCode scanCode;
        private HierarchyClass brand;
        private HierarchyClass tax;
        private HierarchyClass merchandise;
        private HierarchyClass national;
        private HierarchyClass glutenAgency;
        private HierarchyClass kosherAgency;
        private HierarchyClass nonGmoAgency;
        private HierarchyClass organicAgency;
        private HierarchyClass veganAgency;
        private ItemSignAttribute signAttributes;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void Constructor_GivenAScanCodeWithAllItemAttributes_ShouldPopulateAllModelProperties()
        {
            //Given
            SetupScanCode(true);

            //When
            model = new ValidatedItemModel(scanCode);

            //Then
            Assert.AreEqual(scanCode.itemID, model.ItemId);
            Assert.AreEqual(scanCode.scanCode, model.ScanCode);
            Assert.AreEqual(scanCode.ScanCodeType.scanCodeTypeDesc, model.ScanCodeType);
            Assert.AreEqual(validationDate, model.ValidationDate);
            Assert.AreEqual(productDescription, model.ProductDescription);
            Assert.AreEqual(posDescription, model.PosDescription);
            Assert.AreEqual(packageUnit, model.PackageUnit);
            Assert.AreEqual(foodStampEligible, model.FoodStampEligible);
            Assert.AreEqual(posScaleTare, model.Tare);
            Assert.AreEqual(brand.hierarchyClassID, model.BrandId);
            Assert.AreEqual(brand.hierarchyClassName, model.BrandName);
            Assert.AreEqual(taxAbbreviation, model.TaxClassName);
            Assert.AreEqual(subTeamName, model.SubTeamName);
            Assert.AreEqual(nationalClassCode, model.NationalClassCode);
            Assert.IsTrue(model.HasItemSignAttributes);
            Assert.AreEqual(AnimalWelfareRatings.Descriptions.Step1, model.AnimalWelfareRating);
            Assert.IsTrue(model.Biodynamic.Value);
            Assert.AreEqual(MilkTypes.Descriptions.CowGoatSheepMilk, model.CheeseMilkType);
            Assert.IsTrue(model.CheeseRaw.Value);
            Assert.AreEqual(EcoScaleRatings.Descriptions.PremiumYellow, model.EcoScaleRating);
            Assert.IsTrue(model.GlutenFree.Value);
            Assert.IsTrue(model.Kosher.Value);
            Assert.IsTrue(model.Msc.Value);
            Assert.IsTrue(model.NonGmo.Value);
            Assert.IsTrue(model.Organic.Value);
            Assert.IsTrue(model.PremiumBodyCare.Value);
            Assert.AreEqual(SeafoodFreshOrFrozenTypes.Descriptions.Frozen, model.FreshOrFrozen);
            Assert.AreEqual(SeafoodCatchTypes.Descriptions.Wild, model.SeafoodCatchType);
            Assert.IsTrue(model.Vegan.Value);
            Assert.IsTrue(model.Vegetarian.Value);
            Assert.IsTrue(model.WholeTrade.Value);
            Assert.IsTrue(model.GrassFed.Value);
            Assert.IsTrue(model.PastureRaised.Value);
            Assert.IsTrue(model.FreeRange.Value);
            Assert.IsTrue(model.DryAged.Value);
            Assert.IsTrue(model.AirChilled.Value);
            Assert.IsTrue(model.MadeInHouse.Value);
            Assert.AreEqual(Convert.ToDecimal(retailSize), model.RetailSize);
            Assert.AreEqual(retailUom, model.RetailUom);
        }

        [TestMethod]
        public void Constructor_GivenAScanCodeWithNoSignAttributes_SignAttributesShouldBeNull()
        {
            //Given
            SetupScanCode(false);

            //When
            model = new ValidatedItemModel(scanCode);

            //Then
            Assert.AreEqual(scanCode.itemID, model.ItemId);
            Assert.AreEqual(scanCode.scanCode, model.ScanCode);
            Assert.AreEqual(scanCode.ScanCodeType.scanCodeTypeDesc, model.ScanCodeType);
            Assert.AreEqual(validationDate, model.ValidationDate);
            Assert.AreEqual(productDescription, model.ProductDescription);
            Assert.AreEqual(posDescription, model.PosDescription);
            Assert.AreEqual(packageUnit, model.PackageUnit);
            Assert.AreEqual(foodStampEligible, model.FoodStampEligible);
            Assert.AreEqual(posScaleTare, model.Tare);
            Assert.AreEqual(brand.hierarchyClassID, model.BrandId);
            Assert.AreEqual(brand.hierarchyClassName, model.BrandName);
            Assert.AreEqual(taxAbbreviation, model.TaxClassName);
            Assert.AreEqual(subTeamName, model.SubTeamName);
            Assert.AreEqual(nationalClassCode, model.NationalClassCode);
            Assert.IsFalse(model.HasItemSignAttributes);
            Assert.IsNull(model.AnimalWelfareRating);
            Assert.IsNull(model.Biodynamic);
            Assert.IsNull(model.CheeseMilkType);
            Assert.IsNull(model.CheeseRaw);
            Assert.IsNull(model.EcoScaleRating);
            Assert.IsNull(model.GlutenFree);
            Assert.IsNull(model.Kosher);
            Assert.IsNull(model.Msc);
            Assert.IsNull(model.NonGmo);
            Assert.IsNull(model.Organic);
            Assert.IsNull(model.PremiumBodyCare);
            Assert.IsNull(model.FreshOrFrozen);
            Assert.IsNull(model.SeafoodCatchType);
            Assert.IsNull(model.Vegan);
            Assert.IsNull(model.Vegetarian);
            Assert.IsNull(model.WholeTrade);
            Assert.IsNull(model.GrassFed);
            Assert.IsNull(model.PastureRaised);
            Assert.IsNull(model.FreeRange);
            Assert.IsNull(model.DryAged);
            Assert.IsNull(model.AirChilled);
            Assert.IsNull(model.MadeInHouse);
        }

        [TestMethod]
        public void Constructor_GivenAScanCodeWithFalseSignAttributes_SignAttributesShouldHaveFalseValues()
        {
            //Given
            SetupScanCode(false);

            signAttributes = new ItemSignAttribute
            {
                AnimalWelfareRatingId = null,
                Biodynamic = false,
                CheeseMilkTypeId = null,
                CheeseRaw = false,
                EcoScaleRatingId = null,
                GlutenFreeAgencyId = null,
                KosherAgencyId = null,
                Msc = false,
                NonGmoAgencyId = null,
                OrganicAgencyId = null,
                PremiumBodyCare = false,
                SeafoodFreshOrFrozenId = null,
                SeafoodCatchTypeId = null,
                VeganAgencyId = null,
                Vegetarian = false,
                WholeTrade = false,
                GrassFed = false,
                PastureRaised = false,
                FreeRange = false,
                DryAged = false,
                AirChilled = false,
                MadeInHouse = false
            };
            scanCode.Item.ItemSignAttribute.Add(signAttributes);

            context.SaveChanges();

            //When
            model = new ValidatedItemModel(scanCode);

            //Then
            Assert.AreEqual(scanCode.itemID, model.ItemId);
            Assert.AreEqual(scanCode.scanCode, model.ScanCode);
            Assert.AreEqual(scanCode.ScanCodeType.scanCodeTypeDesc, model.ScanCodeType);
            Assert.AreEqual(validationDate, model.ValidationDate);
            Assert.AreEqual(productDescription, model.ProductDescription);
            Assert.AreEqual(posDescription, model.PosDescription);
            Assert.AreEqual(packageUnit, model.PackageUnit);
            Assert.AreEqual(foodStampEligible, model.FoodStampEligible);
            Assert.AreEqual(posScaleTare, model.Tare);
            Assert.AreEqual(brand.hierarchyClassID, model.BrandId);
            Assert.AreEqual(brand.hierarchyClassName, model.BrandName);
            Assert.AreEqual(taxAbbreviation, model.TaxClassName);
            Assert.AreEqual(subTeamName, model.SubTeamName);
            Assert.AreEqual(nationalClassCode, model.NationalClassCode);
            Assert.IsTrue(model.HasItemSignAttributes);
            Assert.IsNull(model.AnimalWelfareRating);
            Assert.IsFalse(model.Biodynamic.Value);
            Assert.IsNull(model.CheeseMilkType);
            Assert.IsFalse(model.CheeseRaw.Value);
            Assert.IsNull(model.EcoScaleRating);
            Assert.IsNull(model.GlutenFree);
            Assert.IsNull(model.Kosher);
            Assert.IsFalse(model.Msc.Value);
            Assert.IsNull(model.NonGmo);
            Assert.IsNull(model.Organic);
            Assert.IsFalse(model.PremiumBodyCare.Value);
            Assert.IsNull(model.FreshOrFrozen);
            Assert.IsNull(model.SeafoodCatchType);
            Assert.IsNull(model.Vegan);
            Assert.IsFalse(model.Vegetarian.Value);
            Assert.IsFalse(model.WholeTrade.Value);
            Assert.IsFalse(model.GrassFed.Value);
            Assert.IsFalse(model.PastureRaised.Value);
            Assert.IsFalse(model.FreeRange.Value);
            Assert.IsFalse(model.DryAged.Value);
            Assert.IsFalse(model.AirChilled.Value);
            Assert.IsFalse(model.MadeInHouse.Value);
        }

        private void SetupScanCode(bool addItemSignAttributes)
        {
            validationDate = DateTime.Now.ToString();
            productDescription = "Test Product Description";
            posDescription = "Test POS Description";
            packageUnit = "Test Package Unit";
            foodStampEligible = "Test FSE";
            posScaleTare = "Test Tare";
            taxAbbreviation = "Test Tax Abbr";
            subTeamName = "Test SubTeam";
            nationalClassCode = "5";
            retailSize = "1";
            retailUom = "EA";

            brand = new HierarchyClass { hierarchyID = Hierarchies.Brands, hierarchyClassName = "Test Brand" };
            tax = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "Test Tax",
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.TaxAbbreviation, traitValue = taxAbbreviation }
                }
            };
            merchandise = new HierarchyClass
            {
                hierarchyID = Hierarchies.Merchandise,
                hierarchyClassName = "Test Merchandise",
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.MerchFinMapping, traitValue = subTeamName }
                }
            };
            national = new HierarchyClass
            {
                hierarchyID = Hierarchies.National,
                hierarchyClassName = "Test National",
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.NationalClassCode, traitValue = nationalClassCode }
                }
            };

            glutenAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Test Gluten" };
            kosherAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Kosher Agency" };
            nonGmoAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "NonGmo Agency" };
            organicAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Organic Agency" };
            veganAgency = new HierarchyClass { hierarchyID = Hierarchies.CertificationAgencyManagement, hierarchyClassName = "Vegan Agency" };

            context.HierarchyClass.AddRange(new List<HierarchyClass> { brand, tax, merchandise, national, glutenAgency, kosherAgency, nonGmoAgency, organicAgency, veganAgency });
            context.SaveChanges();

            scanCode = new ScanCode
            {
                scanCode = "123498765",
                ScanCodeType = new ScanCodeType { scanCodeTypeDesc = "Test ScanCode Type Description" },
                Item = new Item
                {
                    itemTypeID = ItemTypes.RetailSale,
                    ItemTrait = new List<ItemTrait>
                    {
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.ValidationDate, traitValue = validationDate },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.ProductDescription, traitValue = productDescription },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PosDescription, traitValue = posDescription },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PackageUnit, traitValue = packageUnit },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.FoodStampEligible, traitValue = foodStampEligible },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.PosScaleTare, traitValue = posScaleTare },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.RetailSize, traitValue = retailSize },
                        new ItemTrait { localeID = Locales.WholeFoods, traitID = Traits.RetailUom, traitValue = retailUom },
                    },
                    ItemHierarchyClass = new List<ItemHierarchyClass>
                    {
                        new ItemHierarchyClass { hierarchyClassID = brand.hierarchyClassID },
                        new ItemHierarchyClass { hierarchyClassID = tax.hierarchyClassID },
                        new ItemHierarchyClass { hierarchyClassID = merchandise.hierarchyClassID },
                        new ItemHierarchyClass { hierarchyClassID = national.hierarchyClassID }
                    }
                }
            };

            if (addItemSignAttributes)
            {
                signAttributes = new ItemSignAttribute
                {
                    AnimalWelfareRatingId = AnimalWelfareRatings.Step1,
                    Biodynamic = true,
                    CheeseMilkTypeId = MilkTypes.CowGoatSheepMilk,
                    CheeseRaw = true,
                    EcoScaleRatingId = EcoScaleRatings.PremiumYellow,
                    GlutenFreeAgencyId = glutenAgency.hierarchyClassID,
                    KosherAgencyId = kosherAgency.hierarchyClassID,
                    Msc = true,
                    NonGmoAgencyId = nonGmoAgency.hierarchyClassID,
                    OrganicAgencyId = organicAgency.hierarchyClassID,
                    PremiumBodyCare = true,
                    SeafoodFreshOrFrozenId = SeafoodFreshOrFrozenTypes.Frozen,
                    SeafoodCatchTypeId = SeafoodCatchTypes.Wild,
                    VeganAgencyId = veganAgency.hierarchyClassID,
                    Vegetarian = true,
                    WholeTrade = true,
                    GrassFed = true,
                    PastureRaised = true,
                    FreeRange = true,
                    DryAged = true,
                    AirChilled = true,
                    MadeInHouse = true
                };
                scanCode.Item.ItemSignAttribute.Add(signAttributes);
            }
            context.ScanCode.Add(scanCode);
            context.SaveChanges();
        }
    }
}
