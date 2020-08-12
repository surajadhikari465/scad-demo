using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddProductSelectionGroupMessageCommandHandlerTests
    {
        private AddProductSelectionGroupMessageCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            context = new IconContext();
            commandHandler = new AddProductSelectionGroupMessageCommandHandler(context);
            transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Rollback();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddProductSelectionGroupMessage_ProductSelectionGroupTypeIdIsZero_ExceptionShouldBeThrown()
        {
            // Given.
            ProductSelectionGroup psg = new ProductSelectionGroup
            {
                ProductSelectionGroupName = "Test PSG",
                ProductSelectionGroupTypeId = ProductSelectionGroupTypes.Consumable,
                TraitId = Traits.AgeRestrict,
                TraitValue = "Test Trait Value",
                MerchandiseHierarchyClassId = null
            };

            context.ProductSelectionGroup.Add(psg);
            context.SaveChanges();

            // When.
            commandHandler.Execute(new AddProductSelectionGroupMessageCommand
            {
                ProductSelectionGroupId = psg.ProductSelectionGroupId,
                ProductSelectionGroupName = psg.ProductSelectionGroupName,
                ProductSelectionGroupTypeId = 0
            });

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void AddProductSelectionGroupMessage_ProductSelectionGroupTypeIdIsNotNullOrZero_PsgMessageShouldBeAdded()
        {
            // Given.
            ProductSelectionGroup psg = new ProductSelectionGroup
            {
                ProductSelectionGroupName = "Test PSG",
                ProductSelectionGroupTypeId = ProductSelectionGroupTypes.Consumable,
                Trait = context.Trait.Single(t => t.traitID == Traits.AgeRestrict),
                TraitValue = "Test Trait Value",
                MerchandiseHierarchyClassId = null
            };

            context.ProductSelectionGroup.Add(psg);
            context.SaveChanges();

            // When.
            commandHandler.Execute(new AddProductSelectionGroupMessageCommand
            {
                ProductSelectionGroupId = psg.ProductSelectionGroupId,
                ProductSelectionGroupName = psg.ProductSelectionGroupName,
                ProductSelectionGroupTypeId = psg.ProductSelectionGroupTypeId
            });

            // Then.
            var psgMessage = context.MessageQueueProductSelectionGroup.SingleOrDefault(psgm => psgm.ProductSelectionGroupId == psg.ProductSelectionGroupId);
            string psgTypeName = context.ProductSelectionGroupType.Single(type => type.ProductSelectionGroupTypeId == psgMessage.ProductSelectionGroupTypeId).ProductSelectionGroupTypeName;

            Assert.IsNotNull(psgMessage);
            Assert.AreEqual(psg.ProductSelectionGroupId, psgMessage.ProductSelectionGroupId);
            Assert.AreEqual(psg.ProductSelectionGroupName, psgMessage.ProductSelectionGroupName);
            Assert.AreEqual(psg.ProductSelectionGroupTypeId, psgMessage.ProductSelectionGroupTypeId);
            Assert.AreEqual(psgTypeName, psgMessage.ProductSelectionGroupTypeName);
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, psgMessage.MessageActionId);
            Assert.AreEqual(MessageStatusTypes.Ready, psgMessage.MessageStatusId);
            Assert.AreEqual(MessageTypes.ProductSelectionGroup, psgMessage.MessageTypeId);
        }
    }
}
