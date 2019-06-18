using System.Net.Http;

namespace KitBuilderWebApi.Helper
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; set; }
        string BaseUri { get; set; }
        void InitializeClient();


    }
}