using Icon.Common;

namespace Icon.Esb.ListenerApplication
{
    public class ListenerApplicationSettings : IListenerApplicationSettings
    {
        public string ListenerApplicationName { get; set; }
        public string EmailSubjectPrefix { get; set; }
        public string EmailSubjectConnectionSuccess { get; set; }
        public string EmailSubjectError { get; set; }
        public int NumberOfListenerThreads { get; set; }

        protected virtual void LoadSubSettings() { }

        public static ListenerApplicationSettings CreateDefaultSettings(string applicationName)
        {
            var settings = new ListenerApplicationSettings
            {
                ListenerApplicationName = applicationName,
                EmailSubjectPrefix = applicationName + " : "
            };

            settings.EmailSubjectConnectionSuccess = settings.EmailSubjectPrefix + "Connection Success";
            settings.EmailSubjectError = settings.EmailSubjectPrefix + "Error Occurred";
            settings.NumberOfListenerThreads = AppSettingsAccessor.GetIntSetting("NumberOfListenerThreads", 1);

            return settings;
        }

        public static T CreateDefaultSettings<T>(string applicationName) where T : ListenerApplicationSettings, new()
        {
            var settings = new T
            {
                ListenerApplicationName = applicationName,
                EmailSubjectPrefix = applicationName + " : " 
            };

            settings.EmailSubjectConnectionSuccess = settings.EmailSubjectPrefix + "Connection Success";
            settings.EmailSubjectError = settings.EmailSubjectPrefix + "Error Occurred";
            settings.NumberOfListenerThreads = AppSettingsAccessor.GetIntSetting("NumberOfListenerThreads", 1);
            settings.LoadSubSettings();

            return settings;
        }
    }
}
