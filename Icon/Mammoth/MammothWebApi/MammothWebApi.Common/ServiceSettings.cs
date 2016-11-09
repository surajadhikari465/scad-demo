using System.Configuration;

namespace MammothWebApi.Common
{
    public class ServiceSettings : IServiceSettings
    {
        public string ConnectionString { get; set; }

        public static ServiceSettings CreateFromConfig()
        {
            var settings = new ServiceSettings();
            settings.ConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            return settings;
        }
    }
}
