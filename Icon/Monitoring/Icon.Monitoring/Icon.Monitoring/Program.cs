using Icon.Monitoring.Service;
using NLog;
using Topshelf;

namespace Icon.Monitoring
{
    class Program
    {
        static void Main()
        {
            Logger logger = LogManager.GetLogger(typeof(Program).FullName);
            logger.Info("Icon Monitoring service has started...");

            HostFactory.Run(m =>
            {
                var container = SimpleInjectorInitializer.InitializeContainer();
                m.Service<IMonitorService>(s =>
                {
                    s.ConstructUsing(c => container.GetInstance<IMonitorService>());
                    s.WhenStarted(cm => cm.Start());
                    s.WhenStopped(cm => cm.Stop());
                });

                m.SetDescription("Monitors the Queue and Staging tables to make sure the Push Controller and API Controller are running as expected.");
                m.SetDisplayName("Icon Controller Monitor");
                m.SetServiceName("IconControllerMonitor");                
            });
        }
    }
}