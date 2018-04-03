using Mammoth.Common;
using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MoreLinq;
using Mammoth.Price.Controller.DataAccess.Models;
using System.Net;
using System.Text;

namespace Mammoth.Price.Controller.Services
{
    public class CancelAllSalesService : IService<CancelAllSalesEventModel>
    {
        private IHttpClientWrapper httpClient;
        private ILogger logger;

        public CancelAllSalesService(IHttpClientWrapper httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public void Process(List<CancelAllSalesEventModel> data)
        {
            logger.Info(String.Format("Processing {0} Cancel Sales Events.", data.Count));
            SendData(data, Uris.CancelAllSales);
        }

        private void SendData(List<CancelAllSalesEventModel> data, string uri)
        {
            IEnumerable<CancelAllSalesEventModel> filteredDate = data.Where(e => e.EventTypeId == IrmaEventTypes.CancelAllSales);
            if (!filteredDate.Any())
            {
                return;
            }

            HttpResponseMessage response = SendDataViaHttpClient(filteredDate, uri);

            // If passing records in bulk failed process one at a time.
            if (!response.IsSuccessStatusCode)
            {
                logger.Error(String.Format("Error occurred when passing processing Cancel Sales events. Processing Cancel Sales events in batches. URI : {0}. EventTypeId : {1}. Number of Cancel Sales Events : {2}. Error : {3}.",
                    uri,
                    IrmaEventTypes.CancelAllSales,
                    data.Count(),
                    response.ReasonPhrase));

                // Send records to web api in batches recursively
                if (filteredDate.Count() > 1)
                {
                    var batches = filteredDate.Batch(data.Count() / 2);
                    foreach (var batch in batches)
                    {
                        SendData(batch.ToList(), uri);
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
                            data.First().ErrorMessage = response.ReasonPhrase;
                            data.First().ErrorDetails = errorResponseContent;
                            data.First().ErrorSource = Constants.SourceSystem.MammothWebApi;
                        }
                    }
                }
            }
            else if (response.Content != null)
            {
                // set error properties for records returned within a successful response (e.g. BusinessUnit does not exist)
                var responseMessages = response.Content.ReadAsAsync<List<ErrorResponseModel<CancelAllSalesEventModel>>>().Result;
                if (responseMessages != null)
                {
                    foreach (var responseMessage in responseMessages)
                    {
                        var matchingItems = filteredDate.Where(item => item.ScanCode == responseMessage.Model.ScanCode && item.BusinessUnitId == responseMessage.Model.BusinessUnitId);
                        foreach (var matchingItem in matchingItems)
                        {
                            matchingItem.ErrorMessage = responseMessage.Error;
                        }
                    }
                }
            }
        }

        private HttpResponseMessage SendDataViaHttpClient(IEnumerable<CancelAllSalesEventModel> data, string uri)
        {
            HttpResponseMessage response;
            try
            {
                IEnumerable<CancelAllSalesModel> cancelAllSales = data.MapToCancelAllSalesModel();
                switch (uri)
                {
                    case Uris.CancelAllSales:
                        response = httpClient.PutAsJsonAsync(uri, cancelAllSales).Result;
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
                response.Content = new StringContent(exception.Message, Encoding.UTF8, "text/html");
            }

            return response;
        }
    }
}