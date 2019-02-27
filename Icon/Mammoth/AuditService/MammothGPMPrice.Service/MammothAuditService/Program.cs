using Topshelf;
using System.Configuration;
using System.Reflection;

namespace Audit //MammothGpmPrice.Service
{
  class Program
	{
		static void Main(string[] args)
		{
			//Grant FULL access permission to account the service is running under to this folder:  \Users\All Users\Microsoft\Crypto\RSA\MachineKeys
			EncryptAppSettings("Upload");
			EncryptAppSettings("connectionStrings");

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

		static void EncryptAppSettings(string section)
		{
			var mods = Assembly.GetExecutingAssembly().GetModules();
			var cnfg = ConfigurationManager.OpenExeConfiguration(mods[0].FullyQualifiedName);
			var settings = cnfg.GetSection(section);
			if(settings == null) return;

			if(settings != null && !settings.SectionInformation.IsProtected)
			{
				settings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
				settings.SectionInformation.ForceSave = true;
				cnfg.Save(ConfigurationSaveMode.Modified);
			}
		}
	}
}