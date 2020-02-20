using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Wfm.OutOfStock.Api.Legacy;
using WFM.OutOfStock.API.Domain;
using WFM.OutOfStock.API.Domain.Request;
using WFM.OutOfStock.API.Domain.Response;
using WFM.OutOfStock.API.Services.Exceptions;

namespace WFM.OutOfStock.API.Services
{
    public sealed class OutOfStockService : IOutOfStockService
    {
        private readonly string _endpointUri;

        public OutOfStockService(IOptions<AppConfiguration> options)
        {
            _endpointUri = options.Value.ServiceUri;
        }

        public async Task<List<StoreResponse>> RetrieveStoresForRegion(string regionCode)
        {
            var stores = await MakeServiceRequest(client => client.StoreAbbreviationsForAsync(regionCode));

            return stores
                .Select(s => new StoreResponse(s))
                .ToList();
        }

        public async Task SubmitList(UploadItemsRequest list)
        {
            DateTime scanDate = DateTime.Now;
            string regionAbbrev = list.RegionCode;
            string storeAbbrev = list.StoreName;
            string[] upcs = list.Items;
            string userName = list.UserName;
            string userEmail = list.UserEmail;
            string sessionId = Guid.NewGuid().ToString();

            await MakeServiceRequest(client => client.ScanProductsByStoreAbbreviationAsync(scanDate, regionAbbrev, storeAbbrev, upcs, userName, userEmail, sessionId));
        }

        // Following best practices for handling WCF ServiceClient lifecycle as documented here:
        // https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/use-close-abort-release-wcf-client-resources
        private async Task<T> MakeServiceRequest<T>(Func<IOosBackend, Task<T>> request)
        {
            var serviceClient = new OosBackendClient(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(_endpointUri));

            try
            {
                var result = await request(serviceClient);

                serviceClient.Close();
                return result;
            }
            catch (CommunicationException cex)
            {
                serviceClient.Abort();
                throw new ServiceCommunicationException(
                    "Encountered an error communicating with downstream Out Of Stock WCF service.", cex);
            }
            catch (TimeoutException tex)
            {
                serviceClient.Abort();
                throw new ServiceCommunicationException("Connection to Out Of Stock WCF service timed out.", tex);
            }
            catch (Exception)
            {
                serviceClient.Abort();
                throw;
            }
        }

        private async Task MakeServiceRequest(Func<IOosBackend, Task> request)
        {
            var serviceClient = new OosBackendClient(
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress(_endpointUri));

            try
            {
                await request(serviceClient);

                serviceClient.Close();
            }
            catch (CommunicationException cex)
            {
                serviceClient.Abort();
                throw new ServiceCommunicationException(
                    "Encountered an error communicating with downstream Out Of Stock WCF service.", cex);
            }
            catch (TimeoutException tex)
            {
                serviceClient.Abort();
                throw new ServiceCommunicationException("Connection to Out Of Stock WCF service timed out.", tex);
            }
            catch (Exception)
            {
                serviceClient.Abort();
                throw;
            }
        }
    }
}