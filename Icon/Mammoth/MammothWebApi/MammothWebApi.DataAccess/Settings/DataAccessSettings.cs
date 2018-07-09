using Mammoth.Common;

namespace MammothWebApi.DataAccess.Settings
{
    public class DataAccessSettings
    {
        public int DatabaseRetryCount { get; set; }

        public static DataAccessSettings Load()
        {
            return new DataAccessSettings
            {
                DatabaseRetryCount = AppSettingsAccessor.GetIntSetting(nameof(DatabaseRetryCount))
            };
        }
    }
}
