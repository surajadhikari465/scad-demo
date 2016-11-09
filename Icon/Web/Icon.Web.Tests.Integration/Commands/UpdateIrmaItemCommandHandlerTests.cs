using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using Icon.Testing.Builders;
using System.Collections.Generic;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateIrmaItemCommandHandlerTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private int testItemId;
        private UpdateIrmaItemCommandHandler updateIrmaItemCommandHandler;
        private Mock<ILogger> mockLogger;
        private HierarchyClass testGlutenFree;
        private HierarchyClass testKosher;
        private HierarchyClass testNonGmo;
        private HierarchyClass testOrganic;
        private HierarchyClass testVegan;
        private string testGlutenFreeAgencyName;
        private string testKosherAgencyName;
        private string testNonGmoAgencyName;
        private string testOrganicAgencyName;
        private string testVeganAgencyName;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            // Setup a new item in the IRMAItem table for use in the tests.
            IRMAItem testItem = new IRMAItem
            {
                regioncode = "MA",
                identifier = "424242424242",
                itemDescription = "UpdateIrma Test Item Desc 1",
                posDescription = "UpdateIrma Test Item PosDesc 1",
                packageUnit = 6,
                retailSize = 16,
                retailUom = "FLUID OUNCES",
                DeliverySystem = "CAP",
                brandName = "UpdateIrma Test Brand",
                foodStamp = true,
                posScaleTare = 0,
                departmentSale = true,
                insertDate = DateTime.Now
            };
            transaction = context.Database.BeginTransaction();
            context.IRMAItem.Add(testItem);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = String.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = String.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            testGlutenFreeAgencyName = "GF Agency";
            testKosherAgencyName = "KS Agency";
            testNonGmoAgencyName = "GMO Agency";
            testOrganicAgencyName = "OG Agency";
            testVeganAgencyName = "VG Agency";
            testGlutenFree = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testGlutenFreeAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testKosher = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testKosherAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testNonGmo = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testNonGmoAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testOrganic = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testOrganicAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);

            testVegan = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName(testVeganAgencyName)
                .WithHierarchyLevel(HierarchyLevels.CertificationAgencyManagement);
            var gluteen = context.HierarchyClass.FirstOrDefault();
          

            context.HierarchyClass.AddRange(new List<HierarchyClass> 
                { 
                    testGlutenFree, testKosher, testNonGmo, testOrganic, testVegan
                });

            context.SaveChanges();

            
            testItemId = testItem.irmaItemID;

            mockLogger = new Mock<ILogger>();
            updateIrmaItemCommandHandler = new UpdateIrmaItemCommandHandler(this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                transaction.Rollback();
                updateIrmaItemCommandHandler = null;
                mockLogger = null;
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_ItemDescriptionUpdates_SuccessfulUpdate()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "New Modified Update Irma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 6,
                RetailSize = 0,
                RetailUom = String.Empty,
                DeliverySystem = String.Empty,
                BrandName = "UpdateIrma Test Brand",
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            string expectedDescription = updatedItem.ItemDescription;
            string actualDescription = verifyUpdateItem.itemDescription;
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_PosDescriptionUpdates_SuccessfulUpdate()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "New UpdateIrma Test Modified PosDesc 1",
                PackageUnit = 6,
                RetailSize = 0,
                RetailUom = String.Empty,
                DeliverySystem = String.Empty,
                BrandName = "UpdateIrma Test Brand",
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            string expectedDescription = updatedItem.PosDescription;
            string actualDescription = verifyUpdateItem.posDescription;
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_PackageUnitUpdates_SuccessfulUpdate()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 3,
                RetailSize = 0,
                RetailUom = String.Empty,
                DeliverySystem = String.Empty,
                BrandName = "UpdateIrma Test Brand",
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            int expectedPackageUnit = updatedItem.PackageUnit;
            int actualPackageUnit = verifyUpdateItem.packageUnit;
            Assert.AreEqual(expectedPackageUnit, actualPackageUnit);
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_RetailSizeUpdates_SuccessfulUpdate()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 3,
                RetailSize = 11,
                RetailUom = "OUNCES",
                DeliverySystem = "LZ",
                BrandName = "UpdateIrma Test Brand",
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            decimal? expectedRetailSize = updatedItem.RetailSize;
            decimal? actualRetailSize = verifyUpdateItem.retailSize;
            Assert.AreEqual(expectedRetailSize, actualRetailSize, "Retail Sizes do not match.");
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_RetailUomUpdates_SuccessfulUpdate()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 3,
                RetailSize = 11,
                RetailUom = "OUNCES",
                DeliverySystem = "LZ",
                BrandName = "UpdateIrma Test Brand",
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            string expectedRetailUom = updatedItem.RetailUom;
            string actualRetailUom = verifyUpdateItem.retailUom;
            Assert.AreEqual(expectedRetailUom, actualRetailUom, "Retail UOMs do not match.");
        }

        public void UpdateIrmaItemCommandHandlerExecute_DeliverySystemUpdates_SuccessfulUpdate()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 3,
                RetailSize = 11,
                RetailUom = "OUNCES",
                DeliverySystem = "LZ",
                BrandName = "UpdateIrma Test Brand",
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            string expectedDeliverySystem = updatedItem.DeliverySystem;
            string actualDeliverySystem = verifyUpdateItem.DeliverySystem;
            Assert.AreEqual(expectedDeliverySystem, actualDeliverySystem, "Delivery Systems do not match.");
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_FoodStampUpdates_FoodStampValueChanged()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 6,
                RetailSize = 0,
                RetailUom = String.Empty,
                BrandName = "UpdateIrma Test Brand",
                FoodStampEligible = false
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            bool expectedFoodStamp = updatedItem.FoodStampEligible;
            bool actualFoodStamp = verifyUpdateItem.foodStamp;
            Assert.AreEqual(expectedFoodStamp, actualFoodStamp);
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_PosScaleTareUpdates_TareValueChanged()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 6,
                RetailSize = 0,
                RetailUom = String.Empty,
                BrandName = "UpdateIrma Test Brand",
                PosScaleTare = 20
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            decimal expected = updatedItem.PosScaleTare;
            decimal actual = verifyUpdateItem.posScaleTare;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_AddTaxClass_TaxClassIdAdded()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 3,
                RetailSize = 0,
                RetailUom = String.Empty,
                BrandName = "UpdateIrma Test Brand",
                TaxHierarchyClassId = 11
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            int expectedTaxClassId = updatedItem.TaxHierarchyClassId.Value;
            int actualTaxClassId = verifyUpdateItem.taxClassID.Value;
            Assert.AreEqual(expectedTaxClassId, actualTaxClassId);
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_AddMerchClass_MerchClassIdAdded()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 3,
                RetailSize = 0,
                RetailUom = String.Empty,
                BrandName = "UpdateIrma Test Brand",
                MerchandiseHierarchyClassId = 11
            };

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            int expectedMerchClassId = updatedItem.MerchandiseHierarchyClassId.Value;
            int actualMerchClassId = verifyUpdateItem.merchandiseClassID.Value;
            Assert.AreEqual(expectedMerchClassId, actualMerchClassId);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void UpdateIrmaItemCommandHandlerExecute_UpdateFailed_ExceptionThrown()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "New Test Item Desc 1 New Test Item Desc 1 New Test Item Desc 1",
                PosDescription = "UpdateIrma Test Item PosDesc 1",
                PackageUnit = 3,
                BrandName = "UpdateIrma Test Brand",
            };

            // When & Then - Exception Expected.
            updateIrmaItemCommandHandler.Execute(updatedItem);
        }

        [TestMethod]
        public void UpdateIrmaItemCommandHandlerExecute_SignAttributeUpdates_SuccessfulUpdate()
        {
            // Given.
            UpdateIrmaItemCommand updatedItem = new UpdateIrmaItemCommand
            {
                IrmaItemId = testItemId,
                ItemDescription = "UpdateIrma Test Item Desc 1",
                PosDescription = "New UpdateIrma Test Modified PosDesc 1",
                PackageUnit = 6,
                RetailSize = 0,
                RetailUom = String.Empty,
                BrandName = "UpdateIrma Test Brand",
            };
            updatedItem.AnimalWelfareRatingId = AnimalWelfareRatings.AsDictionary.First().Key;
            updatedItem.CheeseMilkTypeId = MilkTypes.AsDictionary.First().Key;
            updatedItem.EcoScaleRatingId = EcoScaleRatings.AsDictionary.First().Key;
            updatedItem.SeafoodFreshOrFrozenId = SeafoodFreshOrFrozenTypes.AsDictionary.First().Key;
            updatedItem.SeafoodCatchTypeId = SeafoodCatchTypes.AsDictionary.First().Key;
            
            updatedItem.GlutenFreeAgencyId = testGlutenFree.hierarchyClassID;
            updatedItem.KosherAgencyId = testKosher.hierarchyClassID;
            updatedItem.NonGmoAgencyId = testNonGmo.hierarchyClassID;
            updatedItem.OrganicAgencyId = testOrganic.hierarchyClassID;
            updatedItem.VeganAgencyId = testVegan.hierarchyClassID;
            updatedItem.Biodynamic = true;
            updatedItem.Vegetarian = true;
            updatedItem.WholeTrade = true;
            updatedItem.PremiumBodyCare = true;
            updatedItem.CheeseRaw = true;

            // When.
            updateIrmaItemCommandHandler.Execute(updatedItem);

            // Then.
            IRMAItem verifyUpdateItem = context.IRMAItem.Find(testItemId);
            string expectedDescription = updatedItem.PosDescription;
            string actualDescription = verifyUpdateItem.posDescription;
            
            Assert.AreEqual(updatedItem.AnimalWelfareRatingId, verifyUpdateItem.AnimalWelfareRatingId);
            Assert.IsTrue(verifyUpdateItem.Biodynamic.Value);
            Assert.AreEqual(updatedItem.CheeseMilkTypeId, verifyUpdateItem.CheeseMilkTypeId);
            Assert.IsTrue(verifyUpdateItem.CheeseRaw.Value);
            Assert.AreEqual(updatedItem.EcoScaleRatingId, verifyUpdateItem.EcoScaleRatingId);
            Assert.AreEqual(updatedItem.GlutenFreeAgencyId, verifyUpdateItem.GlutenFreeAgencyId);
            Assert.AreEqual(updatedItem.KosherAgencyId, verifyUpdateItem.KosherAgencyId);
            Assert.AreEqual(updatedItem.NonGmoAgencyId, verifyUpdateItem.NonGmoAgencyId);
            Assert.AreEqual(updatedItem.OrganicAgencyId, verifyUpdateItem.OrganicAgencyId);
            Assert.IsTrue(verifyUpdateItem.PremiumBodyCare.Value);
            Assert.AreEqual(updatedItem.SeafoodFreshOrFrozenId, verifyUpdateItem.SeafoodFreshOrFrozenId);
            Assert.AreEqual(updatedItem.SeafoodCatchTypeId, verifyUpdateItem.SeafoodCatchTypeId);
            Assert.AreEqual(updatedItem.VeganAgencyId, verifyUpdateItem.VeganAgencyId);
            Assert.IsTrue(verifyUpdateItem.Vegetarian.Value);
            Assert.IsTrue(verifyUpdateItem.WholeTrade.Value);
        }

    }
}
