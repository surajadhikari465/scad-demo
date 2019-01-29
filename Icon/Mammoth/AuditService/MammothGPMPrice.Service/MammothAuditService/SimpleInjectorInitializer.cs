using System.Configuration;
using Mammoth.Logging;
using SimpleInjector;

namespace Audit
{
	public class SimpleInjectorInitializer
	{
		public static Container InitializeContainer()
		{
			var value = new Container();
			value.RegisterSingleton<ILogger>(() => new NLoggerInstance(typeof(NLoggerInstance), ConfigurationManager.AppSettings["InstanceID"]));
      value.RegisterSingleton<Audit.AuditService>();

			return value;
		}
	}
}