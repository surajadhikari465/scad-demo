﻿using Icon.Common;
using WebSupport.Managers;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(WebSupport.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace WebSupport.App_Start
{
    using DataAccess;
    using Esb.Core.EsbServices;
    using Esb.Core.MessageBuilders;
    using Esb.Core.Serializer;
    using Icon.Common.DataAccess;
    using Icon.Esb;
    using Icon.Esb.Factory;
    using Icon.Esb.Schemas.Wfm.Contracts;
    using Icon.Logging;
    using Mammoth.Framework;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Web.Mvc;
    using WebSupport.DataAccess.Models;
    using WebSupport.Models;
    using WebSupport.Services;
    using WebSupport.ViewModels;
    using WebSupport.DataAccess.Queries;
    using System.Collections.Generic;
    using WebSupport.DataAccess.Commands;
    using MessageBuilders;
    using WebSupport.Clients;

    public class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            // TODO: Have reviewed by someone for lifetime management.
            container.Register(() => CreateClientIdManagerInstance());
            container.RegisterSingleton<ILogger>(() => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)));
            container.Register<IIrmaContextFactory, IrmaContextFactory>(Lifestyle.Scoped);
            container.Register(typeof(ICommandHandler<>), new[] { Assembly.Load("WebSupport.DataAccess") }, Lifestyle.Scoped);
            container.Register(typeof(IQueryHandler<,>), new[] { Assembly.Load("WebSupport.DataAccess") }, Lifestyle.Scoped);
            container.RegisterSingleton<IDvsNearRealTimePriceClient, DvsNearRealTimePriceClient>();
            container.RegisterSingleton<IMammothGpmBridgeClient, MammothGpmBridgeClient>();
            container.RegisterSingleton<IJobSchedulerBridgeClient, JobSchedulerBridgeClient>();
            container.Register<IEsbService<JobScheduleModel>>(() => CreateJobScheduleMessageService(container), Lifestyle.Scoped);
            container.Register<IEsbService<PriceResetRequestViewModel>>(() => CreatePriceResetMessageService(container), Lifestyle.Scoped);
            container.Register<IEsbService<PricesAllViewModel>>(() => CreatePricesAllMessageService(container), Lifestyle.Scoped);
            container.Register<IEsbMultipleMessageService<CheckPointRequestViewModel>>(() => CreateCheckPointRequestMessageService(container), Lifestyle.Scoped);
            container.Register<MammothContext>(Lifestyle.Scoped);
            container.Register<IMessageBuilder<PriceResetMessageBuilderModel>, PriceResetMessageBuilder>(Lifestyle.Scoped);
            container.Register<IMessageBuilder<CheckPointRequestBuilderModel>, CheckPointRequestMessageBuilder>(Lifestyle.Scoped);
            container.Register<ISerializer<Icon.Esb.Schemas.Mammoth.ContractTypes.JobSchedule>, Serializer<Icon.Esb.Schemas.Mammoth.ContractTypes.JobSchedule>>(Lifestyle.Scoped);
            container.Register<ISerializer<items>, Serializer<items>>(Lifestyle.Scoped);
            container.Register<ISerializer<Icon.Esb.Schemas.Infor.ContractTypes.PriceChangeMaster>, SerializerWithoutNamepaceAliases<Icon.Esb.Schemas.Infor.ContractTypes.PriceChangeMaster>>(Lifestyle.Scoped);
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString), Lifestyle.Scoped);
            container.Register<IRefreshPriceService, RefreshPriceService>();
            container.Register<IPriceRefreshMessageBuilderFactory, PriceRefreshMessageBuilderFactory>();
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
        }

        private static WebSupportPriceMessageService CreatePriceResetMessageService(Container container)
        {
            return new WebSupportPriceMessageService(
                container.GetInstance<IMessageBuilder<PriceResetMessageBuilderModel>>(),
                container.GetInstance<IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>>>(),
                container.GetInstance<ICommandHandler<SaveSentMessageCommand>>(),
                container.GetInstance<IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>>>(),
                CreateClientIdManagerInstance(),
                container.GetInstance<IDvsNearRealTimePriceClient>());
        }

        private static WebSupportCheckPointRequestMessageService CreateCheckPointRequestMessageService(Container container)
        {
            return new WebSupportCheckPointRequestMessageService(
                container.GetInstance<ILogger>(),
                container.GetInstance<IMessageBuilder<CheckPointRequestBuilderModel>>(),
                container.GetInstance<IQueryHandler<GetCheckPointMessageParameters, IEnumerable<CheckPointMessageModel>>>(),
                container.GetInstance<ICommandHandler<ArchiveCheckpointMessageCommandParameters>>(),
                container.GetInstance<IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>>>(), 
                CreateClientIdManagerInstance(),
                container.GetInstance<IMammothGpmBridgeClient>());
        }

        private static WebSupportJobScheduleMessageService CreateJobScheduleMessageService(Container container)
        {
            return new WebSupportJobScheduleMessageService(
                container.GetInstance<ISerializer<Icon.Esb.Schemas.Mammoth.ContractTypes.JobSchedule>>(),
                container.GetInstance<ILogger>(),
                container.GetInstance<IJobSchedulerBridgeClient>());
        }

        private static WebSupportPriceAllMessageService CreatePricesAllMessageService(Container container)
        {
            return new WebSupportPriceAllMessageService(
                container.GetInstance<ILogger>(),
                container.GetInstance<IMessageBuilder<PriceResetMessageBuilderModel>>(),
                container.GetInstance<IQueryHandler<GetPricesAllParameters, List<PriceResetPrice>>>(),
                container.GetInstance<ICommandHandler<SaveSentMessageCommand>>(),
                CreateClientIdManagerInstance(),
                container.GetInstance<IDvsNearRealTimePriceClient>());
        }

        private static IClientIdManager CreateClientIdManagerInstance()
        {
            var client = new Managers.ClientIdManager();
            client.Initialize(AppSettingsAccessor.GetStringSetting("AppName", "WebSupport"));
            return client;
        }
    }
}