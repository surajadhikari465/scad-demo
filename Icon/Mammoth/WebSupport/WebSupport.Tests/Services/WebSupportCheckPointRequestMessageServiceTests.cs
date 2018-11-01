using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.Commands;
using WebSupport.EsbProducerFactory;
using WebSupport.MessageBuilders;
using WebSupport.Models;
using WebSupport.Services;
using WebSupport.ViewModels;

namespace WebSupport.Tests.Services
{
    [TestClass]
    public class WebSupportCheckPointRequestMessageServiceTests
    {
        private WebSupportCheckPointRequestMessageService service;

        private Mock<ILogger> mockLogger;
        private Mock<IEsbConnectionFactory> mockEsbConnectionFactory;
        private EsbConnectionSettings fakeEsbSettings;
        private Mock<IEsbProducer> mockEsbProducer;
        private Mock<IMessageBuilder<CheckPointRequestBuilderModel>> mockRequestMessageBuilder;
        private Mock<IQueryHandler<GetCheckPointMessageParameters, IEnumerable<CheckPointMessageModel>>> mockGetCheckpointMessageQuery;
        private Mock<ICommandHandler<ArchiveCheckpointMessageCommandParameters>> mockArchiveCheckpointMessageQuery;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockEsbConnectionFactory = new Mock<IEsbConnectionFactory>();
            fakeEsbSettings = new EsbConnectionSettings();
            mockEsbProducer = new Mock<IEsbProducer>();
            mockRequestMessageBuilder = new Mock<IMessageBuilder<CheckPointRequestBuilderModel>>();
            mockGetCheckpointMessageQuery = new Mock<IQueryHandler<GetCheckPointMessageParameters, IEnumerable<CheckPointMessageModel>>>();
            mockArchiveCheckpointMessageQuery = new Mock<ICommandHandler<ArchiveCheckpointMessageCommandParameters>>();

            mockEsbConnectionFactory.Setup(f => f.CreateProducer(fakeEsbSettings))
                .Returns(mockEsbProducer.Object);

            service = new WebSupportCheckPointRequestMessageService(
                mockLogger.Object,
                mockEsbConnectionFactory.Object,
                fakeEsbSettings,
                mockRequestMessageBuilder.Object,
                mockGetCheckpointMessageQuery.Object,
                mockArchiveCheckpointMessageQuery.Object
                );
        }

        [TestMethod]
        public void CheckpointService_Send_ValidSingleStoreItemRequest_SendsCheckpointRequest()
        {
            //Given
            var storeOne = 10000;
            var itemA_id = 4567890;
            var itemA_scanCode = "444000444";
            var sequenceId_A = 1;
            var patchFamilyId_A = $"{itemA_id}-{storeOne}";

            string testMessageA = @"<PriceChangeMaster isCheckPoint=""true"">
                                      <BusinessKey variationID=""0"">00000000-0000-4000-8000-000000000000</BusinessKey>
                                      <PriceChangeHeader>
                                        <PatchFamilyID>4567890-10000</PatchFamilyID>
                                        <PatchNum>1</PatchNum>
                                        <TimeStamp>2018-10-23T10:20:07.408-05:00</TimeStamp>
                                      </PriceChangeHeader>
                                    </PriceChangeMaster>";
            var fakeQueryResults = new List<CheckPointMessageModel>
            {
                new CheckPointMessageModel{
                    ItemId = itemA_id,
                    SequenceId = sequenceId_A,
                    PatchFamilyId = patchFamilyId_A,
                    BusinessUnitID = storeOne,
                    ScanCode = itemA_scanCode
                }
            };
            var viewModel = new CheckPointRequestViewModel
            {
                RegionIndex = 0,
                Stores = new string[] { storeOne.ToString() },
                ScanCodes = itemA_scanCode
            };

            mockGetCheckpointMessageQuery
                .Setup(q => q.Search(It.IsAny<GetCheckPointMessageParameters>()))
                .Returns(fakeQueryResults);
            mockRequestMessageBuilder.Setup(b => b.BuildMessage(It.IsAny<CheckPointRequestBuilderModel>()))
                .Returns(testMessageA);

            //When
            var result = service.Send(viewModel);

            //Then
            mockEsbProducer.Verify(p => p.Send(testMessageA, It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void CheckpointService_Send_ValidMultipleSingleStoreItemRequest_SendsMultipleCheckpointRequests()
        {
            //Given
            var storeOne = 10000;
            var itemA_id = 4567890;
            var itemA_scanCode = "444000444";
            var sequenceId_A = 1;
            var patchFamilyId_A = $"{itemA_id}-{storeOne}";

            var storeTwo = 11111;
            var itemB_id = 45678901;
            var itemB_scanCode = "444000666";
            var sequenceId_B = 1;
            var patchFamilyId_B = $"{itemB_id}-{storeTwo}";

            string testMessageA = @"<PriceChangeMaster isCheckPoint=""true"">
                                      <BusinessKey variationID=""0"">00000000-0000-4000-8000-000000000000</BusinessKey>
                                      <PriceChangeHeader>
                                        <PatchFamilyID>4567890-10000</PatchFamilyID>
                                        <PatchNum>1</PatchNum>
                                        <TimeStamp>2018-10-23T10:20:07.408-05:00</TimeStamp>
                                      </PriceChangeHeader>
                                    </PriceChangeMaster>";
            string testMessageB = @"<PriceChangeMaster isCheckPoint=""true"">
                                      <BusinessKey variationID=""0"">00000000-0000-4000-8000-000000000000</BusinessKey>
                                      <PriceChangeHeader>
                                        <PatchFamilyID>45678901-11111</PatchFamilyID>
                                        <PatchNum>1</PatchNum>
                                        <TimeStamp>2018-10-23T10:20:07.608-05:00</TimeStamp>
                                      </PriceChangeHeader>
                                    </PriceChangeMaster>";

            var checkPointMessageA = new CheckPointMessageModel
            {
                ItemId = itemA_id,
                SequenceId = sequenceId_A,
                PatchFamilyId = patchFamilyId_A,
                BusinessUnitID = storeOne,
                ScanCode = itemA_scanCode
            };
            var checkPointMessageB = new CheckPointMessageModel
            {
                ItemId = itemB_id,
                SequenceId = sequenceId_B,
                PatchFamilyId = patchFamilyId_B,
                BusinessUnitID = storeTwo,
                ScanCode = itemB_scanCode
            };

            var fakeQueryResults = new List<CheckPointMessageModel>
            {
                checkPointMessageA,
                checkPointMessageB
            };
            var viewModel = new CheckPointRequestViewModel
            {
                RegionIndex = 0,
                Stores = new string[] { storeOne.ToString(), storeTwo.ToString() },
                ScanCodes = $"{itemA_scanCode}{Environment.NewLine}{itemB_scanCode}{Environment.NewLine}"
            };

            mockGetCheckpointMessageQuery
                .Setup(q => q.Search(It.IsAny<GetCheckPointMessageParameters>()))
                .Returns(fakeQueryResults);

            mockRequestMessageBuilder.SetupSequence(b => b.BuildMessage(It.IsAny<CheckPointRequestBuilderModel>()))
                .Returns(testMessageA)
                .Returns(testMessageB);

            //When
            var result = service.Send(viewModel);

            //Then
            mockEsbProducer.Verify(p => p.Send(testMessageA, It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockEsbProducer.Verify(p => p.Send(testMessageB, It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void CheckpointService_Send_ValidSingleStoreItemRequest_LogsBeforeAndAfterCheckpointRequest()
        {
            //Given
            var storeOne = 10000;
            var itemA_id = 4567890;
            var itemA_scanCode = "444000444";
            var sequenceId_A = 1;
            var patchFamilyId_A = $"{itemA_id}-{storeOne}";

            string testMessage = @"<PriceChangeMaster isCheckPoint=""true"">
                                      <BusinessKey variationID=""0"">00000000-0000-4000-8000-000000000000</BusinessKey>
                                      <PriceChangeHeader>
                                        <PatchFamilyID>4567890-10000</PatchFamilyID>
                                        <PatchNum>1</PatchNum>
                                        <TimeStamp>2018-10-23T10:20:07.408-05:00</TimeStamp>
                                      </PriceChangeHeader>
                                    </PriceChangeMaster>";
            var fakeQueryResults = new List<CheckPointMessageModel>
            {
                new CheckPointMessageModel{
                    ItemId = itemA_id,
                    SequenceId = sequenceId_A,
                    PatchFamilyId = patchFamilyId_A,
                    BusinessUnitID = storeOne,
                    ScanCode = itemA_scanCode
                }
            };
            var viewModel = new CheckPointRequestViewModel
            {
                RegionIndex = 0,
                Stores = new string[] { storeOne.ToString() },
                ScanCodes = itemA_scanCode
            };

            mockGetCheckpointMessageQuery
                .Setup(q => q.Search(It.IsAny<GetCheckPointMessageParameters>()))
                .Returns(fakeQueryResults);
            mockRequestMessageBuilder.Setup(b => b.BuildMessage(It.IsAny<CheckPointRequestBuilderModel>()))
                .Returns(testMessage);

            var expectedStartLogInfo = @"{""Action"":""Log CheckpointRequest Start"",""Store"":""10000"",""ScanCode"":""444000444""}";
            var expectedResultLogInfo = @"{""Action"":""Log CheckpointRequest Result"",""Store"":""10000"",""ScanCode"":""444000444""," +
                @"""Response"":{""Message"":null,""Status"":0,""ErrorCode"":null,""ErrorDetails"":""Store: '10000' ScanCode: '444000444' sent for checkpoint request.""}}";

            //When
            var result = service.Send(viewModel);

            //Then
            mockLogger.Verify(l => l.Info(expectedStartLogInfo), Times.Once);
            mockLogger.Verify(l => l.Info(expectedResultLogInfo), Times.Once);
        }

        [TestMethod]
        public void CheckpointService_Send_ValidMultipleSingleStoreItemRequest_ArchivesMultipleMessages()
        {
            //Given
            var storeOne = 10000;
            var itemA_id = 4567890;
            var itemA_scanCode = "444000444";
            var sequenceId_A = 1;
            var patchFamilyId_A = $"{itemA_id}-{storeOne}";

            var storeTwo = 11111;
            var itemB_id = 45678901;
            var itemB_scanCode = "444000666";
            var sequenceId_B = 1;
            var patchFamilyId_B = $"{itemB_id}-{storeTwo}";

            string testMessageA = @"<PriceChangeMaster isCheckPoint=""true"">
                                      <BusinessKey variationID=""0"">00000000-0000-4000-8000-000000000000</BusinessKey>
                                      <PriceChangeHeader>
                                        <PatchFamilyID>4567890-10000</PatchFamilyID>
                                        <PatchNum>1</PatchNum>
                                        <TimeStamp>2018-10-23T10:20:07.408-05:00</TimeStamp>
                                      </PriceChangeHeader>
                                    </PriceChangeMaster>";
            string testMessageB = @"<PriceChangeMaster isCheckPoint=""true"">
                                      <BusinessKey variationID=""0"">00000000-0000-4000-8000-000000000000</BusinessKey>
                                      <PriceChangeHeader>
                                        <PatchFamilyID>45678901-11111</PatchFamilyID>
                                        <PatchNum>1</PatchNum>
                                        <TimeStamp>2018-10-23T10:20:07.608-05:00</TimeStamp>
                                      </PriceChangeHeader>
                                    </PriceChangeMaster>";

            var checkPointMessageA = new CheckPointMessageModel
            {
                ItemId = itemA_id,
                SequenceId = sequenceId_A,
                PatchFamilyId = patchFamilyId_A,
                BusinessUnitID = storeOne,
                ScanCode = itemA_scanCode
            };
            var checkPointMessageB = new CheckPointMessageModel
            {
                ItemId = itemB_id,
                SequenceId = sequenceId_B,
                PatchFamilyId = patchFamilyId_B,
                BusinessUnitID = storeTwo,
                ScanCode = itemB_scanCode
            };

            var fakeQueryResults = new List<CheckPointMessageModel>
            {
                checkPointMessageA,
                checkPointMessageB
            };
            var viewModel = new CheckPointRequestViewModel
            {
                RegionIndex = 0,
                Stores = new string[] { storeOne.ToString(), storeTwo.ToString() },
                ScanCodes = $"{itemA_scanCode}{Environment.NewLine}{itemB_scanCode}{Environment.NewLine}"
            };

            mockGetCheckpointMessageQuery
                .Setup(q => q.Search(It.IsAny<GetCheckPointMessageParameters>()))
                .Returns(fakeQueryResults);

            mockRequestMessageBuilder.SetupSequence(b => b.BuildMessage(It.IsAny<CheckPointRequestBuilderModel>()))
                .Returns(testMessageA)
                .Returns(testMessageB);

            //When
            var result = service.Send(viewModel);

            //Then
            mockArchiveCheckpointMessageQuery.Verify(a => a.Execute(It.IsAny<ArchiveCheckpointMessageCommandParameters>()), Times.Exactly(2));
        }
    }
}
