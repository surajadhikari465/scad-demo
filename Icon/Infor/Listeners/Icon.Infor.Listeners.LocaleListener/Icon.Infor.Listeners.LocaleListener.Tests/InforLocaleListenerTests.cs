using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.MessageParsers;
using Icon.Infor.Listeners.LocaleListener.Models;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Icon.Esb.ListenerApplication;
using Icon.Esb;
using Icon.Esb.Subscriber;
using Icon.Common.Email;
using Icon.Logging;
using Moq;
using Icon.Infor.Listeners.LocaleListener.Queries;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;

namespace Icon.Infor.Listeners.LocaleListener.Tests
{
    /// <summary>
    /// Summary description for InforLocaleListenerTests
    /// </summary>
    [TestClass]
    public class InforLocaleListenerTests
    {
        private InforLocaleListener listener;
        private Mock<ICommandHandler<AddOrUpdateLocalesCommand>> mockAddOrUpdateLocalesCommandHandler;
        private Mock<ICommandHandler<ArchiveLocaleMessageCommand>> mockArchiveLocaleMessageCommandHandler;
        private Mock<IEmailClient> mockEmailClient;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<ICommandHandler<GenerateLocaleMessagesCommand>> mockGenerateLocaleMessagesCommandHandler;
        private ListenerApplicationSettings listenerApplicationSettings;
        private Mock<ILogger<InforLocaleListener>> mockLogger;
        private Mock<IMessageParser<LocaleModel>> mockMessageParser;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEsbMessage> mockEsbMessage;
        private Mock<IQueryHandler<GetSequenceIdFromLocaleIdParameters,int>> mockGetSequenceIdFromLocaleIdQueryHandler;
        private Mock<IQueryHandler<GetSequenceIdFromBusinessUnitIdParameters, int>> mockGetSequenceIdFromBusinessUnitIdQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockAddOrUpdateLocalesCommandHandler = new Mock<ICommandHandler<AddOrUpdateLocalesCommand>>();
            mockArchiveLocaleMessageCommandHandler = new Mock<ICommandHandler<ArchiveLocaleMessageCommand>>();
            mockEmailClient = new Mock<IEmailClient>();
            esbConnectionSettings = new EsbConnectionSettings();
            mockGenerateLocaleMessagesCommandHandler = new Mock<ICommandHandler<GenerateLocaleMessagesCommand>>();
            listenerApplicationSettings = new ListenerApplicationSettings();
            mockLogger = new Mock<ILogger<InforLocaleListener>>();
            mockMessageParser = new Mock<IMessageParser<LocaleModel>>();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockGetSequenceIdFromLocaleIdQueryHandler = new Mock<IQueryHandler<GetSequenceIdFromLocaleIdParameters,int>>();
            mockGetSequenceIdFromBusinessUnitIdQueryHandler = new Mock<IQueryHandler<GetSequenceIdFromBusinessUnitIdParameters, int>>();
            listener = new InforLocaleListener(
                mockMessageParser.Object,
                mockAddOrUpdateLocalesCommandHandler.Object,
                mockGenerateLocaleMessagesCommandHandler.Object,
                mockArchiveLocaleMessageCommandHandler.Object,
                listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockGetSequenceIdFromLocaleIdQueryHandler.Object,
                mockGetSequenceIdFromBusinessUnitIdQueryHandler.Object);

            mockEsbMessage = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void HandleMessage_SuccessfullyParsesMessage_CallsCommandHandlers()
        {
            //Given
            LocaleModel localeModel = SetUpData();
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(localeModel);

            //When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
            mockGenerateLocaleMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<GenerateLocaleMessagesCommand>()), Times.Once);
            mockArchiveLocaleMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveLocaleMessageCommand>()), Times.Once);
        }
        [TestMethod]
        public void HandleMessage_SuccessfullyParsesMessage_CallsCommandHandlers_ValidateSequence()
        {
            //Given
            LocaleModel localeModel = SetUpData();
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(localeModel);
            mockGetSequenceIdFromBusinessUnitIdQueryHandler.Setup(m => m.Search(It.IsAny<GetSequenceIdFromBusinessUnitIdParameters>())).Returns(1);
            //When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
            mockGenerateLocaleMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<GenerateLocaleMessagesCommand>()), Times.Once);
            mockArchiveLocaleMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveLocaleMessageCommand>()), Times.Once);
        }

        private LocaleModel SetUpData()
        {
            Models.LocaleAddress localeAddress = new Models.LocaleAddress(4457, "Altamonte Springs Test", "", "", "Tampa", "30216",
                                                                           "USA", "FL", "Eastern Standard Time", "26.365658", "-80.114501", 7865);
            List<LocaleTraitModel> LocaleTraits = new List<LocaleTraitModel>()
                                                        {   new LocaleTraitModel(Traits.PhoneNumber,"407-767-2100",null,7865),
                                                            new LocaleTraitModel(Traits.Fax,"407-767-2111", null,7865),
                                                            new LocaleTraitModel(Traits.ContactPerson,"Admin",  null,7865),
                                                            new LocaleTraitModel(Traits.IrmaStoreId,"555",  null,7865),
                                                            new LocaleTraitModel(Traits.PsBusinessUnitId,"7865",  null,7865),
                                                            new LocaleTraitModel(Traits.StorePosType,"", null,7865),
                                                            new LocaleTraitModel(Traits.StoreAbbreviation,"TST",  null,7865),
                                                        };

            LocaleModel storeModel = new LocaleModel
                (
                    0,
                    2002,
                    10130,
                    "TestStore",
                    "ST",
                    DateTime.Now,
                    DateTime.Now,
                    null,
                    ActionEnum.AddOrUpdate,
                    localeAddress,
                    LocaleTraits
                );
            storeModel.SequenceId = 1;
            LocaleModel metroModel = CreateLocaleModel(2002, "TestMetro", 2001, "MT", ActionEnum.AddOrUpdate, new List<LocaleModel> { storeModel });
            metroModel.SequenceId = 1;
            LocaleModel regionModel = CreateLocaleModel(2001, "TestRegion", 2000, "RG", ActionEnum.AddOrUpdate, new List<LocaleModel> { metroModel });
            regionModel.SequenceId = 1;
            LocaleModel chainModel = CreateLocaleModel(2000, "Testchain", null, "Ch", ActionEnum.AddOrUpdate, new List<LocaleModel>() { regionModel });
            chainModel.SequenceId = 1;
            LocaleModel organizationModel = CreateLocaleModel(0, "TestCompany", null, "CMP", ActionEnum.AddOrUpdate, new List<LocaleModel>() { chainModel });
            organizationModel.SequenceId = 1;
            return organizationModel;
        }

        private LocaleModel CreateStoreLocaleModel(int localeId, int? parentLocaleId, int businessUnitId, string name, string typeCode, DateTime openDate,
                                                DateTime closeDate, string ewicAgency, string posType, ActionEnum action,
                                                Models.LocaleAddress address, IEnumerable<LocaleTraitModel> localeTraitModelCollection)
        {
            LocaleModel localeModel = new LocaleModel(localeId, parentLocaleId, businessUnitId, name, typeCode, openDate, closeDate,
                                                      ewicAgency, action, address, localeTraitModelCollection);
            return localeModel;
        }

        private LocaleModel CreateLocaleModel(int localeId, string name, int? parentLocaleId, string typeCode, ActionEnum action, List<LocaleModel> childLocale)
        {
            LocaleModel localeModel = new LocaleModel(localeId, name, parentLocaleId, typeCode, action);
            localeModel.Locales = childLocale;
            return localeModel;
        }
    }
}