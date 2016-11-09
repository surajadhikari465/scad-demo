using GlobalEventController.Common;
using SubteamEventController.DataAccess.BulkCommands;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SubteamEventController.Controller;


namespace SubteamEventController.Tests.DataAccess.BulkCommandTests
{

    [TestClass]
    public class BulkUpdateItemSubTeamCommandHandlerTests
    {
        private IrmaContext context;
        private BulkUpdateItemSubTeamCommand command;
        private BulkUpdateItemSubTeamCommandHandler handler;
        private List<ItemSubTeamModel> itemSubTeamModelList;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext(); // this is the FL ItemCatalog_Test database
            this.command = new BulkUpdateItemSubTeamCommand();
            this.handler = new BulkUpdateItemSubTeamCommandHandler(this.context);
            this.itemSubTeamModelList = new List<ItemSubTeamModel>();

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        [TestMethod]
        public void BulkUpdateItemSubTeam_ItemWithAlignedSubTeam_ItemUpdatedInIRMAWithSubTeam()
        {
            // Given
            string identifierOne = this.context.ItemIdentifier
                .First(ii => ii.Default_Identifier == 1 && ii.Deleted_Identifier == 0 && ii.Item.Retail_Sale == true).Identifier;

            SubTeam subTeam = this.context.SubTeam.First(st => st.AlignedSubTeam.HasValue && st.AlignedSubTeam.Value);

            this.itemSubTeamModelList.Add(new TestItemSubTeamModelBuilder()
                .WithScanCode(identifierOne).WithSubTeamNotAligned(false).WithDeptNo(subTeam.Dept_No.Value).Build());
            this.command.ItemSubTeamModels = this.itemSubTeamModelList;

            DateTime now = DateTime.Now;

            // When
            this.handler.Handle(this.command);

            // Then
            Item actualItemOne = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierOne));

            //// first item
            Assert.AreEqual(subTeam.SubTeam_No, actualItemOne.SubTeam_No, "The SubTeamNo does not match the expected value.");
            Assert.IsTrue(actualItemOne.LastModifiedDate > now);
        }

        [TestMethod]
        public void BulkUpdateItemSubTeam_ItemWithNonAlignedSubTeam_ItemUpdatedInIRMAWithDummySubTeam()
        {
            // Given
            SubTeam subTeam = this.context.SubTeam.First(st => st.AlignedSubTeam.HasValue && st.AlignedSubTeam.Value);
            SubTeam subTeamNonAligned = this.context.SubTeam.First(st => st.Dept_No == SubTeamConstants.DefaultNonAlignedPosDeptNo);

            var itemQuery = this.context.Item
                 .Where(i => i.ItemIdentifier
                    .Any(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1))
                    .Where(i => i.SubTeam_No == subTeam.SubTeam_No);

            Item item = itemQuery.Where(i => i.SubTeam_No == subTeam.SubTeam_No).First();
            string identifierOne = this.context.ItemIdentifier
                .First(ii => ii.Item_Key == item.Item_Key && ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1)
                .Identifier;

            this.itemSubTeamModelList.Add(new TestItemSubTeamModelBuilder()
                .WithScanCode(identifierOne)
                .WithSubTeamNotAligned(true)
                .WithDeptNo(subTeamNonAligned.Dept_No.Value)
                .Build());

            this.command.ItemSubTeamModels = this.itemSubTeamModelList;
            DateTime now = DateTime.Now;

            // When
            this.handler.Handle(this.command);

            // Then
            Item actualItemOne = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierOne));
            context.Entry<Item>(actualItemOne).Reload();

            Assert.AreEqual(SubTeamConstants.DefaultNonAlignedPosDeptNo, actualItemOne.SubTeam_No, "The SubTeamNo does not match the expected value.");
            Assert.IsTrue(actualItemOne.LastModifiedDate > now);
        }

        [TestMethod]
        public void BulkUpdateItemSubTeam_ItemUpdatedFromAndToNonAlignedSubTeam_ItemIsNotUpdatedInIrma()
        {
            // Given
            SubTeam nonAlignedSubTeam = this.context.SubTeam.First(st => st.AlignedSubTeam.HasValue && !st.AlignedSubTeam.Value);

            Item item = this.context.Item
                .First(i => i.SubTeam_No == nonAlignedSubTeam.SubTeam_No
                    && i.Retail_Sale
                    && i.Remove_Item == 0 && !i.Deleted_Item
                    && i.ItemIdentifier.Any(ii => ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0));

            string identifierOne = item.ItemIdentifier.First(ii => ii.Default_Identifier == 1).Identifier;

            this.itemSubTeamModelList.Add(new TestItemSubTeamModelBuilder()
                .WithScanCode(identifierOne)
                .WithSubTeamNotAligned(true)
                .WithDeptNo(nonAlignedSubTeam.Dept_No.Value)
                .Build());

            this.command.ItemSubTeamModels = this.itemSubTeamModelList;
            DateTime now = DateTime.Now;

            // When
            this.handler.Handle(this.command);

            // Then
            Item actualItemOne = this.context.Item.First(i => i.ItemIdentifier.Any(ii => ii.Identifier == identifierOne));
            Assert.AreEqual(item.SubTeam_No, actualItemOne.SubTeam_No, "The SubTeamNo does not match the expected value.");
            if (item.LastModifiedDate == null)
                Assert.IsNull(actualItemOne.LastModifiedDate);
            else
                Assert.IsTrue(actualItemOne.LastModifiedDate < now);
        }

    }
}
