using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class ValidateItemsCommandHandlerTests
    {
        private ValidateItemsCommandHandler commandHandler;
        private ValidateItemsCommand command;
        private Mock<IRenewableContext<IconContext>> mockGlobalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private ItemModel testItem;
        private HierarchyClass testBrand;
        private HierarchyClass testSubTeam;
        private HierarchyClass testSubBrick;
        private HierarchyClass testNationalClass;
        private HierarchyClass testTax;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockGlobalContext = new Mock<IRenewableContext<IconContext>>();
            mockGlobalContext.SetupGet(m => m.Context).Returns(context);

            commandHandler = new ValidateItemsCommandHandler(mockGlobalContext.Object);
            command = new ValidateItemsCommand();

            testItem = new ItemModel { ItemId = 1234 };
            command.Items = new List<ItemModel> { testItem };
            SetupTestItems();
        }

        private void SetupTestItems()
        {
            testBrand = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test Brand", hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Brand });
            testSubTeam = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test Sub Team (1234)", hierarchyID = Hierarchies.Financial, hierarchyLevel = HierarchyLevels.Financial });
            testSubBrick = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test Sub Brick", hierarchyID = Hierarchies.Merchandise, hierarchyLevel = HierarchyLevels.SubBrick });
            testNationalClass = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "Test National Class", hierarchyID = Hierarchies.National, hierarchyLevel = HierarchyLevels.NationalClass });
            testTax = context.HierarchyClass.Add(new HierarchyClass { hierarchyClassName = "0123456 Test Tax", hierarchyID = Hierarchies.Tax, hierarchyLevel = HierarchyLevels.Tax });
            context.SaveChanges();

            testItem.BrandsHierarchyClassId = testBrand.hierarchyClassID.ToString();
            testItem.FinancialHierarchyClassId = "1234";
            testItem.MerchandiseHierarchyClassId = testSubBrick.hierarchyClassID.ToString();
            testItem.NationalHierarchyClassId = testNationalClass.hierarchyClassID.ToString();
            testItem.TaxHierarchyClassId = "0123456";
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void ValidateItems_AllHierarchyClassesExist_NoError()
        {
            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(null, testItem.ErrorCode);
            Assert.AreEqual(null, testItem.ErrorDetails);
        }

        [TestMethod]
        public void ValidateItems_BrandDoesNotExist_NonExistentBrandError()
        {
            //Given
            context.HierarchyClass.Remove(testBrand);
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorDetails = "No Brand exists in Icon with a hierarchy class ID '{PropertyValue}'.".GetFormattedValidationMessage("", testItem.BrandsHierarchyClassId.ToString());
            Assert.AreEqual("NonExistentBrand", testItem.ErrorCode);
            Assert.AreEqual(expectedErrorDetails, testItem.ErrorDetails);
        }

        [TestMethod]
        public void ValidateItems_SubTeamDoesNotExist_NonExistentSubTeamError()
        {
            //Given
            context.HierarchyClass.Remove(testSubTeam);
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorDetails = "No Sub Team exists in Icon with an sub team code '{PropertyValue}'.".GetFormattedValidationMessage("", testItem.FinancialHierarchyClassId);
            Assert.AreEqual("NonExistentSubTeam", testItem.ErrorCode);
            Assert.AreEqual(expectedErrorDetails, testItem.ErrorDetails);
        }

        [TestMethod]
        public void ValidateItems_SubBrickDoesNotExist_NonExistentSubBrickError()
        {
            //Given
            context.HierarchyClass.Remove(testSubBrick);
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorDetails = "No Sub Brick exists in Icon with a hierarchy class ID '{PropertyValue}'.".GetFormattedValidationMessage("", testItem.MerchandiseHierarchyClassId.ToString());
            Assert.AreEqual("NonExistentSubBrick", testItem.ErrorCode);
            Assert.AreEqual(expectedErrorDetails, testItem.ErrorDetails);
        }

        [TestMethod]
        public void ValidateItems_TaxDoesNotExist_NonExistentTaxError()
        {
            //Given
            context.HierarchyClass.Remove(testTax);
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorDetails = "No Tax Class exists in Icon with a tax code '{PropertyValue}'.".GetFormattedValidationMessage("", testItem.TaxHierarchyClassId);
            Assert.AreEqual("NonExistentTax", testItem.ErrorCode);
            Assert.AreEqual(expectedErrorDetails, testItem.ErrorDetails);
        }

        [TestMethod]
        public void ValidateItems_NationalClassDoesNotExists_NonExistentNationalClassError()
        {
            //Given
            context.HierarchyClass.Remove(testNationalClass);
            context.SaveChanges();

            //When
            commandHandler.Execute(command);

            //Then
            string expectedErrorDetails = "No National Class exists in Icon with a hierarchy class ID '{PropertyValue}'.".GetFormattedValidationMessage("", testItem.NationalHierarchyClassId.ToString());
            Assert.AreEqual("NonExistentNationalClass", testItem.ErrorCode);
            Assert.AreEqual(expectedErrorDetails, testItem.ErrorDetails);
        }
    }
}
