using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;
using Icon.Framework;
using System.Linq;
using System.Transactions;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class ArchiveItemsCommandHandlerTests
    {
        private ArchiveItemsCommandHandler commandHandler;
        private IconDbContextFactory contextFactory;
        private IconContext context;
        private IEnumerable<ItemModel> testModels;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            contextFactory = new IconDbContextFactory();
            context = new IconContext();
            commandHandler = new ArchiveItemsCommandHandler(contextFactory);
            testModels = new List<ItemModel>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ArchiveItems_Given10Items_ShouldAddItemsToArchiveTable()
        {
            //Given
            testModels = CreateTestModels(10).ToList();

            //When
            commandHandler.Execute(new ArchiveItemsCommand { Models = testModels });

            //Then
            var archivedProducts = context.MessageArchiveProduct
                .OrderByDescending(i => i.MessageArchiveId)
                .Take(10)
                .ToList()
                .OrderBy(i => i.ItemId)
                .ToList();

            for (int i = 0; i < testModels.Count(); i++)
            {
                Assert.AreEqual(testModels.ElementAt(i).ItemId, archivedProducts.ElementAt(i).ItemId);
                Assert.AreEqual(testModels.ElementAt(i).ScanCode, archivedProducts.ElementAt(i).ScanCode);
                Assert.AreEqual(testModels.ElementAt(i).InforMessageId, archivedProducts.ElementAt(i).InforMessageId);
            }
        }

        [TestMethod]
        public void ArchiveItems_ScanCodeIsGreaterThan13CharactersButLessThan100_ShouldArchiveItems()
        {
            //Given
            testModels = CreateTestModels(1);
            testModels.First().ScanCode = "test" + new string('t', 96);

            //When
            commandHandler.Execute(new ArchiveItemsCommand { Models = testModels });

            //Then
            var archivedProducts = context.MessageArchiveProduct
                .OrderByDescending(i => i.MessageArchiveId)
                .Take(1)
                .ToList()
                .OrderBy(i => i.ItemId)
                .ToList();

            for (int i = 0; i < testModels.Count(); i++)
            {
                Assert.AreEqual(testModels.ElementAt(i).ItemId, archivedProducts.ElementAt(i).ItemId);
                Assert.AreEqual(testModels.ElementAt(i).ScanCode, archivedProducts.ElementAt(i).ScanCode);
                Assert.AreEqual(testModels.ElementAt(i).InforMessageId, archivedProducts.ElementAt(i).InforMessageId);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ArchiveItems_ScanCodeIsGreaterThan100Characters_ShouldThrowException()
        {
            //Given
            testModels = CreateTestModels(1);
            testModels.First().ScanCode = "test" + new string('t', 97);

            //When
            commandHandler.Execute(new ArchiveItemsCommand { Models = testModels });
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
