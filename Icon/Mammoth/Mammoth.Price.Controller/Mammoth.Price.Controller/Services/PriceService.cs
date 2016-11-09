using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.Logging;
using Mammoth.Price.Controller.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Net.Http;
using System.Threading.Tasks;
using Mammoth.Common.ControllerApplication.Models;

namespace Mammoth.Price.Controller.Services
{
    public class PriceService : IService<PriceEventModel>
    {
        private IHttpClientWrapper httpClient;
        private ILogger logger;

        public PriceService(IHttpClientWrapper httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public void Process(List<PriceEventModel> data)
        {
            logger.Info(String.Format("Processing {0} Mammoth Price Events.", data.Count));

            if (data.Any())
            {
                SendData(data, Uris.PriceUpdate, IrmaEventTypes.Price);
                SendData(data, Uris.PriceRollback, IrmaEventTypes.PriceRollback);
            }
        }

        private void SendData(List<PriceEventModel> data, string uri, int eventTypeId)
        {
            IEnumerable<PriceEventModel> filteredData = data.Where(e => e.EventTypeId == eventTypeId);
            if (!filteredData.Any())
            {
                return;
            }

            HttpResponseMessage response = SendDataViaHttpClient(filteredData, uri);

            // If passing records in bulk failed process one at a time.
            if (!response.IsSuccessStatusCode)
            {
                logger.Error(String.Format("Error occurred when passing processing Price events. Processing each Price event one by one. URI : {0}. EventTypeId : {1}. Number of Price Events : {2}. Error : {3}.",
                    uri,
                    eventTypeId,
                    filteredData.Count(),
                    response.ReasonPhrase));

                // Send each record one at a time
                foreach (var item in filteredData)
                {
                    var singleResponse = SendDataViaHttpClient(new[] { item }, uri);
                    if (!singleResponse.IsSuccessStatusCode)
                    {
                        logger.Error(String.Format("Error occurred when processing Price events. URI : {0}. EventTypeId: {1}. QueueId : {2}. Scan Code {3}. Error : {4}.",
                            uri,
                            eventTypeId,
                            item.QueueId,
                            item.ScanCode,
                            response.ReasonPhrase));
                        item.ErrorMessage = singleResponse.ReasonPhrase;
                    }
                }
            }
            else if (response.Content != null)
            {
                var responseMessages = response.Content.ReadAsAsync<List<ErrorResponseModel<PriceEventModel>>>().Result;
                if (responseMessages != null)
                {
                    foreach (var responseMessage in responseMessages)
                    {
                        var matchingItems = filteredData.Where(item => item.ScanCode == responseMessage.Model.ScanCode && item.BusinessUnitId == responseMessage.Model.BusinessUnitId);
                        foreach (var matchingItem in matchingItems)
                        {
                            matchingItem.ErrorMessage = responseMessage.Error;
                        }
                    }
                }
            }
        }

        private HttpResponseMessage SendDataViaHttpClient(IEnumerable<PriceEventModel> data, string uri)
        {
            HttpResponseMessage response;
            try
            {
                IEnumerable<PriceModel> prices = data.MapToPriceModel();

                switch (uri)
                {
                    case Uris.PriceUpdate:
                        response = httpClient.PutAsJsonAsync(uri, prices).Result;
                        break;
                    case Uris.PriceRollback:
                        response = httpClient.PostAsJsonAsync(uri, prices).Result;
                        break;
                    default:
                        logger.Error(string.Format("There was an unsupported Uri used to pass price data. Uri: {0}.", uri));
                        response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                        break;
                }
            }
            catch (Exception)
            {
                response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}