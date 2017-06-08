using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Framework;
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
using TIBCO.EMS;
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

            container.RegisterSingleton<IconContext>();
            container.RegisterSingleton<IRenewableContext<IconContext>>(() => new GlobalContext<IconContext>());
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

            container.Register<VimEsbConnectionSettings>();
            container.Register(() => EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("Infor"), Lifestyle.Singleton);
            container.Register<VimEsbConnectionFactory>();

            container.Register<IEsbConnectionFactory, VimEsbConnectionFactory>();

            var types = GetHierarchyClassServices();

            container.RegisterCollection<IHierarchyClassService>(types);

            return container;
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
