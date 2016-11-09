using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using SubteamEventController.DataAccess.Commands;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using GlobalEventController.DataAccess.Commands;

namespace SubteamEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddItemCategoryCommandHandlerTests
    {
        private IrmaContext context;
        private AddItemCategoryCommand addItemCategoryCommand;
        private AddItemCategoryCommandHandler addItemCategoryCommandHandler;
        private List<string> scanCodeList;
        private DbContextTransaction transaction;
        private readonly string SubTeamAlignedcategoryName = "SubTeam Aligned";

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.addItemCategoryCommand = new AddItemCategoryCommand();
            this.addItemCategoryCommandHandler = new AddItemCategoryCommandHandler(this.context);
            this.scanCodeList = new List<string>();

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {

            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void AddItemCategory_SubTeamAligendCategoryExists_DoesNotAddNewOne()
        {
            // Given
            ItemCategory existingItemCategory = GetSubTeamAlignedCategory();

            //When
            this.addItemCategoryCommand.SubTeamNo = existingItemCategory.SubTeam_No.Value;
            this.addItemCategoryCommandHandler.Handle(this.addItemCategoryCommand);

            //Then
            
            Assert.AreEqual(existingItemCategory.Category_ID, addItemCategoryCommand.ItemCategoryId);
        }

        [TestMethod]
        public void AddItemCategory_SubTeamAligendCategoryDoesNotExist_AddNewCategoryOne()
        {
            // Given
            int subTeamNo = GetSubTeamNotAlignedCategory();

            //When
            this.addItemCategoryCommand.SubTeamNo = subTeamNo;
            this.addItemCategoryCommandHandler.Handle(this.addItemCategoryCommand);

            //Then
            ItemCategory itemCategory = this.context.ItemCategory.SingleOrDefault(ic => ic.SubTeam_No == subTeamNo && ic.Category_Name.Equals("SubTeam Aligned"));
            Assert.IsNotNull(itemCategory);
        }

        private ItemCategory GetSubTeamAlignedCategory()
        {
            ItemCategory itemCategory = this.context.ItemCategory.FirstOrDefault(ic => ic.Category_Name.Equals("SubTeam Aligned"));
            if(itemCategory == null)
            {
                itemCategory = new ItemCategory();
                itemCategory.Category_Name = SubTeamAlignedcategoryName;
                itemCategory.SubTeam_No = this.context.SubTeam.First().SubTeam_No;
                this.context.ItemCategory.Add(itemCategory);
                this.context.SaveChanges();
            }
            return itemCategory;
        }

        private int GetSubTeamNotAlignedCategory()
        {
            int subTeamNo = -1;
            subTeamNo = this.context.SubTeam.First(st => !st.ItemCategory.Any(ic => ic.Category_Name.Equals(SubTeamAlignedcategoryName))).SubTeam_No;
            if (subTeamNo == -1)
            {
                return 0;
            }
            return subTeamNo;
        }
    }
}
