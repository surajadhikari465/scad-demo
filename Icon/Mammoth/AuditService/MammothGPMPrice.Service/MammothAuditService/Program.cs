using Topshelf;
using System.Configuration;

namespace Audit //MammothGpmPrice.Service
{
  class Program
	{
		static void Main(string[] args)
		{
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