using Mammoth.Common;
using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.Logging;
using Mammoth.Price.Controller.DataAccess.Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

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
            IEnumerable<PriceEventModel> filteredPriceData = data.Where(e => e.EventTypeId == eventTypeId);
            if (!filteredPriceData.Any())
            {
                return;
            }

            HttpResponseMessage response = SendDataViaHttpClient(filteredPriceData, uri);

            // If passing records in bulk failed process one at a time.
            if (!response.IsSuccessStatusCode)
            {
                logger.Error(String.Format("Error occurred when passing processing Price events. Processing Price events in batches. URI : {0}. EventTypeId : {1}. Number of Price Events : {2}. Error : {3}.",
                    uri,
                    eventTypeId,
                    filteredPriceData.Count(),
                    response.ReasonPhrase));

                // Send records to web api in batches recursively
                if (filteredPriceData.Count() > 1)
                {
                    var batches = filteredPriceData.Batch(filteredPriceData.Count() / 2);
                    foreach (var batch in batches)
                    {
                        SendData(batch.ToList(), uri, eventTypeId);
                    }
                }
                else
                {
                    // Set error properties for the one failed record
                    if (response.Content != null)
                    {
                        string errorResponseContent = response.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(errorResponseContent))
                        {
                            filteredPriceData.First().ErrorMessage = response.ReasonPhrase;
                            filteredPriceData.First().ErrorDetails = errorResponseContent;
                            filteredPriceData.First().ErrorSource = Constants.SourceSystem.MammothWebApi;
                        }
                    }
                }
            }
            else if (response.Content != null)
            {
                // set error properties for records returned within a successful response (e.g. BusinessUnit does not exist)
                var responseMessages = response.Content.ReadAsAsync<List<ErrorResponseModel<PriceEventModel>>>().Result;
                if (responseMessages != null)
                {
                    foreach (var responseMessage in responseMessages)
                    {
                        var matchingItems = filteredPriceData.Where(item => item.ScanCode == responseMessage.Model.ScanCode && item.BusinessUnitId == responseMessage.Model.BusinessUnitId);
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
            catch (Exception exception)
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}