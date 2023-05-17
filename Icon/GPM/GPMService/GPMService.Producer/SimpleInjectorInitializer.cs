using GPMService.Producer.Archive;
using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
using GPMService.Producer.Listener.JustInTime;
using GPMService.Producer.Listener.NearRealTime;
using GPMService.Producer.Helpers;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Message.Processor;
using GPMService.Producer.Publish;
using GPMService.Producer.Serializer;
using GPMService.Producer.Service;
using GPMService.Producer.Service.Listener;
using GPMService.Producer.Settings;using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Infor;
using Icon.Esb.Schemas.Mammoth;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Mammoth.Framework;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.ExtendedClient.SNS;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.S3;
using Wfm.Aws.S3.Settings;
using Wfm.Aws.SNS;
using Wfm.Aws.SNS.Settings;
using Wfm.Aws.SQS;
using Wfm.Aws.SQS.Settings;

namespace GPMService.Producer
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(string serviceType)
        {
            Container container = new Container();
            container.RegisterSingleton<IEmailClient>(() => { return EmailClient.CreateFromConfig(); });
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton(() => GPMProducerServiceSettings.CreateSettings());
            container.RegisterSingleton<ErrorEventPublisher>();
            container.RegisterSingleton<ISerializer<ErrorMessage>, Serializer<ErrorMessage>>();
            RegisterServiceImplementation(container, serviceType);
            return container;
        }

        private static void RegisterServiceImplementation(Container container, string serviceType)
        {
            switch (serviceType)
            {
                case Constants.ProducerType.NearRealTime:
                    container.RegisterSingleton(() => S3FacadeSettings.CreateSettingsFromConfig());
                    container.RegisterSingleton(() => SQSFacadeSettings.CreateSettingsFromConfig());
                    container.RegisterSingleton(() => SQSExtendedClientListenerSettings.CreateSettingsFromConfig());
                    container.RegisterSingleton<IS3Facade, S3Facade>();
                    container.RegisterSingleton<ISQSFacade, SQSFacade>();
                    container.RegisterSingleton<IExtendedClientMessageSerializer, S3EventMessageSerializer>();
                    container.RegisterSingleton<ISQSExtendedClient, SQSExtendedClient>();
                    container.RegisterSingleton<IMessagePublisher, NearRealTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<NearRealTimeMessagePublisher>, NLogLogger<NearRealTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<NearRealTimeMessageListener>, NLogLogger<NearRealTimeMessageListener>>();
                    container.RegisterSingleton<ILogger<NearRealTimeMessageProcessor>, NLogLogger<NearRealTimeMessageProcessor>>();
                    container.RegisterSingleton<ILogger<NearRealTimeProcessorDAL>, NLogLogger<NearRealTimeProcessorDAL>>();
                    container.RegisterSingleton<ILogger<NearRealTimeProducerService>, NLogLogger<NearRealTimeProducerService>>();
                    container.RegisterSingleton<ILogger<ConfirmBODErrorHandler>, NLogLogger<ConfirmBODErrorHandler>>();
                    container.RegisterSingleton<ILogger<ProcessBODErrorHandler>, NLogLogger<ProcessBODErrorHandler>>();
                    container.RegisterSingleton<IMessageParser<items>, NearRealTimeMessageParser>();
                    container.RegisterSingleton<INearRealTimeProcessorDAL, NearRealTimeProcessorDAL>();
                    container.RegisterSingleton<IMessageProcessor, NearRealTimeMessageProcessor>();
                    container.RegisterSingleton<SQSExtendedClientListener<NearRealTimeMessageListener>, NearRealTimeMessageListener>();
                    container.RegisterSingleton<IGPMProducerService, NearRealTimeProducerService>();
                    container.RegisterSingleton<ConfirmBODErrorHandler>();
                    container.RegisterSingleton<ProcessBODErrorHandler>();
                    container.RegisterSingleton<ISerializer<ConfirmBODType>, Serializer<ConfirmBODType>>();
                    container.RegisterSingleton<ISerializer<PriceChangeMaster>, Serializer<PriceChangeMaster>>();
                    container.RegisterSingleton<ISerializer<MammothPriceType>, Serializer<MammothPriceType>>();
                    container.RegisterSingleton<ISerializer<PriceMessageArchiveType>, Serializer<PriceMessageArchiveType>>();
                    container.RegisterSingleton<ISerializer<items>, Serializer<items>>();
                    break;
                case Constants.ProducerType.JustInTime.ActivePrice:
                    S3FacadeSettings activePricePublishS3Settings = S3FacadeSettings.CreateSettingsFromConfig();
                    SNSFacadeSettings activePricePublishSNSSettings = SNSFacadeSettings.CreateSettingsFromConfig();
                    S3Facade activePricePublishS3Facade = new S3Facade(activePricePublishS3Settings);
                    SNSFacade activePricePublishSNSFacade = new SNSFacade(activePricePublishSNSSettings);
                    container.RegisterSingleton<ISNSExtendedClient>(() => new SNSExtendedClient(activePricePublishSNSFacade, activePricePublishS3Facade, new ExtendedClientMessageSerializer()));

                    S3FacadeSettings activePriceListenerS3Settings = S3FacadeSettings.CreateSettingsFromNamedConfig("ActivePriceListenerS3Config");
                    SQSFacadeSettings activePriceListenerSQSSettings = SQSFacadeSettings.CreateSettingsFromNamedConfig("ActivePriceListenerSQSConfig");
                    S3Facade activePriceListenerS3Facade = new S3Facade(activePriceListenerS3Settings);
                    SQSFacade activePriceListenerSQSFacade = new SQSFacade(activePriceListenerSQSSettings);
                    container.RegisterSingleton(() => SQSExtendedClientListenerSettings.CreateSettingsFromNamedConfig("ActivePriceListenerSQSExtendedClientConfig"));
                    container.RegisterSingleton<ISQSExtendedClient>(() => new SQSExtendedClient(activePriceListenerSQSFacade, activePriceListenerS3Facade, new S3EventMessageSerializer()));

                    container.RegisterSingleton<IMessagePublisher, JustInTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<JustInTimeMessagePublisher>, NLogLogger<JustInTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<ActivePriceMessageListener>, NLogLogger<ActivePriceMessageListener>>();
                    container.RegisterSingleton<ILogger<ActivePriceMessageProcessor>, NLogLogger<ActivePriceMessageProcessor>>();
                    container.RegisterSingleton<ILogger<ActivePriceProcessorDAL>, NLogLogger<ActivePriceProcessorDAL>>();
                    container.RegisterSingleton<ILogger<CommonDAL>, NLogLogger<CommonDAL>>();
                    container.RegisterSingleton<ILogger<ActivePriceProducerService>, NLogLogger<ActivePriceProducerService>>();
                    container.RegisterSingleton<ILogger<JustInTimePriceArchiver>, NLogLogger<JustInTimePriceArchiver>>();
                    container.RegisterSingleton<JustInTimePriceArchiver>();
                    container.RegisterSingleton<IMessageParser<JobSchedule>, ActivePriceMessageParser>();
                    container.RegisterSingleton<IActivePriceProcessorDAL, ActivePriceProcessorDAL>();
                    container.RegisterSingleton<ICommonDAL, CommonDAL>();
                    container.RegisterSingleton<IMessageProcessor, ActivePriceMessageProcessor>();
                    container.RegisterSingleton<SQSExtendedClientListener<ActivePriceMessageListener>, ActivePriceMessageListener>();
                    container.RegisterSingleton<IGPMProducerService, ActivePriceProducerService>();
                    container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
                    container.RegisterSingleton<ISerializer<JobSchedule>, Serializer<JobSchedule>>();
                    break;
                case Constants.ProducerType.JustInTime.ExpiringTpr:
                    S3FacadeSettings expiringTPRPublishS3Settings = S3FacadeSettings.CreateSettingsFromConfig();
                    SNSFacadeSettings expiringTPRPublishSNSSettings = SNSFacadeSettings.CreateSettingsFromConfig();
                    S3Facade expiringTPRPublishS3Facade = new S3Facade(expiringTPRPublishS3Settings);
                    SNSFacade expiringTPRPublishSNSFacade = new SNSFacade(expiringTPRPublishSNSSettings);
                    container.RegisterSingleton<ISNSExtendedClient>(() => new SNSExtendedClient(expiringTPRPublishSNSFacade, expiringTPRPublishS3Facade, new ExtendedClientMessageSerializer()));

                    S3FacadeSettings expiringTPRListenerS3Settings = S3FacadeSettings.CreateSettingsFromNamedConfig("ExpiringTPRListenerS3Config");
                    SQSFacadeSettings expiringTPRListenerSQSSettings = SQSFacadeSettings.CreateSettingsFromNamedConfig("ExpiringTPRListenerSQSConfig");
                    S3Facade expiringTPRListenerS3Facade = new S3Facade(expiringTPRListenerS3Settings);
                    SQSFacade expiringTPRListenerSQSFacade = new SQSFacade(expiringTPRListenerSQSSettings);
                    container.RegisterSingleton(() => SQSExtendedClientListenerSettings.CreateSettingsFromNamedConfig("ExpiringTPRListenerSQSExtendedClientConfig"));
                    container.RegisterSingleton<ISQSExtendedClient>(() => new SQSExtendedClient(expiringTPRListenerSQSFacade, expiringTPRListenerS3Facade, new S3EventMessageSerializer()));
                    
                    container.RegisterSingleton<IMessagePublisher, JustInTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<JustInTimeMessagePublisher>, NLogLogger<JustInTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<ExpiringTprMessageListener>, NLogLogger<ExpiringTprMessageListener>>();
                    container.RegisterSingleton<ILogger<ExpiringTprMessageProcessor>, NLogLogger<ExpiringTprMessageProcessor>>();
                    container.RegisterSingleton<ILogger<ExpiringTprProcessorDAL>, NLogLogger<ExpiringTprProcessorDAL>>();
                    container.RegisterSingleton<ILogger<CommonDAL>, NLogLogger<CommonDAL>>();
                    container.RegisterSingleton<ILogger<ExpiringTprProducerService>, NLogLogger<ExpiringTprProducerService>>();
                    container.RegisterSingleton<ILogger<JustInTimePriceArchiver>, NLogLogger<JustInTimePriceArchiver>>();
                    container.RegisterSingleton<JustInTimePriceArchiver>();
                    container.RegisterSingleton<IMessageParser<JobSchedule>, ExpiringTprMessageParser>();
                    container.RegisterSingleton<IExpiringTprProcessorDAL, ExpiringTprProcessorDAL>();
                    container.RegisterSingleton<ICommonDAL, CommonDAL>();
                    container.RegisterSingleton<IMessageProcessor, ExpiringTprMessageProcessor>();
                    container.RegisterSingleton<SQSExtendedClientListener<ExpiringTprMessageListener>, ExpiringTprMessageListener>();
                    container.RegisterSingleton<IGPMProducerService, ExpiringTprProducerService>();
                    container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
                    container.RegisterSingleton<ISerializer<JobSchedule>, Serializer<JobSchedule>>();
                    break;
                case Constants.ProducerType.JustInTime.EmergencyPrice:
                    container.RegisterSingleton(() => S3FacadeSettings.CreateSettingsFromConfig());
                    container.RegisterSingleton(() => SNSFacadeSettings.CreateSettingsFromConfig());
                    container.RegisterSingleton<IS3Facade, S3Facade>();
                    container.RegisterSingleton<ISNSFacade, SNSFacade>();
                    container.RegisterSingleton<IExtendedClientMessageSerializer, ExtendedClientMessageSerializer>();
                    container.RegisterSingleton<ISNSExtendedClient, SNSExtendedClient>();

                    container.RegisterSingleton<IMessagePublisher, JustInTimeMessagePublisher>();
                    container.RegisterSingleton<ILogger<JustInTimeMessagePublisher>, NLogLogger<JustInTimeMessagePublisher>>();
                    container.RegisterSingleton<ILogger<EmergencyPriceMessageProcessor>, NLogLogger<EmergencyPriceMessageProcessor>>();
                    container.RegisterSingleton<ILogger<EmergencyPriceProcessorDAL>, NLogLogger<EmergencyPriceProcessorDAL>>();
                    container.RegisterSingleton<ILogger<EmergencyPriceProducerService>, NLogLogger<EmergencyPriceProducerService>>();
                    container.RegisterSingleton<ILogger<CommonDAL>, NLogLogger<CommonDAL>>();
                    container.RegisterSingleton<ILogger<JustInTimePriceArchiver>, NLogLogger<JustInTimePriceArchiver>>();
                    container.RegisterSingleton<JustInTimePriceArchiver>();
                    container.RegisterSingleton<IEmergencyPriceProcessorDAL, EmergencyPriceProcessorDAL>();
                    container.RegisterSingleton<ICommonDAL, CommonDAL>();
                    container.RegisterSingleton<IMessageProcessor, EmergencyPriceMessageProcessor>();
                    container.RegisterSingleton<IGPMProducerService, EmergencyPriceProducerService>();
                    container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
                    container.RegisterSingleton<ISerializer<MammothPriceType>, Serializer<MammothPriceType>>();
                    break;
                default:
                    throw new ArgumentException(
                        $"No type implementation exists for service type argument {serviceType}");
            }
        }
    }
}
