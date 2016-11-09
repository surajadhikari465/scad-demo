using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalEventController.Controller.EventOperations;
using Icon.Logging;
using Moq;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;
using Icon.Testing.Builders;
using System.Data.Entity;
using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using Irma.Framework;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.DataServices;
using SubteamEventController.Controller.EventServices;
using SubteamEventController.DataAccess.Commands;
using SubteamEventController.DataAccess.Queries;
using SubteamEventController.DataAccess.DataServices;

namespace SubteamEventController.Tests.Controller
{
    [TestClass]
    public class ItemSubTeamEventServiceTests
    {
        private IrmaContext irmaContext;
        private Mock<IQueryHandler<GetScanCodeQuery, List<ScanCode>>> getScanCodeHandler;
        private Mock<IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>> getItemIdentifierHandler;
        private Mock<IDataService<UpdateItemSubTeamDataService>> updateItemServiceHandler;
        private Mock<IQueryHandler<GetSubTeamHierarchyQuery, HierarchyClass>> getSubTeamHierarchyQueryHandlercs;
        private Mock<ICommandHandler<AddItemCategoryCommand>> addItemCategoryCommandHandler;
        private Mock<IQueryHandler<GetUserQuery, Users>> getUserHandler;

        private ItemSubTeamEventService eventService;


        [TestInitialize]
        public void InitializeData()
        {
            irmaContext = new IrmaContext();
            getScanCodeHandler = new Mock<IQueryHandler<GetScanCodeQuery, List<ScanCode>>>();
            getItemIdentifierHandler = new Mock<IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>>();
            updateItemServiceHandler = new Mock<IDataService<UpdateItemSubTeamDataService>>();
            getSubTeamHierarchyQueryHandlercs = new Mock<IQueryHandler<GetSubTeamHierarchyQuery, HierarchyClass>>();
            addItemCategoryCommandHandler = new Mock<ICommandHandler<AddItemCategoryCommand>>();
            getUserHandler = new Mock<IQueryHandler<GetUserQuery, Users>>();
            eventService = new ItemSubTeamEventService(irmaContext, getScanCodeHandler.Object, getItemIdentifierHandler.Object, updateItemServiceHandler.Object, getSubTeamHierarchyQueryHandlercs.Object, addItemCategoryCommandHandler.Object, getUserHandler.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            irmaContext.Dispose();
            Cache.IdentifierToScanCode.Remove("ItemTestScanCode");
        }

        [TestMethod]
        public void ItemSubTeamEventService_ScanCodeMessageNotInScanCodeDictionaryCache_GetScanCodeQueryCalledOneTime()
        {
            //Given
            eventService.Message = "ItemTestScanCode";
            ScanCode scanCode = GetTestScanCode();
            getScanCodeHandler.Setup(h => h.Handle(It.IsAny<GetScanCodeQuery>())).Returns(new List<ScanCode> { scanCode });
            getItemIdentifierHandler.Setup(h => h.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> { });
            updateItemServiceHandler.Setup(h => h.Process(It.IsAny<UpdateItemSubTeamDataService>()));
            getUserHandler.Setup(h => h.Handle(It.IsAny<GetUserQuery>()));
            getSubTeamHierarchyQueryHandlercs.Setup(h => h.Handle(It.IsAny<GetSubTeamHierarchyQuery>())).Returns(new HierarchyClass());

            //When
            eventService.Run();

            //Then           
            getScanCodeHandler.Verify(command => command.Handle(It.IsAny<GetScanCodeQuery>()), Times.Once);
        }

        [TestMethod]
        public void ItemSubTeamEventService_ScanCodeMessageNotInScanCodeCache_ScanCodeAddedToScanCodeDictionaryCache()
        {

            //Given
            eventService.Message = "ItemTestScanCode";
            ScanCode scanCode = GetTestScanCode();
            getScanCodeHandler.Setup(h => h.Handle(It.IsAny<GetScanCodeQuery>())).Returns(new List<ScanCode> { scanCode });
            getItemIdentifierHandler.Setup(h => h.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> { });
            updateItemServiceHandler.Setup(h => h.Process(It.IsAny<UpdateItemSubTeamDataService>()));
            getUserHandler.Setup(h => h.Handle(It.IsAny<GetUserQuery>()));
            getSubTeamHierarchyQueryHandlercs.Setup(h => h.Handle(It.IsAny<GetSubTeamHierarchyQuery>())).Returns(new HierarchyClass());

            //When
            eventService.Run();

            //Then 
            ScanCode actualScanCode;
            Cache.IdentifierToScanCode.TryGetValue(eventService.Message, out actualScanCode);
            Assert.AreNotEqual(null, actualScanCode);
        }

        [TestMethod]
        public void ItemSubTeamEventService_ScanCodeMessageInScanCodeCache_GetScanCodeQueryNotCalled()
        {
            //Given
            eventService.Message = "ItemTestScanCode";
            ScanCode scanCode = GetTestScanCode();
            getScanCodeHandler.Setup(h => h.Handle(It.IsAny<GetScanCodeQuery>())).Returns(new List<ScanCode> { scanCode });
            getItemIdentifierHandler.Setup(h => h.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> { });
            updateItemServiceHandler.Setup(h => h.Process(It.IsAny<UpdateItemSubTeamDataService>()));
            getUserHandler.Setup(h => h.Handle(It.IsAny<GetUserQuery>()));
            getSubTeamHierarchyQueryHandlercs.Setup(h => h.Handle(It.IsAny<GetSubTeamHierarchyQuery>())).Returns(new HierarchyClass());

            Cache.IdentifierToScanCode.Add(eventService.Message, scanCode);

            //When
            eventService.Run();

            //Then           
            getScanCodeHandler.Verify(command => command.Handle(It.IsAny<GetScanCodeQuery>()), Times.Never);
        }

        [TestMethod]
        public void ItemSubTeamEventService_ScanCodeDataRetreivedFromIcon_GetItemIdentifiersQueryCalledOneTime()
        {
            //Given
            eventService.Message = "ItemTestScanCode";
            ScanCode scanCode = GetTestScanCode();
            getScanCodeHandler.Setup(h => h.Handle(It.IsAny<GetScanCodeQuery>())).Returns(new List<ScanCode> { scanCode });
            getItemIdentifierHandler.Setup(h => h.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> { });
            updateItemServiceHandler.Setup(h => h.Process(It.IsAny<UpdateItemSubTeamDataService>()));
            getUserHandler.Setup(h => h.Handle(It.IsAny<GetUserQuery>()));
            getSubTeamHierarchyQueryHandlercs.Setup(h => h.Handle(It.IsAny<GetSubTeamHierarchyQuery>())).Returns(new HierarchyClass());

            //When
            eventService.Run();

            //Then           
            getItemIdentifierHandler.Verify(command => command.Handle(It.IsAny<GetItemIdentifiersQuery>()), Times.Once);
        }

        [TestMethod]
    
        public void ItemSubTeamEventService_NoSubTeamNameForDept_No_ThrowsSubTeamNotFoundException()
        {
            //Given
            eventService.Message = "ItemTestScanCode";
            ScanCode scanCode = GetTestScanCode();

            List<ItemIdentifier> identiferList = new List<ItemIdentifier>();
            identiferList.Add(GetDefaultIdentifier(scanCode));
            getScanCodeHandler.Setup(h => h.Handle(It.IsAny<GetScanCodeQuery>())).Returns(new List<ScanCode> { scanCode });
            getItemIdentifierHandler.Setup(h => h.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(identiferList);
            updateItemServiceHandler.Setup(h => h.Process(It.IsAny<UpdateItemSubTeamDataService>()));
            getUserHandler.Setup(h => h.Handle(It.IsAny<GetUserQuery>()));
            getSubTeamHierarchyQueryHandlercs.Setup(h => h.Handle(It.IsAny<GetSubTeamHierarchyQuery>())).Returns(new HierarchyClass());

            //When
            try
            {
                eventService.Run();
            }

            catch (Exception e)
            {
                //Then
                Assert.IsTrue(e.Message.StartsWith("The Sub team was not found for Dept_No:"));
            }
        }
        
        private ScanCode GetTestScanCode()
        {

            TestHierarchyClassBuilder testHierArchyClassBuilder = new TestHierarchyClassBuilder().WithHierarchyClassId(3).WithTaxAbbreviationTrait("TestTax");
            HierarchyClass taxHierArchyClass = testHierArchyClassBuilder;
            taxHierArchyClass.hierarchyID = 3;
            taxHierArchyClass.hierarchyClassName = "TestTaxHierArchy";
            //Can this go into Builder class
            taxHierArchyClass.HierarchyClassTrait.FirstOrDefault().Trait = new Trait() { traitCode = TraitCodes.TaxAbbreviation };
            Icon.Framework.Item testItemBuilder = new TestItemBuilder().WithScanCode("ItemTestScanCode").WithBrandAssociation(2).WithTaxClassAssociation(3)
                .WithValidationDate("01/12/2015").WithModifiedDate("01/12/2015").WithPackageUnit("1");
            ScanCode scanCode = testItemBuilder.ScanCode.FirstOrDefault();
            scanCode.ScanCodeType = new ScanCodeType() { scanCodeTypeDesc = "TestScanCodeDesc" };
            scanCode.Item = testItemBuilder;
            HierarchyClass brandHierarchyclass = new HierarchyClass() { hierarchyID = 2, hierarchyClassName = "TestBrandHierArchy", hierarchyClassID = 4 };
            brandHierarchyclass.Hierarchy = new Hierarchy() { hierarchyName = HierarchyNames.Brands };
            taxHierArchyClass.Hierarchy = new Hierarchy() { hierarchyName = HierarchyNames.Tax };
            scanCode.Item.ItemHierarchyClass.SingleOrDefault(ihc => ihc.hierarchyClassID == 2).HierarchyClass = brandHierarchyclass;
            scanCode.Item.ItemHierarchyClass.SingleOrDefault(ihc => ihc.hierarchyClassID == 3).HierarchyClass = taxHierArchyClass;

            //Add all needed Traits to Scan code
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.ValidationDate).Trait = new Trait() { traitCode = TraitCodes.ValidationDate };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.ProductDescription).Trait = new Trait() { traitCode = TraitCodes.ProductDescription };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.PosDescription).Trait = new Trait() { traitCode = TraitCodes.PosDescription };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.PosScaleTare).Locale = new Locale() { localeTypeID = LocaleTypes.Chain };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.FoodStampEligible).Trait = new Trait() { traitCode = TraitCodes.FoodStampEligible };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.PosScaleTare).Trait = new Trait() { traitCode = TraitCodes.PosScaleTare };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.PackageUnit).Trait = new Trait() { traitCode = TraitCodes.PackageUnit };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.ModifiedDate).Trait = new Trait() { traitCode = TraitCodes.ModifiedDate };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.RetailSize).Trait = new Trait() { traitCode = TraitCodes.RetailSize };
            scanCode.Item.ItemTrait.SingleOrDefault(itemTrait => itemTrait.traitID == Traits.RetailUom).Trait = new Trait() { traitCode = TraitCodes.RetailUom };
            return scanCode;
        }

        private ItemIdentifier GetDefaultIdentifier(ScanCode scanCode)
        {
            ItemIdentifier identifier = new ItemIdentifier();
            identifier.Default_Identifier = 1;
            identifier.Deleted_Identifier = 0;
            identifier.Identifier = scanCode.scanCode;
            return identifier;
        }
    }
}
