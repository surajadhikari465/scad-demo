using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Infor.ContractTypes;
using Icon.Esb.Services.ConfirmationBod;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Decorators;
using Icon.Infor.Listeners.HierarchyClass.ErrorUtility;
using Icon.Infor.Listeners.HierarchyClass.EsbService;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using Icon.Infor.Listeners.HierarchyClass.MessageBuilders;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Infor.Listeners.HierarchyClass.Notifier;
using Icon.Infor.Listeners.HierarchyClass.Requests;
using Icon.Infor.Listeners.HierarchyClass.Validators;
using Icon.Logging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Topshelf;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class Program
    {
        private const string ApplicationName = "Infor HierarchyClass Listener";
        private const string RegionsKey = "Regions";

        static void Main(string[] args)
        {
            HostFactory.Run(r =>
            {
                r.Service<IListenerApplication>(s =>
                {
                    s.ConstructUsing(() => CreateHierarchyClassListener().GetInstance<IListenerApplication>());
                    s.WhenStarted(cm => cm.Run());
                    s.WhenStopped(cm => cm.Close());
                });
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
            });
        }

        public static Container CreateHierarchyClassListener()
        {
            var container = new Container();

            container.Register<IListenerApplication, HierarchyClassListener>();
            container.Register<IMessageParser<IEnumerable<InforHierarchyClassModel>>, HierarchyClassMessageParser>();
            container.Register(() => ListenerApplicationSettings.CreateDefaultSettings(ApplicationName));
            container.Register<IEsbSubscriber, EsbSubscriber>(Lifestyle.Singleton);
            container.Register<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.Register<List<string>>(() => ConfigurationManager.AppSettings[RegionsKey].ToRegionList());
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register(typeof(IQueryHandler<,>), new List<Assembly> { Assembly.GetAssembly(typeof(HierarchyClassListener)) });
            container.Register(typeof(ICommandHandler<>), new List<Assembly> { Assembly.GetAssembly(typeof(HierarchyClassListener)) });
            container.Register<IHierarchyClassListenerNotifier, HierarchyClassListenerNotifier>();
            container.Register<ICollectionValidator<InforHierarchyClassModel>, HierarchyClassModelValidator>();
            container.Register<IEsbService<HierarchyClassEsbServiceRequest>, HierarchyClassEsbService>();
            container.Register<IMessageBuilder<HierarchyClassEsbServiceRequest>, HierarchyClassMessageBuilder>();
            container.Register<ISerializer<Contracts.HierarchyType>, Serializer<Contracts.HierarchyType>>();
            container.Register<IHierarchyClassListenerSettings>(() => HierarchyClassListenerSettings.CreateFromConfig(), Lifestyle.Singleton);
            container.Register<IDbContextFactory<IconContext>, IconDbContextFactory>();

            container.Register<VimEsbConnectionSettings>();
            container.Register(() => EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("Infor"), Lifestyle.Singleton);
            container.Register<VimEsbConnectionFactory>();

            container.Register<IEsbConnectionFactory, EsbConnectionFactory>();
            container.Register<IMessageBuilder<ConfirmationBodEsbRequest>, ConfirmationBodMessageBuilder>();
            container.Register<IEsbService<ConfirmationBodEsbRequest>>(() => GetConfirmationBodEsbService(container));
            container.Register<ISerializer<ConfirmBODType>, Serializer<ConfirmBODType>>();

            container.RegisterDecorator<ICommandHandler<AddOrUpdateHierarchyClassesCommand>, RetryCommandHandlerDecorator<AddOrUpdateHierarchyClassesCommand>>();
            container.RegisterDecorator<ICommandHandler<DeleteHierarchyClassesCommand>, RetryCommandHandlerDecorator<DeleteHierarchyClassesCommand>>();

            container.Register<IErrorMapper, ErrorMapper>();

            var types = GetHierarchyClassServices();

            container.RegisterCollection<IHierarchyClassService>(types);

            return container;
        }

        private static IEsbService<ConfirmationBodEsbRequest> GetConfirmationBodEsbService(Container container)
        {
            ConfirmationBodEsbService confirmationBodEsbService =
                new ConfirmationBodEsbService
                (
                    EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("ConfirmBOD"),
                    container.GetInstance<IEsbConnectionFactory>(),
                    container.GetInstance<IMessageBuilder<ConfirmationBodEsbRequest>>()
                );

            return confirmationBodEsbService;
        }

        private static IEnumerable<Type> GetHierarchyClassServices()
        {
            var names = AppSettingsAccessor.GetStringSetting("HierarchyClassServices")
                .Split(',')
                .Select(n => n.Trim() + "Service")
                .ToList();

            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IHierarchyClassService).IsAssignableFrom(t) && t.IsClass && names.Contains(t.Name))
                .OrderBy(t => names.IndexOf(t.Name))
                .ToList();

            return types;
        }
    }
}
