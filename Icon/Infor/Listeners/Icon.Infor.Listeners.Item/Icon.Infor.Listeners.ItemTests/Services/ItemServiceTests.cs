using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.Item.Services;
using Moq;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Common.DataAccess;
using System.Linq;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Item.Tests.Services
{
    [TestClass]
    public class ItemServiceTests
    {
        private ItemService itemService;
        private Mock<ICommandHandler<ItemAddOrUpdateCommand>> mockAddOrUpdateItemsCommandHandler;
        private Mock<ICommandHandler<GenerateItemMessagesCommand>> mockGenerateItemMessagesCommandHandler;
        private Mock<ICommandHandler<ArchiveItemsCommand>> mockArchiveItemsCommandHandler;
        private Mock<ICommandHandler<ArchiveMessageCommand>> mockArchiveMessageCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockAddOrUpdateItemsCommandHandler = new Mock<ICommandHandler<ItemAddOrUpdateCommand>>();
            mockGenerateItemMessagesCommandHandler = new Mock<ICommandHandler<GenerateItemMessagesCommand>>();
            mockArchiveItemsCommandHandler = new Mock<ICommandHandler<ArchiveItemsCommand>>();
            mockArchiveMessageCommandHandler = new Mock<ICommandHandler<ArchiveMessageCommand>>();

            itemService = new ItemService(
                mockAddOrUpdateItemsCommandHandler.Object,
                mockGenerateItemMessagesCommandHandler.Object,
                mockArchiveItemsCommandHandler.Object,
                mockArchiveMessageCommandHandler.Object);
        }

        [TestMethod]
        public void AddOrUpdateItems_WhenPassedInItemModels_ShouldCallCommandHandlerWithItemModels()
        {
            //When
            itemService.AddOrUpdateItems(new List<ItemModel>());

            //Then
            mockAddOrUpdateItemsCommandHandler
                .Verify(m => m.Execute(It.IsAny<ItemAddOrUpdateCommand>()), Times.Once);
        }
    }
}
