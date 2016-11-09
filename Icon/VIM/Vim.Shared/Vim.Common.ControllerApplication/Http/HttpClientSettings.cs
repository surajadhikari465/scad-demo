namespace Vim.Common.ControllerApplication.Http
{
    public class HttpClientSettings
    {
        public string BaseAddress { get; set; }

        public static HttpClientSettings CreateFromConfig()
        {
            return new HttpClientSettings
            {
                BaseAddress = AppSettingsAccessor.GetStringSetting("BaseAddress"),
            };
        }
    }
}