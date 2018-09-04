using System;
using KitBuilderWebApi.Controllers;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Expressions;


namespace KitBuilderWebApi.Tests.Controllers
{
    [TestClass]
    public class LinkGroupControllerTests
    {
        private LinkGroupController linkGroupController;
        private Mock<ILogger<LinkGroupController>> mockLogger;
        private Mock<IRepository<LinkGroup>> mockLinkGroupRepository;
        private Mock<IRepository<LinkGroupItem>> mockLinkGroupItemRepository;
        private Mock<IRepository<Items>> mockItemsRepository;
        private Mock<IUrlHelper> mockUrlHelper;
        private Mock<LinkGroupHelper> mockLinkGroupHelper;
        private Mock<IRepository<KitLinkGroupItem>> mockKitlinkGroupItemRepository;
        private Mock<IRepository<KitLinkGroup>> mockKitlinkGroupRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            mockLogger = new Mock<ILogger<LinkGroupController>>();
            mockLinkGroupRepository = new Mock<IRepository<LinkGroup>>();
            mockLinkGroupItemRepository = new Mock<IRepository<LinkGroupItem>>();
            mockItemsRepository = new Mock<IRepository<Items>>();
            mockKitlinkGroupItemRepository = new Mock<IRepository<KitLinkGroupItem>>();
            mockKitlinkGroupRepository = new Mock<IRepository<KitLinkGroup>>();

            mockLinkGroupHelper = new Mock<LinkGroupHelper>(mockUrlHelper, mockLinkGroupRepository, mockLinkGroupItemRepository,
                                                            mockItemsRepository, mockKitlinkGroupItemRepository,
                                                            mockKitlinkGroupRepository);

            linkGroupController = new LinkGroupController(mockLinkGroupRepository.Object,
                mockLinkGroupItemRepository.Object,
                mockItemsRepository.Object,
                mockLogger.Object,
                mockLinkGroupHelper.Object);
        }

        [TestMethod]
        public void linkGroupController_GetInstructionsList_NoParametersPassed_Returns_OK()
        {   // Given

            //When
            var response = linkGroupController.GetLinkGroups(null);

            // Then
            Assert.IsInstanceOfType(response, typeof(OkObjectResult), "Ok Request Expected");
        }

    }
}
