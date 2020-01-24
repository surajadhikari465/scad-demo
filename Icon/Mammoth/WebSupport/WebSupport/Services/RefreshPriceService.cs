using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private ILogger logger;
        private IPriceRefreshEsbProducerFactory priceRefreshEsbProducerFactory;
        private IPriceRefreshMessageBuilderFactory priceRefreshMessageBuilderFactory;
        private IQueryHandler<GetGpmPricesParameters, List<GpmPrice>> getGpmPricesQuery;
        private IQueryHandler<DoesScanCodeExistParameters, bool> doesScanCodeExistQuery;
        private IQueryHandler<DoesStoreExistParameters, bool> doesStoreExistQuery;
        private IClientIdManager clientIdManager;

        public RefreshPriceService(
            ILogger logger,
            IPriceRefreshEsbProducerFactory priceRefreshEsbProducerFactory,
            IPriceRefreshMessageBuilderFactory priceRefreshMessageBuilderFactory,
            IQueryHandler<GetGpmPricesParameters, List<GpmPrice>> getGpmPricesQuery,
            IQueryHandler<DoesScanCodeExistParameters, bool> doesScanCodeExistQuery,
            IQueryHandler<DoesStoreExistParameters, bool> doesStoreExistQuery,
            IClientIdManager clientIdManager)
        {
            this.logger = logger;
            this.priceRefreshEsbProducerFactory = priceRefreshEsbProducerFactory;
            this.priceRefreshMessageBuilderFactory = priceRefreshMessageBuilderFactory;
            this.getGpmPricesQuery = getGpmPricesQuery;
            this.doesScanCodeExistQuery = doesScanCodeExistQuery;
            this.doesStoreExistQuery = doesStoreExistQuery;
            this.clientIdManager = clientIdManager;
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
            foreach (var businessUnitId in businessUnitIds)
            {
                if (StoreExistsInMammoth(businessUnitId))
                {
                    foreach (var scanCode in scanCodes)
                    {
                        if (ScanCodeExistsInMammoth(scanCode))
                        {
                            var prices = GetPrices(region, businessUnitId, scanCode);
                            if (prices.Count > 0)
                            {
                                try
                                {
                                    SendPriceRefreshMessage(region, systems, prices);
                                }
                                catch (Exception ex)
                                {
                                    response.Errors.Add($"Unable to refresh price for Business Unit:{businessUnitId} and Scan Code:{scanCode} because an unexpected error occurred while connecting to the ESB. Check logs and connection settings for more information.");
                                    logger.Error($"Unable to refresh price for Business Unit:{businessUnitId} and Scan Code:{scanCode} because an unexpected error occurred while connecting to the ESB. Check logs and connection settings for more information. Error Details:{ex.ToString()}");
                                }
                            }
                            else
                            {
                                response.Errors.Add($"Unable to refresh price for Business Unit:{businessUnitId} and Scan Code:{scanCode} because it does not exist in Mammoth.");
                            }
                        }
                        else
                        {
                            response.Errors.Add($"Unable to refresh Item:{scanCode} because it does not exist in Mammoth.");
                        }
                    }
                }
                else
                {
                    response.Errors.Add($"Unable to refresh Business Unit:{businessUnitId} because it does not exist in Mammoth.");
                }
            }

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

        private List<GpmPrice> GetPrices(string region, string businessUnitId, string scanCode)
        {
            return getGpmPricesQuery.Search(
                new GetGpmPricesParameters
                {
                    Region = region,
                    BusinessUnitId = businessUnitId,
                    ScanCode = scanCode
                });
        }

        private void SendPriceRefreshMessage(string region, List<string> systems, List<GpmPrice> prices)
        {
            string patchFamilyId = prices[0].PatchFamilyId;
            string sequenceId = prices[0].SequenceId;

            foreach (var system in systems)
            {
                var messageBuilder = priceRefreshMessageBuilderFactory.CreateMessageBuilder(system);
                var message = messageBuilder.BuildMessage(prices);
                Dictionary<string, string> messageProperties = new Dictionary<string, string>
                            {
                                { EsbConstants.TransactionTypeKey, EsbConstants.PriceTransactionTypeValue },
                                { EsbConstants.TransactionIdKey, Guid.NewGuid().ToString() },
                                { EsbConstants.CorrelationIdKey, patchFamilyId },
                                { EsbConstants.SequenceIdKey, sequenceId },
                                { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName },
                                { EsbConstants.NonReceivingSystemsKey, GetNonReceivingSystems(system) },
                                { EsbConstants.PriceResetKey, EsbConstants.PriceResetFalseValue }
                            };
                using (IEsbProducer esbProducer = priceRefreshEsbProducerFactory.CreateEsbProducer(system, region))
                {
                    esbProducer.OpenConnection();
                    esbProducer.ClientId = clientIdManager.GetClientId();
                    esbProducer.Send(
                        message,
                        messageProperties);
                }
                logger.Info(JsonConvert.SerializeObject(
                    new
                    {
                        Action = "RefreshPrice",
                        Region = region,
                        System = system,
                        Prices = prices,
                        MessageProperties = messageProperties,
                        Message = message
                    }));
            }
        }

        private string GetNonReceivingSystems(string system)
        {
            return string.Join(",", StaticData.DownstreamSystems.Except(new List<string> { system }));
        }
    }
}