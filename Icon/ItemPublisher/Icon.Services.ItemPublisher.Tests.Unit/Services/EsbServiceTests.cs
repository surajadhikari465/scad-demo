using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Esb;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services.Tests
{
    [TestClass()]
    public class EsbServiceTests
    {
        /// <summary>
        /// Tests that when the ESB service returns success that it is handled
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_Succeeds_ShouldSendMessageAndReturnSuccess()
        {
            // Given.
            Mock<IEsbClient> esbClientMock = new Mock<IEsbClient>();
            Mock<IEsbMessageBuilder> esbMessageBuilderMock = new Mock<IEsbMessageBuilder>();
            Mock<ILogger<ItemPublisherService>> loggerMock = new Mock<ILogger<ItemPublisherService>>();
            Mock<ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items>> serializerMock = new Mock<ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items>>();

            items esbItems = new items
            {
                item = new ItemType[]
                {
                    new ItemType
                    {
                        Action = ActionEnum.AddOrUpdate,
                        id = 1,
                        @base = new BaseItemType
                        {
                            type = new ItemTypeType
                            {
                                code = ItemPublisherConstants.RetailSaleTypeCode,
                                description = ItemPublisherConstants.RetailSaleTypeCodeDescription
                            }
                        }
                    }
                 }
            };

            esbMessageBuilderMock.Setup(x => x.BuildItem(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult<BuildMessageResult>(new BuildMessageResult(true, esbItems, new List<string>() { })));

            EsbService service = new EsbService(serializerMock.Object,
                esbClientMock.Object,
                esbMessageBuilderMock.Object,
                loggerMock.Object);

            esbClientMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(Task.FromResult(new EsbSendResult(true, "test", "request", new Dictionary<string, string>() { }, Guid.Parse("48a5364b-5748-493d-80ee-748cd3008869"))));
            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());

            List<MessageQueueItemModel> models = new List<MessageQueueItemModel>()
            {
                messageQueueItemModelBuilder.Build(
                new Item()
                {
                    ItemAttributesJson = "{'test':'value', 'IsActive':1}",
                },
                new List<Hierarchy>(),
                new Nutrition())
            };

            // When.
            EsbSendResult result = await service.Process(models, new List<string>() { });

            // Then.
            Assert.IsTrue(result.Success, "The service sent a message and we should have received a success response");
            esbClientMock.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<List<string>>()));
        }

        /// <summary>
        /// Tests that if an error is returned from the ESB client that the error is returned correctly
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_Fails_ShouldReturnErrors()
        {
            // Given.
            Mock<IEsbClient> esbClientMock = new Mock<IEsbClient>();
            Mock<IEsbMessageBuilder> esbMessageBuilderMock = new Mock<IEsbMessageBuilder>();
            Mock<ILogger<ItemPublisherService>> loggerMock = new Mock<ILogger<ItemPublisherService>>();
            Mock<ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items>> serializerMock = new Mock<ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items>>();

            items esbItems = new items
            {
                item = new ItemType[]
                {
                    new ItemType
                    {
                        Action = ActionEnum.AddOrUpdate,
                        id = 1,
                        @base = new BaseItemType
                        {
                            type = new ItemTypeType
                            {
                                code = ItemPublisherConstants.RetailSaleTypeCode,
                                description = ItemPublisherConstants.RetailSaleTypeCodeDescription
                            }
                        }
                    }
                }
            };

            esbMessageBuilderMock.Setup(x => x.BuildItem(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult<BuildMessageResult>(new BuildMessageResult(false, esbItems, new List<string>() { })));

            esbClientMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(Task.FromResult(new EsbSendResult(false, "Error Occurred", "request", new Dictionary<string, string>() { }, Guid.Parse("48a5364b-5748-493d-80ee-748cd3008869"))));

            EsbService service = new EsbService(serializerMock.Object,
                esbClientMock.Object,
                esbMessageBuilderMock.Object,
                loggerMock.Object);

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            List<MessageQueueItemModel> models = new List<MessageQueueItemModel>()
            {
                messageQueueItemModelBuilder.Build(
                new Item()
                {
                     ItemAttributesJson = "{'test':'value', 'IsActive':1}",
                },
                new List<Hierarchy>(),
                new Nutrition())
            };

            // When.
            EsbSendResult result = await service.Process(models, new List<string>() { });

            // Then.
            Assert.IsFalse(result.Success, "The ESB should have returned an error");
            Assert.AreEqual("Error Occurred", result.Message);
        }

        /// <summary>
        /// Tests that if an Item has an attribute that cannot be mapped to the attributes table that a warning is generated
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Process_TraitNotFound_WarningShouldBeSet()
        {
            // Given.
            string expectedWarning = "Trait not found";

            Mock<IEsbClient> esbClientMock = new Mock<IEsbClient>();
            Mock<IEsbMessageBuilder> esbMessageBuilderMock = new Mock<IEsbMessageBuilder>();
            Mock<ILogger<ItemPublisherService>> loggerMock = new Mock<ILogger<ItemPublisherService>>();
            Mock<ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items>> serializerMock = new Mock<ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items>>();

            items esbItems = new items
            {
                item = new ItemType[]
                {
                    new ItemType
                    {
                        Action = ActionEnum.AddOrUpdate,
                        id = 1,
                        @base = new BaseItemType
                        {
                            type = new ItemTypeType
                            {
                                code = ItemPublisherConstants.RetailSaleTypeCode,
                                description = ItemPublisherConstants.RetailSaleTypeCodeDescription
                            }
                        }
                    }
                }
            };

            esbMessageBuilderMock.Setup(x => x.BuildItem(It.IsAny<List<MessageQueueItemModel>>())).Returns(Task.FromResult<BuildMessageResult>(new BuildMessageResult(true, esbItems, new List<string>() { expectedWarning })));

            esbClientMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(Task.FromResult(new EsbSendResult(true, "", "request", new Dictionary<string, string>() { }, Guid.Parse("48a5364b-5748-493d-80ee-748cd3008869"))));

            EsbService service = new EsbService(serializerMock.Object,
                esbClientMock.Object,
                esbMessageBuilderMock.Object,
                loggerMock.Object);

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            List<MessageQueueItemModel> models = new List<MessageQueueItemModel>()
            {
                messageQueueItemModelBuilder.Build(
                new Item()
                {
                     ItemAttributesJson = "{'test':'value', 'Inactive':'true'}",
                },
                new List<Hierarchy>(),
                new Nutrition())
            };

            // When.
            EsbSendResult result = await service.Process(models, new List<string>() { });

            // Then.
            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedWarning, result.Warnings.First());
        }
    }
}