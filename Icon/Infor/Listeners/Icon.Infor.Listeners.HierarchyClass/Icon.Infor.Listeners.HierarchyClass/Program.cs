using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Extensions;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Infor.Listeners.HierarchyClass.Notifier;
using Icon.Infor.Listeners.HierarchyClass.Queries;
using Icon.Infor.Listeners.HierarchyClass.Validators;
using Icon.Logging;
using SimpleInjector;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Topshelf;

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
            container.Register<IMessageParser<IEnumerable<HierarchyClassModel>>, HierarchyClassMessageParser>();
            container.Register(() => ListenerApplicationSettings.CreateDefaultSettings(ApplicationName));
            container.Register(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.Register<IEsbSubscriber>(() => new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()), Lifestyle.Singleton);
            container.Register<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.Register<ILogger<HierarchyClassListener>, NLogLogger<HierarchyClassListener>>();                     
            container.Register<ILogger<HierarchyClassMessageParser>, NLogLogger<HierarchyClassMessageParser>>();
            container.RegisterCollection<IHierarchyClassService>(new[] { typeof(IHierarchyClassService).Assembly });
            container.Register<List<string>>(() => ConfigurationManager.AppSettings[RegionsKey].ToRegionList());
            container.Register(typeof(IQueryHandler<,>), new List<Assembly> { Assembly.GetAssembly(typeof(HierarchyClassListener)) });
            container.Register(typeof(ICommandHandler<>), new List<Assembly> { Assembly.GetAssembly(typeof(HierarchyClassListener)) });
            container.Register<IHierarchyClassListenerNotifier, HierarchyClassListenerNotifier>();
            container.Register<ICollectionValidator<HierarchyClassModel>, HierarchyClassModelValidator>();
            return container;
        }
    }
}
