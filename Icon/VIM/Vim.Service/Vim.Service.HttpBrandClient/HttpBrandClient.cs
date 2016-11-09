namespace Vim.Service.Brand.HttpClient
{
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Vim.Service.Models;

    public class HttpBrandClient : IVimBrandClient
    {
        private readonly Uri serviceUri = new Uri(
            string.Format(
                "{0}/{1}",
                HttpBrandClientSettings.Instance.BaseAddress,
                HttpBrandClientSettings.Instance.EndPointAddress));

        public async Task<VimBrandResponse> SendBrandsToVimAsync(IEnumerable<VimBrand> brands)
        {
            var client = new RestClient(this.serviceUri);
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(brands);

            var sendBrandsTask = client.ExecuteTaskAsync(request);

            var response = await sendBrandsTask;

            return new VimBrandResponse
            {
                IsSuccessful = response.StatusCode == HttpStatusCode.OK
                    ? true
                    : false,
                FailureMessage = response.StatusDescription
            };
        }
    }
}
