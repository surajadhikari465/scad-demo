using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpsgenieAlert;
using Services.Extract.Credentials;
using Services.Extract.DataAccess.Commands;
using Services.Extract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extract.Tests
{
    [TestClass]
    public class ExtractServiceListenerTests
    {
        private ExtractServiceListener listener;
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> subscriber;
        private Mock<IEmailClient> emailClient;
        private Mock<ILogger<ExtractServiceListener>> serviceLogger;
        private Mock<ILogger<ExtractJobRunner>> extractJoblogger;
        private Mock<ICredentialsCacheManager> credentialsCacheManager;
        private Mock<IFileDestinationCache> fileDestinationCache;
        private Mock<IOpsgenieAlert> opsGenieAlert;
        private Mock<ICommandHandler<UpdateJobLastRunEndCommand>> updateJobLastRunEndCommandHandler;
        private Mock<ICommandHandler<UpdateJobStatusCommand>> updateJobStatusCommandHandler;
        private Mock<IMessageParser<JobSchedule>> messageParser;
        private Mock<IExtractJobConfigurationParser> extractJobConfigurationParser;
        private Mock<IExtractJobRunnerFactory> extractJobRunnerFactory;

        [TestInitialize]
        public void Initialize()
        {
            listenerApplicationSettings = new ListenerApplicationSettings();
            esbConnectionSettings = new EsbConnectionSettings();
            subscriber = new Mock<IEsbSubscriber>();
            emailClient = new Mock<IEmailClient>();
            serviceLogger = new Mock<ILogger<ExtractServiceListener>>();
            extractJoblogger = new Mock<ILogger<ExtractJobRunner>>();
            credentialsCacheManager = new Mock<ICredentialsCacheManager>();
            fileDestinationCache = new Mock<IFileDestinationCache>();
            opsGenieAlert = new Mock<IOpsgenieAlert>();
            updateJobLastRunEndCommandHandler = new Mock<ICommandHandler<UpdateJobLastRunEndCommand>>();
            updateJobStatusCommandHandler = new Mock<ICommandHandler<UpdateJobStatusCommand>>();
            messageParser = new Mock<IMessageParser<JobSchedule>>();
            extractJobConfigurationParser = new Mock<IExtractJobConfigurationParser>();
            extractJobRunnerFactory = new Mock<IExtractJobRunnerFactory>();

            listener = new ExtractServiceListener(
                listenerApplicationSettings,
                esbConnectionSettings,
                subscriber.Object,
                emailClient.Object,
                serviceLogger.Object,
                extractJoblogger.Object,
                credentialsCacheManager.Object,
                fileDestinationCache.Object,
                opsGenieAlert.Object,
                updateJobLastRunEndCommandHandler.Object,
                updateJobStatusCommandHandler.Object,
                messageParser.Object,
                extractJobConfigurationParser.Object,
                extractJobRunnerFactory.Object);
        }

        [TestMethod]
        public void HandleMessage_ReceivesMessageWithNoError_RunsExtractAndUpdatesStatus()
        {
            //Given
            Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();
            messageParser
                .Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new JobSchedule { JobName = "Test" });
            var jobRunner = new Mock<IExtractJobRunner>();
            extractJobRunnerFactory.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<ILogger<ExtractJobRunner>>(), It.IsAny<IOpsgenieAlert>(), It.IsAny<ICredentialsCacheManager>(), It.IsAny<IFileDestinationCache>()))
                    .Returns(jobRunner.Object);
            
            //When
            listener.HandleMessage(
                null, 
                new EsbMessageEventArgs { Message = mockMessage.Object });

            //Then
            messageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()));
            extractJobConfigurationParser.Verify(m => m.Parse(It.IsAny<string>()));
            extractJobRunnerFactory.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<ILogger<ExtractJobRunner>>(), It.IsAny<IOpsgenieAlert>(), It.IsAny<ICredentialsCacheManager>(), It.IsAny<IFileDestinationCache>()));
            updateJobStatusCommandHandler.Verify(m => m.Execute(It.Is<UpdateJobStatusCommand>(c => c.Status == Constants.RunningJobStatus)), Times.Once);
            updateJobStatusCommandHandler.Verify(m => m.Execute(It.Is<UpdateJobStatusCommand>(c => c.Status == Constants.ReadyJobStatus)), Times.Once);
            updateJobLastRunEndCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateJobLastRunEndCommand>()));
            jobRunner.Verify(m => m.Run(It.IsAny<ExtractJobConfiguration>()));
            serviceLogger.Verify(m => m.Info("Job Complete: Test"));
        }

        [TestMethod]
        public void HandleMessage_ReceivesMessageWithError_CatchExceptionAndUpdatesStatus()
        {
            //Given
            Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();
            messageParser
                .Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new JobSchedule { JobName = "Test" });
            var jobRunner = new Mock<IExtractJobRunner>();
            jobRunner.Setup(m => m.Run(It.IsAny<ExtractJobConfiguration>()))
                .Throws(new Exception("Test Exception"));
            extractJobRunnerFactory.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<ILogger<ExtractJobRunner>>(), It.IsAny<IOpsgenieAlert>(), It.IsAny<ICredentialsCacheManager>(), It.IsAny<IFileDestinationCache>()))
                    .Returns(jobRunner.Object);

            //When
            listener.HandleMessage(
                null,
                new EsbMessageEventArgs { Message = mockMessage.Object });

            //Then
            messageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()));
            extractJobConfigurationParser.Verify(m => m.Parse(It.IsAny<string>()));
            extractJobRunnerFactory.Verify(m => m.Create(It.IsAny<string>(), It.IsAny<ILogger<ExtractJobRunner>>(), It.IsAny<IOpsgenieAlert>(), It.IsAny<ICredentialsCacheManager>(), It.IsAny<IFileDestinationCache>()));
            updateJobStatusCommandHandler.Verify(m => m.Execute(It.Is<UpdateJobStatusCommand>(c => c.Status == Constants.RunningJobStatus)), Times.Once);
            updateJobStatusCommandHandler.Verify(m => m.Execute(It.Is<UpdateJobStatusCommand>(c => c.Status == Constants.ReadyJobStatus)), Times.Once);
            updateJobLastRunEndCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateJobLastRunEndCommand>()));
            jobRunner.Verify(m => m.Run(It.IsAny<ExtractJobConfiguration>()));
            serviceLogger.Verify(m => m.Error("Job Failed: Test"));
        }
    }
}
