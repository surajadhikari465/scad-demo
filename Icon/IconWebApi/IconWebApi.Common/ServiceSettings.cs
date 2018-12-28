using System.Configuration;

namespace IconWebApi.Common
{

	public class ServiceSettings : IServiceSettings
	{
		public string ConnectionString { get; set; }

		public static ServiceSettings CreateFromConfig()
		{
			var settings = new ServiceSettings();
			settings.ConnectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
			return settings;
		}
	}
}
