using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.NewItem.Commands;
using Icon.Framework;
using System.Data.Entity;
using Services.NewItem.Cache;
using Moq;
using Services.NewItem.Models;
using System.Linq;
using Icon.Common.Context;

namespace Services.NewItem.Tests.Commands
{
    /// <summary>
    /// Summary description for AddNewItemsToIconCommandHandlerTests
    /// </summary>
    [TestClass]
    public class UpdateItemSubscriptionInIconCommandHandlerTests
    {
        private UpdateItemSubscriptionInIconCommandHandler commandHandler;
        private UpdateItemSubscriptionInIconCommand command;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<IIconCache> mockIconCache;
        private Mock<IRenewableContext<IconContext>> mockRenewableContext;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockRenewableContext = new Mock<IRenewableContext<IconContext>>();
            mockRenewableContext.SetupGet(m => m.Context).Returns(context);

            mockIconCache = new Mock<IIconCache>();
            commandHandler = new UpdateItemSubscriptionInIconCommandHandler(mockRenewableContext.Object, mockIconCache.Object);
            command = new UpdateItemSubscriptionInIconCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void UpdateItemSubscriptionInIcon_NoNewItemsGiven_ShouldNotTryToInsertNewItems()
        {
            //Given
            Mock<IEnumerable<NewItemModel>> mockNewItems = new Mock<IEnumerable<NewItemModel>>();
            mockNewItems.Setup(m => m.GetEnumerator())
                .Returns(new List<NewItemModel>().GetEnumerator());
            command.NewItems = mockNewItems.Object;

            //When
            commandHandler.Execute(command);

            //Then
            mockNewItems.Verify(m => m.GetEnumerator(), Times.Once);
            mockIconCache.VerifyGet(m => m.TaxClassCodesToIdDictionary, Times.Never);
            mockIconCache.VerifyGet(m => m.NationalClassCodesToIdDictionary, Times.Never);
        }

        [TestMethod]
        public void UpdateItemSubscriptionInIcon_NoNewItemsOrSubscriptionsExistForNewItems_ShouldAddNewItemsAndSubscriptions()
        {
            //Given
            int testTaxClassId = 12345;
            int testNationalClassId = 6789;
            mockIconCache.SetupGet(m => m.NationalClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "12345", testNationalClassId } });
            mockIconCache.SetupGet(m => m.TaxClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "1111111", testTaxClassId } });

            var newItem = new NewItemModel
            {
                Region = "FL",
                ScanCode = "123456789011",
                IsDefaultIdentifier = true,
                BrandName = "Test Brand",
                ItemDescription = "Test Item Description",
                PosDescription = "Test POS Description",
                PackageUnit = 1,
                RetailSize = 1.1m,
                RetailUom = "EA",
                FoodStampEligible = true,
                TaxClassCode = "1111111",
                SubTeamName = "Test Sub Team",
                NationalClassCode = "12345"
            };
            command.NewItems = new List<NewItemModel>
            {
                newItem
            };

            if(context.ScanCode.Any(sc => sc.scanCode == newItem.ScanCode))
            {
                throw new InvalidOperationException("ScanCode already exists in Icon. Test is invalid.");
            }

            //When
            commandHandler.Execute(command);

            //Then  
            AssertItemSubscription(newItem);
        }

        [TestMethod]
        public void UpdateItemSubscriptionInIcon_NoNewItemsExistButSubscriptionsExistForNewItems_ShouldAddNewItemsButNotSubscriptions()
        {
            //Given
            int testTaxClassId = 12345;
            int testNationalClassId = 6789;
            mockIconCache.SetupGet(m => m.NationalClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "12345", testNationalClassId } });
            mockIconCache.SetupGet(m => m.TaxClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "1111111", testTaxClassId } });

            var newItem = new NewItemModel
            {
                Region = "FL",
                ScanCode = "123456789011",
                IsDefaultIdentifier = true,
                BrandName = "Test Brand",
                ItemDescription = "Test Item Description",
                PosDescription = "Test POS Description",
                PackageUnit = 1,
                RetailSize = 1.1m,
                RetailUom = "EA",
                FoodStampEligible = true,
                TaxClassCode = "1111111",
                SubTeamName = "Test Sub Team",
                NationalClassCode = "12345"
            };
            command.NewItems = new List<NewItemModel>
            {
                newItem
            };

            context.IRMAItemSubscription.Add(new IRMAItemSubscription
            {
                identifier = newItem.ScanCode,
                regioncode = newItem.Region,
                deleteDate = null,
                insertDate = DateTime.Now
            });
            context.SaveChanges();

            if (context.ScanCode.Any(sc => sc.scanCode == newItem.ScanCode))
            {
                throw new InvalidOperationException("ScanCode already exists in Icon. Test is invalid.");
            }

            //When
            commandHandler.Execute(command);

            //Then  
            AssertItemSubscription(newItem);
        }

        [TestMethod]
        public void UpdateItemSubscriptionInIcon_NewItemsExistButSubscriptionsDoNotExistForNewItems_ShouldAddSubscriptionsButNotNewItems()
        {
            //Given
            int testTaxClassId = 12345;
            int testNationalClassId = 6789;
            mockIconCache.SetupGet(m => m.NationalClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "12345", testNationalClassId } });
            mockIconCache.SetupGet(m => m.TaxClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "1111111", testTaxClassId } });

            var newItem = new NewItemModel
            {
                Region = "FL",
                ScanCode = "1234567890",
                IsDefaultIdentifier = true,
                BrandName = "Test Brand",
                ItemDescription = "Test Item Description",
                PosDescription = "Test POS Description",
                PackageUnit = 1,
                RetailSize = 1.1m,
                RetailUom = "EA",
                FoodStampEligible = true,
                TaxClassCode = "1111111",
                SubTeamName = "Test Sub Team",
                NationalClassCode = "12345"
            };
            command.NewItems = new List<NewItemModel>
            {
                newItem
            };

            context.IRMAItem.Add(new IRMAItem
            {
                regioncode = newItem.Region,
                identifier = newItem.ScanCode,
                defaultIdentifier = newItem.IsDefaultIdentifier,
                brandName = newItem.BrandName,
                itemDescription = newItem.ItemDescription,
                posDescription = newItem.PosDescription,
                packageUnit = (int)newItem.PackageUnit,
                retailSize = newItem.RetailSize,
                retailUom = newItem.RetailUom,
                foodStamp = newItem.FoodStampEligible,
                posScaleTare = 0.0m,
                departmentSale = false,
                irmaSubTeamName = newItem.SubTeamName,
                taxClassID = testTaxClassId,
                nationalClassID = testNationalClassId
            });
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then  
            AssertItemSubscription(newItem);
        }

        private void AssertItemSubscription(NewItemModel newItem)
        {
            var irmaItemSubscription = context.IRMAItemSubscription
                .SingleOrDefault(iis => iis.identifier == newItem.ScanCode && iis.regioncode == newItem.Region);
            Assert.IsNotNull(irmaItemSubscription, "IRMA Item Subscription is null.");
            Assert.AreEqual(irmaItemSubscription.identifier, newItem.ScanCode);
            Assert.AreEqual(irmaItemSubscription.regioncode, newItem.Region);
            Assert.IsNull(irmaItemSubscription.deleteDate);
        }
    }
}
