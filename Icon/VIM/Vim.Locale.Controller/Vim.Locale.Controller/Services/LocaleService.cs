using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MoreLinq;
using Vim.Common.ControllerApplication.Http;
using Vim.Common.ControllerApplication.Services;
using Vim.Common.DataAccess;
using Vim.Locale.Controller.DataAccess.Models;
using Vim.Logging;
using Newtonsoft.Json;
namespace Vim.Locale.Controller.Services
{
    public class LocaleService : IService<LocaleEventModel>
    {
        private IHttpClientWrapper httpClient;
        private ILogger logger;

        public LocaleService(IHttpClientWrapper httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public void Process(List<LocaleEventModel> data)
        {
            logger.Info(String.Format("Processing {0} VIM Locale Events.", data.Count));

            MarkInvalid(data.Where(d=> d.StoreModel == null));
            AddOrUpdate(Uris.StoreAddOrUpdateUri, data.Where(d => d.StoreModel !=null));
        }

        private void MarkInvalid(IEnumerable<LocaleEventModel> invalidData)
        {
            foreach (var localeEventModel in invalidData)
            {
                localeEventModel.ErrorMessage = "Missing Store Model information";
            }
        }
        private void AddOrUpdate(string uri, IEnumerable<LocaleEventModel> localeEvents)
        {
            if (localeEvents.Any())
            {
                List<VimStoreModel> stores = new List<VimStoreModel>();

                foreach (LocaleEventModel storeevent in localeEvents)
                {
                    stores.Add(storeevent.GetValidatedStoreModel());
                }

                var response = httpClient.PostAsJsonAsync(uri, stores).Result;
                AddOrUpdateOnFailure(response, uri, localeEvents);
            }
        }

        private void AddOrUpdateOnFailure(HttpResponseMessage response, string uri, IEnumerable<LocaleEventModel> localeEvents)
        {
            if (!response.IsSuccessStatusCode)
            {
                logger.Error(String.Format("Error occurred when adding or updating stores in VIM. Processing each store one by one. URI : {0}. Number of stores : {1}. Error : {2}.",
                    uri,
                    localeEvents.Count(),
                    response.ReasonPhrase));
                foreach (var localeEvent in localeEvents)
                {
                    var singleResponse = httpClient.PostAsJsonAsync(uri, new[] { localeEvent.GetValidatedStoreModel() }).Result;
                    if (!singleResponse.IsSuccessStatusCode)
                    {
                        logger.Error(String.Format("Error occurred when adding or updating single Hierarchy Class. URI : {0}. QueueId : {1}. LocaleId {2}. Error : {3}.",
                            uri,
                            localeEvent.QueueId,
                            localeEvent.EventReferenceId,
                            response.ReasonPhrase));
                        localeEvent.ErrorMessage = singleResponse.ReasonPhrase;
                    }
                }
            }
        }
    }
}

