using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MoreLinq;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common;

namespace Mammoth.ItemLocale.Controller.Services
{
    public class ItemLocaleService : IService<ItemLocaleEventModel>
    {
        private IHttpClientWrapper httpClient;
        private ILogger logger;
        private ItemLocaleControllerApplicationSettings settings;

        public ItemLocaleService(IHttpClientWrapper httpClient, ILogger logger, ItemLocaleControllerApplicationSettings settings)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.settings = settings;
        }

        public void Process(List<ItemLocaleEventModel> data)
        {
            logger.Debug(String.Format("Processing {0} Mammoth Item Locale rows for the {1} Region.", data.Count, settings.CurrentRegion));

            if (data.Any())
            {
                SendData(data, IrmaEventTypes.ItemLocaleAddOrUpdate);
                SendData(data, IrmaEventTypes.ItemDelete);  // Note: currently ItemDeletes are using the same URI as ItemLocaleAddOrUpdates 
                SendData(data, IrmaEventTypes.ItemDeauthorization);
            }
        }

        private void SendData(List<ItemLocaleEventModel> data, int eventTypeId)
        {
            IEnumerable<ItemLocaleEventModel> filteredItemLocaleData = data.Where(d => d.EventTypeId == eventTypeId);
            if (!filteredItemLocaleData.Any())
            {
                return;
            }

            // Pass to Web Api in batches using app setting using Morelinq Batch extension method.
            // This is to limit the number of rows sent via HTTP request
            var batches = filteredItemLocaleData.Batch(this.settings.ApiRowLimit);

            foreach (var batch in batches)
            {
                HttpResponseMessage response;

                switch (eventTypeId)
                {
                    case IrmaEventTypes.ItemLocaleAddOrUpdate:
                    case IrmaEventTypes.ItemDelete:
                        response = httpClient.PutAsJsonAsync(Uris.ItemLocaleUpdate, batch).Result;
                        break;
                    case IrmaEventTypes.ItemDeauthorization:
                        response = httpClient.PutAsJsonAsync(Uris.DeauthorizeItemLocale, batch).Result;
                        break;
                    default:
                        response = httpClient.PutAsJsonAsync(Uris.ItemLocaleUpdate, batch).Result;
                        break;
                }

            // If passing main batch of records fails, process in smaller bathes/bundles
            if (!response.IsSuccessStatusCode)
            {
                logger.Error(String.Format("Error occurred when processing ItemLocale events. Processing this set of events in batches. " +
                    "Region: {0}. URI : {1}. EventTypeId : {2}. Number of ItemLocale Rows : {3}. Number of QueueIDs: {4}. Error : {5}.",
                        settings.CurrentRegion,
                        settings.UriBaseAddress + Uris.ItemLocaleUpdate,
                        eventTypeId,
                        batch.Count(),
                        batch.DistinctBy(d => d.QueueId).Count(),
                        response.ReasonPhrase));

                // Send records to web api in batches/bundles recursively
                if (filteredItemLocaleData.Count() > 1)
                {
                    var bundles = filteredItemLocaleData.Batch(filteredItemLocaleData.Count() / 2);
                    foreach (var bundle in bundles)
                    {
                        SendData(bundle.ToList(), eventTypeId);
                    }
                }
                else
                {
                    string errorResponseContent = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(errorResponseContent))
                    {
                        filteredItemLocaleData.First().ErrorMessage = response.ReasonPhrase;
                        filteredItemLocaleData.First().ErrorDetails = errorResponseContent;
                        filteredItemLocaleData.First().ErrorSource = Constants.SourceSystem.MammothWebApi;
                    }
                }
            }
            else if (response.Content != null)
            {
                var responseMessages = response.Content.ReadAsAsync<List<ErrorResponseModel<ItemLocaleEventModel>>>().Result;
                if (responseMessages != null)
                {
                    foreach (var responseMessage in responseMessages)
                    {
                        var matchingItems = filteredItemLocaleData.Where(item => item.ScanCode == responseMessage.Model.ScanCode && item.BusinessUnitId == responseMessage.Model.BusinessUnitId);
                        foreach (var matchingItem in matchingItems)
                        {
                            matchingItem.ErrorMessage = responseMessage.Error;
                        }
                    }
                }
            }
        }
    }
}
}