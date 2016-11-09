using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;
using Icon.Framework;
using System.Linq;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class ArchiveItemsCommandHandlerTests
    {
        private ArchiveItemsCommandHandler commandHandler;
        private IconContext context;
        private IEnumerable<ItemModel> testModels;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new ArchiveItemsCommandHandler();
            testModels = new List<ItemModel>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            var archivedProducts = context.MessageArchiveProduct.Where(p => p.ScanCode.StartsWith("test")).ToList();
            context.MessageArchiveProduct.RemoveRange(archivedProducts);
            context.SaveChanges();
        }

        [TestMethod]
        public void ArchiveItems_Given10Items_ShouldAddItemsToArchiveTable()
        {
            //Given
            testModels = CreateTestModels(10);

            //When
            commandHandler.Execute(new ArchiveItemsCommand { Models = testModels });

            //Then
            var archivedProducts = context.MessageArchiveProduct.Where(p => p.ScanCode.StartsWith("test")).ToList();

            Assert.AreEqual(10, archivedProducts.Count);
        }

        private IEnumerable<ItemModel> CreateTestModels(int numberOfModels)
        {
            List<ItemModel> models = new List<ItemModel>();
            for (int i = 0; i < numberOfModels; i++)
            {
                models.Add(new ItemModel
                {
                    ItemId = i,
                    ScanCode = "test" + i.ToString(),
                    InforMessageId = Guid.NewGuid()
                });
            }
            return models;
        }
    }
}
