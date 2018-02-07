using Esb.Core.MessageBuilders;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.Commands;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.PrimeAffinity.Library.Tests.Processors
{
    [TestClass]
    public class PrimeAffinityPsgProcessorTests
    {
        private PrimeAffinityPsgProcessor processor;
        private PrimeAffinityPsgProcessorSettings settings;
        private Mock<IMessageBuilder<PrimeAffinityMessageBuilderParameters>> messageBuilder;
        private Mock<IEsbConnectionFactory> esbConnectionFactory;
        private Mock<ICommandHandler<ArchivePrimeAffinityMessageCommand>> archivePrimeAffinityMessagesCommandHandler;
        private Mock<ILogger<PrimeAffinityPsgProcessor>> logger;
        private PrimeAffinityPsgProcessorParameters parameters;
        private Mock<IEsbProducer> producer;

        [TestInitialize]
        public void Initialize()
        {
            settings = new PrimeAffinityPsgProcessorSettings();
            messageBuilder = new Mock<IMessageBuilder<PrimeAffinityMessageBuilderParameters>>();
            esbConnectionFactory = new Mock<IEsbConnectionFactory>();
            archivePrimeAffinityMessagesCommandHandler = new Mock<ICommandHandler<ArchivePrimeAffinityMessageCommand>>();
            logger = new Mock<ILogger<PrimeAffinityPsgProcessor>>();

            processor = new PrimeAffinityPsgProcessor(
                settings,
                messageBuilder.Object,
                esbConnectionFactory.Object,
                archivePrimeAffinityMessagesCommandHandler.Object,
                logger.Object);
            parameters = new PrimeAffinityPsgProcessorParameters { Region = "FL" };
            producer = new Mock<IEsbProducer>();
            esbConnectionFactory.Setup(m => m.CreateProducer(true))
                .Returns(producer.Object);
            messageBuilder.Setup(m => m.BuildMessage(It.IsAny<PrimeAffinityMessageBuilderParameters>()))
                .Returns("Test");
        }

        [TestMethod]
        public void Process_SalesExist_ShouldSendMessageToEsb()
        {
            //Given
            parameters.PrimeAffinityMessageModels = new List<PrimeAffinityMessageModel>
                {
                    new PrimeAffinityMessageModel
                    {
                        BusinessUnitID = 1,
                        MessageAction = ActionEnum.AddOrUpdate
                    }
                }.AsEnumerable();

            //When
            processor.SendPsgs(parameters);

            //Then
            messageBuilder.Verify(m => m.BuildMessage(It.IsAny<PrimeAffinityMessageBuilderParameters>()), Times.Once);
            esbConnectionFactory.Verify(m => m.CreateProducer(true), Times.Once);
            archivePrimeAffinityMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePrimeAffinityMessageCommand>()), Times.Once);
            producer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void Process_SalesDontExist_ShouldNotSendMessageToEsb()
        {
            //Given
            parameters.PrimeAffinityMessageModels = new List<PrimeAffinityMessageModel>().AsEnumerable();

            //When
            processor.SendPsgs(parameters);

            //Then
            messageBuilder.Verify(m => m.BuildMessage(It.IsAny<PrimeAffinityMessageBuilderParameters>()), Times.Never);
            esbConnectionFactory.Verify(m => m.CreateProducer(true), Times.Once);
            archivePrimeAffinityMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePrimeAffinityMessageCommand>()), Times.Never);
            producer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void Process_ErrorOccurs_ShouldArchiveError()
        {
            //Given
            parameters.PrimeAffinityMessageModels = new List<PrimeAffinityMessageModel>
                {
                    new PrimeAffinityMessageModel
                    {
                        BusinessUnitID = 1,
                        MessageAction = ActionEnum.AddOrUpdate
                    }
                }.AsEnumerable();
            producer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new Exception());

            //When
            processor.SendPsgs(parameters);

            //Then
            messageBuilder.Verify(m => m.BuildMessage(It.IsAny<PrimeAffinityMessageBuilderParameters>()), Times.Once);
            esbConnectionFactory.Verify(m => m.CreateProducer(true), Times.Once);
            archivePrimeAffinityMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePrimeAffinityMessageCommand>()), Times.Once);
            producer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            logger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Process_ErrorOccursWhenSendingOneMessage_ShouldSendNonErroredMessages()
        {
            //Given
            parameters.PrimeAffinityMessageModels = new List<PrimeAffinityMessageModel>
                {
                    new PrimeAffinityMessageModel { BusinessUnitID = 1, MessageAction = ActionEnum.AddOrUpdate },
                    new PrimeAffinityMessageModel { BusinessUnitID = 2, MessageAction = ActionEnum.AddOrUpdate }
                }.AsEnumerable();
            producer.SetupSequence(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new Exception())
                .Pass();

            //When
            processor.SendPsgs(parameters);

            //Then
            messageBuilder.Verify(m => m.BuildMessage(It.IsAny<PrimeAffinityMessageBuilderParameters>()), Times.Exactly(2));
            esbConnectionFactory.Verify(m => m.CreateProducer(true), Times.Once);
            archivePrimeAffinityMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePrimeAffinityMessageCommand>()), Times.Exactly(2));
            producer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(2));
            logger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Process_NumberOfSalesIsGreaterThanBatchSize_ShouldSendMessagesIn100CountBatches()
        {
            //Given
            var primeAffinityModels = new List<PrimeAffinityMessageModel>();
            for (int i = 0; i < 350; i++)
            {
                primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 1, MessageAction = ActionEnum.AddOrUpdate });
            }
            parameters.PrimeAffinityMessageModels = primeAffinityModels.AsEnumerable();

            //When
            processor.SendPsgs(parameters);

            //Then
            messageBuilder.Verify(m => m.BuildMessage(It.IsAny<PrimeAffinityMessageBuilderParameters>()), Times.Exactly(4));
            esbConnectionFactory.Verify(m => m.CreateProducer(true), Times.Once);
            archivePrimeAffinityMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePrimeAffinityMessageCommand>()), Times.Exactly(4));
            producer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(4));
            logger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Process_MultipleBusinessUnitsExist_ShouldSendMessagesGroupedByBusinessUnit()
        {
            //Given
            var primeAffinityModels = new List<PrimeAffinityMessageModel>();
            primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 1, MessageAction = ActionEnum.AddOrUpdate });
            primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 2, MessageAction = ActionEnum.AddOrUpdate });
            primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 3, MessageAction = ActionEnum.AddOrUpdate });
            primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 4, MessageAction = ActionEnum.AddOrUpdate });
            parameters.PrimeAffinityMessageModels = primeAffinityModels.AsEnumerable();

            //When
            processor.SendPsgs(parameters);

            //Then
            messageBuilder.Verify(m => m.BuildMessage(It.IsAny<PrimeAffinityMessageBuilderParameters>()), Times.Exactly(4));
            esbConnectionFactory.Verify(m => m.CreateProducer(true), Times.Once);
            archivePrimeAffinityMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePrimeAffinityMessageCommand>()), Times.Exactly(4));
            producer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(4));
            logger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Process_MultipleBusinessUnitsExistAndNumberOfSalesIsGreaterThanBatchSize_ShouldSendMessagesGroupedByBusinessUnitAndMessagesAreGroupedByBusinessUnit()
        {
            //Given
            var primeAffinityModels = new List<PrimeAffinityMessageModel>();
            for (int i = 0; i < 201; i++)
            {
                primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 1, MessageAction = ActionEnum.AddOrUpdate });
            }
            for (int i = 0; i < 202; i++)
            {
                primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 2, MessageAction = ActionEnum.AddOrUpdate });
            }
            for (int i = 0; i < 203; i++)
            {
                primeAffinityModels.Add(new PrimeAffinityMessageModel { BusinessUnitID = 3, MessageAction = ActionEnum.AddOrUpdate });
            }
            parameters.PrimeAffinityMessageModels = primeAffinityModels.AsEnumerable();

            //When
            processor.SendPsgs(parameters);

            //Then
            messageBuilder.Verify(
                m => m.BuildMessage(
                    It.Is<PrimeAffinityMessageBuilderParameters>(
                        (p) => p.PrimeAffinityMessageModels
                            .All(pam => pam.BusinessUnitID == p.PrimeAffinityMessageModels.First().BusinessUnitID))),
                Times.Exactly(9));
            esbConnectionFactory.Verify(m => m.CreateProducer(true), Times.Once);
            archivePrimeAffinityMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchivePrimeAffinityMessageCommand>()), Times.Exactly(9));
            producer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(9));
            logger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
        }
    }
}

