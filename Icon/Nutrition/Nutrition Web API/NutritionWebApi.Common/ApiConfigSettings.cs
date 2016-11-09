using System.Configuration;

namespace NutritionWebApi.Common
{
    public class ApiConfigSettings
    {
        private static ApiConfigSettings appSettings = null;
        private string dbConnectionString;

        private ApiConfigSettings()
        {
            dbConnectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
        }

        public static ApiConfigSettings Instance
        {
            get
            {
                if (appSettings == null)
                {
                    appSettings = new ApiConfigSettings();
                }
                return appSettings;
            }
        }

        public string ConnectionString
        {
            get
            {
                return dbConnectionString;
            }
        }
    }
}
