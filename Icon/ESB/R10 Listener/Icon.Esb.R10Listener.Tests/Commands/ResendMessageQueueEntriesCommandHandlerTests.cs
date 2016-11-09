using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.R10Listener.Commands;
using Moq;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Esb.R10Listener.Models;
using System.Collections.Generic;
using Icon.Testing.Builders;
using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.Infrastructure.Cache;

namespace Icon.Esb.R10Listener.Tests.Commands
{
    [TestClass]
    public class ResendMessageQueueEntriesCommandHandlerTests : CommandHandlerTestBase<ResendMessageQueueEntriesCommandHandler, ResendMessageQueueEntriesCommand>
    {
        private MessageHistory messageHistory;
        private R10ListenerApplicationSettings applicationSettings;
        Mock<IMessageQueueResendStatusCache> mockCache;

        protected override void Initialize()
        {
            applicationSettings = new R10ListenerApplicationSettings { ResendMessageCount = 1 };
            mockCache = new Mock<IMessageQueueResendStatusCache>();
            commandHandler = new ResendMessageQueueEntriesCommandHandler(context, mockCache.Object, applicationSettings);

            command = new ResendMessageQueueEntriesCommand();
            messageHistory = new MessageHistory
            {
                Message = "<test>test</test>",
                MessageTypeId = MessageTypes.Product,
                MessageStatusId = MessageStatusTypes.Sent,
                InsertDate = DateTime.Now
            };
        }

        [TestMethod]
        public void ResendMessageQueueEntries_MessageHistoryIsProductAndResendStatusIsLessThanMaxResendStatus_ShouldMarkErrorProductMessagesAsReady()
        {
            //Given
            List<MessageQueueProduct> productMessages = new List<MessageQueueProduct> 
                { 
                    new TestProductMessageBuilder().WithItemId(1).WithStatusId(MessageStatusTypes.Associated),
                    new TestProductMessageBuilder().WithItemId(2).WithStatusId(MessageStatusTypes.Associated), 
                    new TestProductMessageBuilder().WithItemId(3).WithStatusId(MessageStatusTypes.Associated)
                };
            foreach (var product in productMessages)
            {
                messageHistory.MessageQueueProduct.Add(product);
            }
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            List<BusinessErrorModel> businessErrors = new List<BusinessErrorModel>
            {
                new BusinessErrorModel { MainId = productMessages[0].ItemId, Code = "TestErrorCode" },
                new BusinessErrorModel { MainId = productMessages[1].ItemId, Code = BusinessErrorCodes.ThresholdExceededError },
                new BusinessErrorModel { MainId = productMessages[2].ItemId, Code = BusinessErrorCodes.ThresholdExceededError }
            };
            command.MessageResponse = new R10MessageResponseModel
            {
                BusinessErrors = businessErrors,
                MessageHistoryId = messageHistory.MessageHistoryId
            };
            command.MessageHistory = messageHistory;

            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 0 });

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.AreEqual(MessageStatusTypes.Associated, productMessages[0].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Ready, productMessages[1].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Ready, productMessages[2].MessageStatusId);
        }

        [TestMethod]
        public void ResendMessageQueueEntries_MessageHistoryIsPriceAndResendStatusIsLessThanMaxResendStatus_ShouldMarkErrorPriceMessagesAsReady()
        {
            //Given
            IRMAPush irmaPush = new TestIrmaPushBuilder();
            context.Context.IRMAPush.Add(irmaPush);
            context.SaveChanges();

            List<MessageQueuePrice> priceMessages = new List<MessageQueuePrice> 
                { 
                    new TestPriceMessageBuilder().WithItemId(1).WithStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID),
                    new TestPriceMessageBuilder().WithItemId(2).WithStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID), 
                    new TestPriceMessageBuilder().WithItemId(3).WithStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID)
                };
            foreach (var priceMessage in priceMessages)
            {
                messageHistory.MessageQueuePrice.Add(priceMessage);
            }
            messageHistory.MessageTypeId = MessageTypes.Price;
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();

            List<BusinessErrorModel> businessErrors = new List<BusinessErrorModel>
            {
                new BusinessErrorModel { MainId = priceMessages[0].ItemId, Code = "TestErrorCode" },
                new BusinessErrorModel { MainId = priceMessages[1].ItemId, Code = BusinessErrorCodes.ThresholdExceededError },
                new BusinessErrorModel { MainId = priceMessages[2].ItemId, Code = BusinessErrorCodes.ThresholdExceededError }
            };
            command.MessageResponse = new R10MessageResponseModel
            {
                BusinessErrors = businessErrors,
                MessageHistoryId = messageHistory.MessageHistoryId
            };
            command.MessageHistory = messageHistory;

            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 0 });

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.AreEqual(MessageStatusTypes.Associated, priceMessages[0].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Ready, priceMessages[1].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Ready, priceMessages[2].MessageStatusId);
        }

        [TestMethod]
        public void ResendMessageQueueEntries_MessageHistoryIsItemLocaleAndResendStatusIsLessThanMaxResendStatus_ShouldMarkErrorItemLocaleMessagesAsReady()
        {
            //Given
            IRMAPush irmaPush = new TestIrmaPushBuilder();
            context.Context.IRMAPush.Add(irmaPush);
            context.SaveChanges();

            List<MessageQueueItemLocale> itemLocaleMessages = new List<MessageQueueItemLocale> 
                { 
                    new TestItemLocaleMessageBuilder().WithItemId(1).WithMessageStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID),
                    new TestItemLocaleMessageBuilder().WithItemId(2).WithMessageStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID),
                    new TestItemLocaleMessageBuilder().WithItemId(3).WithMessageStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID)
                };
            foreach (var itemLocaleMessage in itemLocaleMessages)
            {
                messageHistory.MessageQueueItemLocale.Add(itemLocaleMessage);
            }
            messageHistory.MessageTypeId = MessageTypes.ItemLocale;
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            List<BusinessErrorModel> businessErrors = new List<BusinessErrorModel>
            {
                new BusinessErrorModel { MainId = itemLocaleMessages[0].ItemId, Code = "TestErrorCode" },
                new BusinessErrorModel { MainId = itemLocaleMessages[1].ItemId, Code = BusinessErrorCodes.ThresholdExceededError },
                new BusinessErrorModel { MainId = itemLocaleMessages[2].ItemId, Code = BusinessErrorCodes.ThresholdExceededError }
            };
            command.MessageResponse = new R10MessageResponseModel
            {
                BusinessErrors = businessErrors,
                MessageHistoryId = messageHistory.MessageHistoryId
            };
            command.MessageHistory = messageHistory;

            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 0 });

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.AreEqual(MessageStatusTypes.Associated, itemLocaleMessages[0].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Ready, itemLocaleMessages[1].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Ready, itemLocaleMessages[2].MessageStatusId);
        }
        [TestMethod]
        public void ResendMessageQueueEntries_MessageHistoryIsProductAndResendStatusIsEqualToMaxResendStatus_ShouldMarkErrorProductMessagesAsReady()
        {
            //Given
            List<MessageQueueProduct> productMessages = new List<MessageQueueProduct> 
                { 
                    new TestProductMessageBuilder().WithItemId(1).WithStatusId(MessageStatusTypes.Associated),
                    new TestProductMessageBuilder().WithItemId(2).WithStatusId(MessageStatusTypes.Associated), 
                    new TestProductMessageBuilder().WithItemId(3).WithStatusId(MessageStatusTypes.Associated)
                };
            foreach (var product in productMessages)
            {
                messageHistory.MessageQueueProduct.Add(product);
            }
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            List<BusinessErrorModel> businessErrors = new List<BusinessErrorModel>
            {
                new BusinessErrorModel { MainId = productMessages[0].ItemId, Code = "TestErrorCode" },
                new BusinessErrorModel { MainId = productMessages[1].ItemId, Code = BusinessErrorCodes.ThresholdExceededError },
                new BusinessErrorModel { MainId = productMessages[2].ItemId, Code = BusinessErrorCodes.ThresholdExceededError }
            };
            command.MessageResponse = new R10MessageResponseModel
            {
                BusinessErrors = businessErrors,
                MessageHistoryId = messageHistory.MessageHistoryId
            };
            command.MessageHistory = messageHistory;

            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 1 });

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.AreEqual(MessageStatusTypes.Associated, productMessages[0].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Associated, productMessages[1].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Associated, productMessages[2].MessageStatusId);
        }

        [TestMethod]
        public void ResendMessageQueueEntries_MessageHistoryIsPriceAndResendStatusIsEqualToThanMaxResendStatus_ShouldMarkErrorPriceMessagesAsReady()
        {
            //Given
            IRMAPush irmaPush = new TestIrmaPushBuilder();
            context.Context.IRMAPush.Add(irmaPush);
            context.SaveChanges();

            List<MessageQueuePrice> priceMessages = new List<MessageQueuePrice> 
                { 
                    new TestPriceMessageBuilder().WithItemId(1).WithStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID),
                    new TestPriceMessageBuilder().WithItemId(2).WithStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID), 
                    new TestPriceMessageBuilder().WithItemId(3).WithStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID)
                };
            foreach (var priceMessage in priceMessages)
            {
                messageHistory.MessageQueuePrice.Add(priceMessage);
            }
            messageHistory.MessageTypeId = MessageTypes.Price;
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();

            List<BusinessErrorModel> businessErrors = new List<BusinessErrorModel>
            {
                new BusinessErrorModel { MainId = priceMessages[0].ItemId, Code = "TestErrorCode" },
                new BusinessErrorModel { MainId = priceMessages[1].ItemId, Code = BusinessErrorCodes.ThresholdExceededError },
                new BusinessErrorModel { MainId = priceMessages[2].ItemId, Code = BusinessErrorCodes.ThresholdExceededError }
            };
            command.MessageResponse = new R10MessageResponseModel
            {
                BusinessErrors = businessErrors,
                MessageHistoryId = messageHistory.MessageHistoryId
            };
            command.MessageHistory = messageHistory;

            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 1 });

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.AreEqual(MessageStatusTypes.Associated, priceMessages[0].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Associated, priceMessages[1].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Associated, priceMessages[2].MessageStatusId);
        }

        [TestMethod]
        public void ResendMessageQueueEntries_MessageHistoryIsItemLocaleAndResendStatusIsEqualToThanMaxResendStatus_ShouldMarkErrorItemLocaleMessagesAsReady()
        {
            //Given
            IRMAPush irmaPush = new TestIrmaPushBuilder();
            context.Context.IRMAPush.Add(irmaPush);
            context.SaveChanges();

            List<MessageQueueItemLocale> itemLocaleMessages = new List<MessageQueueItemLocale> 
                { 
                    new TestItemLocaleMessageBuilder().WithItemId(1).WithMessageStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID),
                    new TestItemLocaleMessageBuilder().WithItemId(2).WithMessageStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID),
                    new TestItemLocaleMessageBuilder().WithItemId(3).WithMessageStatusId(MessageStatusTypes.Associated).WithIrmaPushId(irmaPush.IRMAPushID)
                };
            foreach (var itemLocaleMessage in itemLocaleMessages)
            {
                messageHistory.MessageQueueItemLocale.Add(itemLocaleMessage);
            }
            messageHistory.MessageTypeId = MessageTypes.ItemLocale;
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            List<BusinessErrorModel> businessErrors = new List<BusinessErrorModel>
            {
                new BusinessErrorModel { MainId = itemLocaleMessages[0].ItemId, Code = "TestErrorCode" },
                new BusinessErrorModel { MainId = itemLocaleMessages[1].ItemId, Code = BusinessErrorCodes.ThresholdExceededError },
                new BusinessErrorModel { MainId = itemLocaleMessages[2].ItemId, Code = BusinessErrorCodes.ThresholdExceededError }
            };
            command.MessageResponse = new R10MessageResponseModel
            {
                BusinessErrors = businessErrors,
                MessageHistoryId = messageHistory.MessageHistoryId
            };
            command.MessageHistory = messageHistory;

            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 1 });

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.AreEqual(MessageStatusTypes.Associated, itemLocaleMessages[0].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Associated, itemLocaleMessages[1].MessageStatusId);
            Assert.AreEqual(MessageStatusTypes.Associated, itemLocaleMessages[2].MessageStatusId);
        }
    }
}
