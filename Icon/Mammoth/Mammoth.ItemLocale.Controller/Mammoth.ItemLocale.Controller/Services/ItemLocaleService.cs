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
            }
        }

        private void SendData(List<ItemLocaleEventModel> data, int eventTypeId)
        {
            IEnumerable<ItemLocaleEventModel> filteredData = data.Where(d => d.EventTypeId == eventTypeId);
            if (!filteredData.Any())
            {
                return;
            }

            // Pass to Web Api in batches using app setting using Morelinq Batch extension method.
            var batches = filteredData.Batch(this.settings.ApiRowLimit);
            foreach (var batch in batches)
            {
                HttpResponseMessage response = httpClient.PutAsJsonAsync(Uris.ItemLocaleUpdate, batch).Result;

                // If passing records in bulk failed process one at a time.
                if (!response.IsSuccessStatusCode)
                {
                    logger.Error(String.Format("Error occurred when passing processing ItemLocale events. Processing each ItemLocale event one by one. " +
                        "Region: {0}. URI : {1}. EventTypeId : {2}. Number of ItemLocale Rows : {3}. Number of QueueIDs: {4}. Error : {5}.",
                            settings.CurrentRegion,
                            settings.UriBaseAddress + Uris.ItemLocaleUpdate,
                            eventTypeId,
                            batch.Count(),
                            batch.DistinctBy(d => d.QueueId).Count(),
                            response.ReasonPhrase));

                    // Send each record one at a time
                    foreach (var item in batch)
                    {
                        var singleResponse = httpClient.PutAsJsonAsync(Uris.ItemLocaleUpdate, new[] { item }).Result;
                        if (!singleResponse.IsSuccessStatusCode)
                        {
                            logger.Error(String.Format("Error occurred when processing ItemLocale events. " +
                                "Region: {0}. URI : {1}. EventTypeId: {2}. QueueId : {3}. ScanCode: {4}. BusinessUnit: {5}. Error : {6}.",
                                    settings.CurrentRegion,
                                    settings.UriBaseAddress + Uris.ItemLocaleUpdate,
                                    eventTypeId,
                                    item.QueueId,
                                    item.ScanCode,
                                    item.BusinessUnitId,
                                    response.ReasonPhrase));
                            item.ErrorMessage = singleResponse.ReasonPhrase;
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
                            var matchingItems = filteredData.Where(item => item.ScanCode == responseMessage.Model.ScanCode && item.BusinessUnitId == responseMessage.Model.BusinessUnitId);
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
