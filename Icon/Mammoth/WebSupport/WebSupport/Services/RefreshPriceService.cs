using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupport.Clients;
using WebSupport.DataAccess;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.EsbProducerFactory;
using WebSupport.Managers;
using WebSupport.MessageBuilders;
using WebSupport.Models;

namespace WebSupport.Services
{
    public class RefreshPriceService : IRefreshPriceService
    {
        const string RefreshPrice = "RefreshPrice";
        private ILogger logger;
        private IPriceRefreshEsbProducerFactory priceRefreshEsbProducerFactory;
        private IPriceRefreshMessageBuilderFactory priceRefreshMessageBuilderFactory;
        private IQueryHandler<GetGpmPricesParameters, List<GpmPrice>> getGpmPricesQuery;
        private IQueryHandler<DoesScanCodeExistParameters, bool> doesScanCodeExistQuery;
        private IQueryHandler<DoesStoreExistParameters, bool> doesStoreExistQuery;
        private IClientIdManager clientIdManager;
        private string selectedRegion;
        private Dictionary<string, string> nonReceivingSystems;
        private Dictionary<string, Esb.Core.MessageBuilders.IMessageBuilder<List<GpmPrice>>> messageBuilders;
        private IMammothGpmBridgeClient mammothGpmClient;

        public int InvalidScanCodeCount { get; private set; }

        public RefreshPriceService(
            ILogger logger,
            IPriceRefreshEsbProducerFactory priceRefreshEsbProducerFactory,
            IPriceRefreshMessageBuilderFactory priceRefreshMessageBuilderFactory,
            IQueryHandler<GetGpmPricesParameters, List<GpmPrice>> getGpmPricesQuery,
            IQueryHandler<DoesScanCodeExistParameters, bool> doesScanCodeExistQuery,
            IQueryHandler<DoesStoreExistParameters, bool> doesStoreExistQuery,
            IClientIdManager clientIdManager,
            IMammothGpmBridgeClient mammothGpmClient)
        {
            this.logger = logger;
            this.priceRefreshEsbProducerFactory = priceRefreshEsbProducerFactory;
            this.priceRefreshMessageBuilderFactory = priceRefreshMessageBuilderFactory;
            this.getGpmPricesQuery = getGpmPricesQuery;
            this.doesScanCodeExistQuery = doesScanCodeExistQuery;
            this.doesStoreExistQuery = doesStoreExistQuery;
            this.clientIdManager = clientIdManager;
            this.mammothGpmClient = mammothGpmClient;
        }

        public RefreshPriceResponse RefreshPrices(
            string region,
            List<string> systems,
            List<string> businessUnitIds,
            List<string> scanCodes)
        {
            RefreshPriceResponse response = new RefreshPriceResponse { Errors = new List<string>() };
            ValidateParametersAndPopulateParameterErrors(region, systems, businessUnitIds, scanCodes, response);
            if (response.Errors.Count > 0)
            {
                return response;
            }
            
            int batchId = 0;
            int totalCnt = 0;
            const int batchSize = 1000;
            var isAllItems = scanCodes.Count == 1 && scanCodes[0] == "*";
            int? itemId = isAllItems ? (int?)0 : null;

            this.selectedRegion = region;
            this.InvalidScanCodeCount = 0;
            this.nonReceivingSystems = systems.ToDictionary(x => x, x => GetNonReceivingSystems(x), StringComparer.InvariantCultureIgnoreCase);
            this.messageBuilders = systems.ToDictionary(x => x, x => priceRefreshMessageBuilderFactory.CreateMessageBuilder(x), StringComparer.InvariantCultureIgnoreCase);

            logger.Info(JsonConvert.SerializeObject(
                 new
                 {
                     Action = $"Begin {RefreshPrice}",
                     Region = this.selectedRegion,
                     System = String.Join(",", this.messageBuilders.Select(x => x.Key))
                 }));

            foreach (var businessUnitId in businessUnitIds)
            {
                if (StoreExistsInMammoth(businessUnitId))
                {
                    var parameter = new GetGpmPricesParameters
                    {
                        Region = region,
                        BusinessUnitId = businessUnitId,
                        ItemId = itemId,
                        ScanCodes = isAllItems ? new List<string>() : scanCodes.Take(batchSize).ToList()
                    };

                    while(true)
                    {
                        var prices = getGpmPricesQuery.Search(parameter);
                        totalCnt += prices.Count();

                        if(prices.Count == 0)
                        {
                            break;
                        }
                        else
                        {
                            try
                            { 
                                SendPriceRefreshMessage(prices);
                                logger.Info(JsonConvert.SerializeObject(
                                    new
                                    {
                                        Action = $"Processing {RefreshPrice}",
                                        Region = this.selectedRegion,
                                        System = String.Join(",", this.messageBuilders.Select(x => x.Key)),
                                        TotalRecords = totalCnt
                                    }));
                            }
                            catch (Exception ex)
                            {
                                response.Errors.Add($"Unable to refresh price for Business Unit: {businessUnitId} because an unexpected error occurred while connecting to the ESB. Check logs and connection settings for more information.");
                                logger.Error($"Unable to refresh price for Business Unit: {businessUnitId} because an unexpected error occurred while connecting to the ESB. Error: {ex.ToString()}");
                            }
                        }

                        batchId++;
                        if(isAllItems)
                        {
                            parameter.ItemId = prices.Max(x => x.ItemId);
                        }
                        else
                        { 
                            this.InvalidScanCodeCount += parameter.ScanCodes.Except(prices.Select(x => x.ScanCode)).Count();
                            parameter.ScanCodes = scanCodes.Skip(batchId * batchSize).Take(batchSize).ToList();
                            if(parameter.ScanCodes.Count == 0) break;
                        }
                    }
                }
                else
                {
                    response.Errors.Add($"Unable to refresh Business Unit:{businessUnitId} because it does not exist in Mammoth.");
                }
            }

            logger.Info(JsonConvert.SerializeObject(
                 new
                 {
                     Action = $"End {RefreshPrice}",
                     Region = this.selectedRegion,
                     System = String.Join(",", this.messageBuilders.Select(x => x.Key)),
                     TotalRecords = totalCnt
                 }));
            return response;
        }

        private static void ValidateParametersAndPopulateParameterErrors(
            string region,
            List<string> systems,
            List<string> stores,
            List<string> scanCodes,
            RefreshPriceResponse response)
        {
            if (string.IsNullOrWhiteSpace(region))
            {
                response.Errors.Add("Unable to perform refresh because Region not supplied.");
            }
            else if (systems == null || systems.Count < 1)
            {
                response.Errors.Add("Unable to perform refresh because no downstream systems were supplied.");
            }
            else if (systems.Any(s => !DataConstants.JustInTimeDownstreamSystems.Contains(s)))
            {
                response.Errors.Add($"Unable to perform refresh because selected downstream systems ({string.Join(",", systems)}) contains a non-just in time system ({string.Join(",", DataConstants.JustInTimeDownstreamSystems)}).");
            }
            else if (stores == null || stores.Count < 1)
            {
                response.Errors.Add("Unable to perform refresh because no Stores were supplied");
            }
            else if (scanCodes == null || scanCodes.Count < 1)
            {
                response.Errors.Add("Unable to perform refresh because no Scan Codes were supplied");
            }
        }

        private bool StoreExistsInMammoth(string businessUnitId)
        {
            return doesStoreExistQuery.Search(new DoesStoreExistParameters { BusinessUnitId = businessUnitId });
        }

        private bool ScanCodeExistsInMammoth(string scanCode)
        {
            return doesScanCodeExistQuery.Search(new DoesScanCodeExistParameters { ScanCode = scanCode });
        }

        private void SendPriceRefreshMessage(List<GpmPrice> prices)
        {
            foreach(var builder in this.messageBuilders)
            {
                var properties = new Dictionary<string, string>
                {
                    { EsbConstants.TransactionTypeKey, EsbConstants.PriceTransactionTypeValue },
                    { EsbConstants.TransactionIdKey, null },
                    { EsbConstants.CorrelationIdKey, null },
                    { EsbConstants.SequenceIdKey, null },
                    { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName },
                    { EsbConstants.NonReceivingSystemsKey, this.nonReceivingSystems[builder.Key] },
                    { EsbConstants.PriceResetKey, EsbConstants.PriceResetFalseValue }
                };  

                using (IEsbProducer esbProducer = priceRefreshEsbProducerFactory.CreateEsbProducer(builder.Key, this.selectedRegion))
                {
                    esbProducer.OpenConnection(clientIdManager.GetClientId());

                    foreach(var grp in prices.GroupBy(x => x.ItemId))
                    {
                        var priceList = grp.ToList();
                        properties[EsbConstants.SequenceIdKey] = priceList[0].SequenceId;
                        properties[EsbConstants.CorrelationIdKey] = priceList[0].PatchFamilyId;
                        properties[EsbConstants.TransactionIdKey] = Guid.NewGuid().ToString();

                        var message = builder.Value.BuildMessage(priceList);

                        this.mammothGpmClient.SendToJustInTimeConsumers(message, properties, this.selectedRegion, builder.Key);
                        esbProducer.Send(message, properties);

                        logger.Debug(JsonConvert.SerializeObject(
                            new
                            {
                                Action = RefreshPrice,
                                Region = this.selectedRegion,
                                System = builder.Key,
                                Message = message
                            }));
                    }
                }
            }
        }

        private string GetNonReceivingSystems(string system)
        {
            return string.Join(",", StaticData.DownstreamSystems.Except(new List<string> { system }));
        }
    }
}