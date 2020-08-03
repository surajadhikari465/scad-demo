using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Icon.Common.DataAccess;
using Icon.FeatureFlags;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class PriceLineAssociationControllerTests
    {
        private PriceLineAssociationController controller;
        private Mock<IQueryHandler<GetItemGroupByIdParameters, ItemGroupModel>> getItemGroupByIdQuery;
        private Mock<IQueryHandler<GetItemGroupMembersParameters, IEnumerable<ItemGroupMember>>> getItemGroupMembersQuery;
        private Mock<IQueryHandler<GetItemGroupAssociationSearchItemPartialParameters, IEnumerable<ItemGroupAssociationItemModel>>> getItemGroupAssociationSearchItemPartialQuery;
        private Mock<ICommandHandler<SetPrimaryItemGroupItemCommand>> setPrimaryItemGroupItemCommand;
        private Mock<ICommandHandler<AddItemToItemGroupCommand>> addItemToItemGroupCommand;
        private Mock<IFeatureFlagService> featureFlagService;
        private Mock<ILogger> logger;
        private ItemGroupModel itemGroupModel;
        private List<ItemGroupMember> itemGroupMembers;
        private List<ItemGroupAssociationItemModel> itemAssociationItems;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.getItemGroupByIdQuery = new Mock<IQueryHandler<GetItemGroupByIdParameters, ItemGroupModel>>();
            this.getItemGroupMembersQuery = new Mock<IQueryHandler<GetItemGroupMembersParameters, IEnumerable<ItemGroupMember>>>();
            this.setPrimaryItemGroupItemCommand = new Mock<ICommandHandler<SetPrimaryItemGroupItemCommand>>();
            this.getItemGroupAssociationSearchItemPartialQuery = new Mock<IQueryHandler<GetItemGroupAssociationSearchItemPartialParameters, IEnumerable<ItemGroupAssociationItemModel>>>();
            this.addItemToItemGroupCommand = new Mock<ICommandHandler<AddItemToItemGroupCommand>>();
            this.featureFlagService = new Mock<IFeatureFlagService>();
            this.logger = new Mock<ILogger>();

            // Setup data (ItemGroup / Item Group Model)
            this.itemGroupModel = new ItemGroupModel
            {
                ItemGroupId = 1313,
                ItemGroupAttributesJson = "{\"PriceLineDescription\":\"Evil Cat Litter $.85 1 EA\",\"PriceLineRetailSize\":\"1\",\"PriceLineUOM\":\"EA\"}",
                ItemGroupTypeId = ItemGroupTypeId.Priceline,
                ItemCount = 1,
                PriceLineDescription = "Evil Cat Litter $.85 1 EA",
                PriceLineSize = "1",
                PriceLineUOM = "EA",
                ScanCode = "0987654321321",
                SKUDescription = null
            };

            this.itemGroupMembers = new List<ItemGroupMember>
                {
                    new ItemGroupMember
                    {
                        ItemGroupId = 1313,
                        ItemId = 123,
                        IsPrimary = true,
                        CustomerFriendlyDescription = "Organic Evil Cat Litter",
                        ItemPack = "1",
                        ProductDescription = "Organic Evil Cat Litter",
                        RetailSize = "1",
                        ScanCode = "0987654321321",
                        UOM = "EA"
                    },
                    new ItemGroupMember
                    {
                        ItemGroupId = 101313,
                        ItemId = 10123,
                        IsPrimary = false,
                        CustomerFriendlyDescription = "Premium Evil Cat Litter",
                        ItemPack = "1",
                        ProductDescription = "Premium Evil Cat Litter",
                        RetailSize = "1",
                        ScanCode = "1230987654321",
                        UOM = "EA"
                    }
                };

            this.itemAssociationItems = new List<ItemGroupAssociationItemModel> {
                new ItemGroupAssociationItemModel
                {
                    CustomerFriendlyDescription = "Angelic Cat Litter",
                    IsPrimary = true,
                    ItemGroupId = 1010101,
                    ItemGroupItemCount = 1,
                    ItemId = 101010101,
                    PriceLineDescription =  "Angelic Cat Litter 1 lb",
                    ScanCode = "2222222222222",
                    SKUDescription = null
                },
                new ItemGroupAssociationItemModel
                {
                    CustomerFriendlyDescription = "Pure Angelic Cat Litter",
                    IsPrimary = true,
                    ItemGroupId = 1010102,
                    ItemGroupItemCount = 1,
                    ItemId = 101010102,
                    PriceLineDescription =  "Pure Angelic Cat Litter 1 lb",
                    ScanCode = "2222222222221",
                    SKUDescription = null
                }
            };

            // Setup Mocks
            this.getItemGroupByIdQuery.Setup(m => m.Search(It.IsAny<GetItemGroupByIdParameters>()))
                .Returns(this.itemGroupModel);
            this.getItemGroupMembersQuery.Setup(m => m.Search(It.IsAny<GetItemGroupMembersParameters>()))
                .Returns((IEnumerable<ItemGroupMember>)this.itemGroupMembers);
            this.setPrimaryItemGroupItemCommand.Setup(m => m.Execute(It.IsAny<SetPrimaryItemGroupItemCommand>()))
                .Callback(() => 
                {
                    var oldPrimary = this.itemGroupMembers.First(igm => igm.IsPrimary == true);
                    var newPrimary = this.itemGroupMembers.First(igm => igm.IsPrimary == false);
                    oldPrimary.IsPrimary = false;
                    newPrimary.IsPrimary = true;
                });
            this.getItemGroupAssociationSearchItemPartialQuery.Setup(m => m.Search(It.IsAny<GetItemGroupAssociationSearchItemPartialParameters>()))
                .Returns(this.itemAssociationItems);

            this.addItemToItemGroupCommand.Setup(m => m.Execute(It.IsAny<AddItemToItemGroupCommand>()));

            this.featureFlagService.Setup(m => m.IsEnabled(It.IsAny<string>())).Returns(() => true);
            this.controller = new PriceLineAssociationController(
                this.getItemGroupByIdQuery.Object,
                this.getItemGroupMembersQuery.Object,
                this.getItemGroupAssociationSearchItemPartialQuery.Object,
                this.setPrimaryItemGroupItemCommand.Object,
                this.addItemToItemGroupCommand.Object,
                this.featureFlagService.Object,
                this.logger.Object
                );
        }

        [TestMethod]
        public void Controller_contructor_should_validate_arguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(
                null,
                this.getItemGroupMembersQuery.Object,
                this.getItemGroupAssociationSearchItemPartialQuery.Object,
                this.setPrimaryItemGroupItemCommand.Object,
                this.addItemToItemGroupCommand.Object,
                this.featureFlagService.Object,
                this.logger.Object
                ));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(
                this.getItemGroupByIdQuery.Object,
                null,
                this.getItemGroupAssociationSearchItemPartialQuery.Object,
                this.setPrimaryItemGroupItemCommand.Object,
                this.addItemToItemGroupCommand.Object,
                this.featureFlagService.Object,
                this.logger.Object
                ));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(
                this.getItemGroupByIdQuery.Object,
                this.getItemGroupMembersQuery.Object,
                null,
                this.setPrimaryItemGroupItemCommand.Object,
                this.addItemToItemGroupCommand.Object,
                this.featureFlagService.Object,
                this.logger.Object
                ));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(
                this.getItemGroupByIdQuery.Object,
                this.getItemGroupMembersQuery.Object,
                this.getItemGroupAssociationSearchItemPartialQuery.Object,
                null,
                this.addItemToItemGroupCommand.Object,
                this.featureFlagService.Object,
                this.logger.Object
                ));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(
                this.getItemGroupByIdQuery.Object,
                this.getItemGroupMembersQuery.Object,
                this.getItemGroupAssociationSearchItemPartialQuery.Object,
                this.setPrimaryItemGroupItemCommand.Object,
                null,
                this.featureFlagService.Object,
                this.logger.Object
                ));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(
                this.getItemGroupByIdQuery.Object,
                this.getItemGroupMembersQuery.Object,
                this.getItemGroupAssociationSearchItemPartialQuery.Object,
                this.setPrimaryItemGroupItemCommand.Object,
                this.addItemToItemGroupCommand.Object,
                null,
                this.logger.Object
                )); 
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(
                 this.getItemGroupByIdQuery.Object,
                 this.getItemGroupMembersQuery.Object,
                 this.getItemGroupAssociationSearchItemPartialQuery.Object,
                 this.setPrimaryItemGroupItemCommand.Object,
                 this.addItemToItemGroupCommand.Object,
                 this.featureFlagService.Object,
                 null
                 ));
            Assert.ThrowsException<ArgumentNullException>(() => new PriceLineAssociationController(null, null, null, null, null, null, null));
        }

        /// <summary>
        /// Checks that index returna a view.
        /// </summary>
        [TestMethod]
        public void Controller_index_should_return_view()
        {
            // Given.

            // When.
            ViewResult result = controller.Index(123) as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Checks that GetAllRelatedItems returns json.
        /// </summary>
        [TestMethod]
        public void GetAllRelatedItems_returns_json()
        {
            // Given.

            // When.
            JsonResult result = controller.GetAllRelatedItems(123) as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var resultPriceLineMemberData = JsonConvert.DeserializeObject<DataTableResponse<ItemGroupMember>>(jsonResult);

            Assert.AreEqual(2, resultPriceLineMemberData.data.Count);
        }


        /// <summary>
        /// Checks that GetAllRelatedItems returns json.
        /// </summary>
        [TestMethod]
        public void SearchItemByPrefix_returns_json()
        {
            // Given.

            // When.
            JsonResult result = controller.SearchItemByPrefix(123, "123") as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var resultSearchItemsData = JsonConvert.DeserializeObject<List<ItemGroupAssociationAutoCompleteData>>(jsonResult);
            Assert.AreEqual(2, resultSearchItemsData.Count);
        }

        /// <summary>
        /// Checks that GetAllRelatedItems returns json.
        /// </summary>
        [TestMethod]
        public void ChangePrimaryItem_returns_json()
        {
            // Given.

            // When.
            JsonResult result = controller.ChangePrimaryItem(123, 123) as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var changePrimaryItemResult = JsonConvert.DeserializeObject<ChangePrimaryItemResult>(jsonResult);

            Assert.AreEqual(true, changePrimaryItemResult.success);
        }

        [TestMethod]
        public void AddItem_returns_json()
        {
            // Given.

            // When.
            JsonResult result = controller.AddItemToPriceLine(123, 123) as JsonResult;

            // Then.
            Assert.IsNotNull(result);

            //Verify returned data
            string jsonResult = JsonConvert.SerializeObject(result.Data);
            var changePrimaryItemResult = JsonConvert.DeserializeObject<ChangePrimaryItemResult>(jsonResult);

            Assert.AreEqual(true, changePrimaryItemResult.success);
        }

        private class ChangePrimaryItemResult
        {
            public bool success { get; set; }
        }
    }
}
