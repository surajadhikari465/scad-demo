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
    using System;
    using WebSupport.DataAccess.Queries;
    using System.Collections.Generic;
    using WebSupport.DataAccess.Commands;
    using MessageBuilders;
    using WebSupport.Controllers;
    using WebSupport.EsbProducerFactory;
    using Mammoth.PrimeAffinity.Library.Esb;
    using Quartz.Impl;
    using Quartz;
    using WebSupport.BackgroundJobs;
    using WebSupport.PrimeAffinity;
    using Mammoth.PrimeAffinity.Library.Processors;
    using Mammoth.PrimeAffinity.Library.MessageBuilders;
    using Mammoth.PrimeAffinity.Library.Commands;

    public class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            ScheduleEsbReconnectJob(container);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            // TODO: Have reviewed by someone for lifetime management.
            container.RegisterSingleton<ILogger>(() => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)));
            container.Register<IIrmaContextFactory, IrmaContextFactory>(Lifestyle.Scoped);
            container.Register(typeof(ICommandHandler<>), new[] { Assembly.Load("WebSupport.DataAccess") }, Lifestyle.Scoped);
            container.Register(typeof(IQueryHandler<,>), new[] { Assembly.Load("WebSupport.DataAccess") }, Lifestyle.Scoped);
            container.Register<IEsbService<JobScheduleModel>>(() => CreateJobScheduleMessageService(container), Lifestyle.Scoped);
            container.Register<IEsbService<PriceResetRequestViewModel>>(() => CreatePriceResetMessageService(container), Lifestyle.Scoped);
            container.Register<IEsbService<CheckPointRequestViewModel>>(() => CreateCheckPointRequestMessageService(container), Lifestyle.Scoped);
            container.Register<IEsbConnectionFactory, EsbConnectionFactory>(Lifestyle.Scoped);
            container.Register<MammothContext>(Lifestyle.Scoped);
            container.Register<IMessageBuilder<PriceResetMessageBuilderModel>, PriceResetMessageBuilder>(Lifestyle.Scoped);
            container.Register<IMessageBuilder<CheckPointRequestBuilderModel>, CheckPointRequestMessageBuilder>(Lifestyle.Scoped);
            container.Register<ISerializer<Icon.Esb.Schemas.Mammoth.ContractTypes.JobSchedule>, Serializer<Icon.Esb.Schemas.Mammoth.ContractTypes.JobSchedule>>(Lifestyle.Scoped);
            container.Register<ISerializer<items>, Serializer<items>>(Lifestyle.Scoped);
            container.Register<ISerializer<Icon.Esb.Schemas.Infor.ContractTypes.PriceChangeMaster>, SerializerWithoutNamepaceAliases<Icon.Esb.Schemas.Infor.ContractTypes.PriceChangeMaster>>(Lifestyle.Scoped);
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString), Lifestyle.Scoped);
            container.Register<IRefreshPriceService, RefreshPriceService>();
            container.Register<IPriceRefreshEsbProducerFactory, PriceRefreshEsbProducerFactory>();
            container.Register<IPriceRefreshMessageBuilderFactory, PriceRefreshMessageBuilderFactory>();
            container.Register<ReconnectToEsbJob>();
            container.Register(() => PrimeAffinitySettings.Load());
            container.Register(() => PrimeAffinityPsgProcessorSettings.Load());
            container.Register(() => PrimeAffinityMessageBuilderSettings.Load());
            container.Register<IMessageBuilder<PrimeAffinityMessageBuilderParameters>, PrimeAffinityMessageBuilder>();
            container.Register<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>, PrimeAffinityPsgProcessor>();
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register(typeof(Mammoth.Common.DataAccess.CommandQuery.ICommandHandler<>), new[] { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(ArchivePrimeAffinityMessageCommandHandler)) });

            EsbConnectionCache.EsbConnectionSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("PrimeAffinityPsg");
            EsbConnectionCache.InitializeConnectionFactoryAndConnection();
        }

        private static WebSupportPriceMessageService CreatePriceResetMessageService(Container container)
        {
            return new WebSupportPriceMessageService(
                container.GetInstance<IEsbConnectionFactory>(),
                EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("EsbEmsConnection"),
                container.GetInstance<IMessageBuilder<PriceResetMessageBuilderModel>>(),
                container.GetInstance<IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>>>(),
                container.GetInstance<ICommandHandler<SaveSentMessageCommand>>());
        }

        private static WebSupportCheckPointRequestMessageService CreateCheckPointRequestMessageService(Container container)
        {
            return new WebSupportCheckPointRequestMessageService(
                container.GetInstance<IEsbConnectionFactory>(),
                EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("EsbEmsCheckPointRequestConnection"),
                container.GetInstance<IMessageBuilder<CheckPointRequestBuilderModel>>(),
                container.GetInstance<IQueryHandler<GetCheckPointMessageParameters, CheckPointMessageModel>>());
        }

        private static WebSupportJobScheduleMessageService CreateJobScheduleMessageService(Container container)
        {
            return new WebSupportJobScheduleMessageService(
                container.GetInstance<ISerializer<Icon.Esb.Schemas.Mammoth.ContractTypes.JobSchedule>>(),
                container.GetInstance<ILogger>())
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("Sb1EmsConnection")
            };
        }

        private static void ScheduleEsbReconnectJob(Container container)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.JobFactory = new JobFactory(container);
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<ReconnectToEsbJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(
                    s => s.WithIntervalInSeconds(3)
                        .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}