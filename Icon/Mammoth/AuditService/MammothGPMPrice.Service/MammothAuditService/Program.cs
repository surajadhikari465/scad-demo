using Topshelf;
using System.Configuration;

namespace Audit
{
    class Program
    {
        static void Main(string[] args)
        {
            //Grant FULL access permission to account the service is running under to this folder:  \Users\All Users\Microsoft\Crypto\RSA\MachineKeys
            Utility.EncryptAppSettings(Utility.VARIABLES, Utility.SQL_CONNECTIONS);

            HostFactory.Run(configure =>
            {
                var container = SimpleInjectorInitializer.InitializeContainer();
                container.Verify();

                configure.Service<AuditService>(service =>
                        {
                            service.ConstructUsing(s => container.GetInstance<AuditService>());
                            service.WhenStarted(s => s.Start());
                            service.WhenStopped(s => s.Stop());
                        });

                configure.RunAsLocalSystem();
                configure.SetServiceName(ConfigurationManager.AppSettings["ServiceName"]);
                configure.SetDisplayName(ConfigurationManager.AppSettings["DisplayName"]);
                configure.SetDescription(ConfigurationManager.AppSettings["Description"]);
            });
        }
    }
}