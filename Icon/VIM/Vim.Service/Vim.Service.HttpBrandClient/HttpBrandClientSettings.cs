namespace Vim.Service.Brand.HttpClient
{
    using Icon.Common;
    using System;

    public sealed class HttpBrandClientSettings
    {
        private static readonly Lazy<HttpBrandClientSettings> lazy = 
            new Lazy<HttpBrandClientSettings>(() => new HttpBrandClientSettings());

        public static HttpBrandClientSettings Instance { get { return lazy.Value; } }

        private HttpBrandClientSettings()
        {
            this.BaseAddress = AppSettingsAccessor.GetStringSetting(AppSettings.BaseAddress, true);
            this.EndPointAddress = AppSettingsAccessor.GetStringSetting(AppSettings.EndPointAddress, true);
        }

        public string BaseAddress { get; set; }
        public string EndPointAddress { get; set; }

    }
}
