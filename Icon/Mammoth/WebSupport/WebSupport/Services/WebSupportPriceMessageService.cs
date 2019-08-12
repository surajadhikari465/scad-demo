using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Models;
using WebSupport.ViewModels;

namespace WebSupport.Services
{
    public class WebSupportPriceMessageService : IEsbService<PriceResetRequestViewModel>
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<PriceResetMessageBuilderModel> priceResetMessageBuilder;
        private IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>> getPriceResetPricesQuery;
        private ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler;
        private IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>> searchScanCodes;

        public EsbConnectionSettings Settings { get; set; }

        public WebSupportPriceMessageService(
            IEsbConnectionFactory esbConnectionFactory,
            EsbConnectionSettings settings,
            IMessageBuilder<PriceResetMessageBuilderModel> priceResetMessageBuilder,
            IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>> getPriceResetPricesQuery,
            ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler,
            IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>> searchScanCodes)
        {
            this.esbConnectionFactory = esbConnectionFactory;
            this.Settings = settings;
            this.priceResetMessageBuilder = priceResetMessageBuilder;
            this.getPriceResetPricesQuery = getPriceResetPricesQuery;
            this.saveSentMessageCommandHandler = saveSentMessageCommandHandler;
            this.searchScanCodes = searchScanCodes;
        }

        public EsbServiceResponse Send(PriceResetRequestViewModel request)
        {
            EsbServiceResponse response = new EsbServiceResponse();
            try
            {
                var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(request.RegionIndex);
                var chosenSystems = StaticData.DownstreamSystems
                    .Where((s, i) => request.DownstreamSystems.Contains(i))
                    .ToList();
                var chosenStores = request.Stores.ToList();

                var codes = request.Items.Replace(" ", String.Empty)
                                   .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                   .Distinct().ToList();

                var chosenScanCodes = !request.IsItemId
                    ? codes
                    : searchScanCodes.Search(new GetMammothItemIdsToScanCodesParameters { ItemIds = codes });

                var priceResetPrices = getPriceResetPricesQuery.Search(new GetPriceResetPricesParameters
                {
                    BusinessUnitIds = chosenStores,
                    Region = chosenRegion,
                    ScanCodes = chosenScanCodes
                });

                if (priceResetPrices.Any())
                {
                    List<string> errors = new List<string>();
                    using (var producer = esbConnectionFactory.CreateProducer(Settings))
                    {
                        producer.OpenConnection();
                        foreach (var priceGroup in priceResetPrices.GroupBy(p => new { p.BusinessUnitId, p.ItemId, p.ScanCode }))
                        {
                            var sequenceId = priceGroup.First().SequenceId;
                            var patchFamilyId = priceGroup.First().PatchFamilyId;

                            if (!string.IsNullOrWhiteSpace(sequenceId) && !string.IsNullOrWhiteSpace(patchFamilyId))
                            {
                                SendPriceResetMessage(producer, sequenceId, patchFamilyId, priceGroup, chosenSystems);
                            }
                            else
                            {
                                errors.Add($"Mammoth cannot send a Price Reset for {{ScanCode:{priceGroup.Key.ScanCode},BusinessUnitID:{priceGroup.Key.BusinessUnitId}}} since GPM has not sent a price for this item yet.");
                            }
                        }

                        if (errors.Any())
                        {
                            response.Status = EsbServiceResponseStatus.Failed;
                            response.ErrorCode = ErrorConstants.Codes.SequenceIdOrPatchFamilyIdNotExist;
                            response.ErrorDetails = string.Join("|", errors);
                        }
                        else
                        {
                            response.Status = EsbServiceResponseStatus.Sent;
                        }

                        return response;
                    }
                }
                else
                {
                    response.Status = EsbServiceResponseStatus.Failed;
                    response.ErrorCode = ErrorConstants.Codes.NoPricesExist;
                    response.ErrorDetails = ErrorConstants.Details.NoPricesExist;
                }
            }
            catch (Exception ex)
            {
                response.Status = EsbServiceResponseStatus.Failed;
                response.ErrorCode = ErrorConstants.Codes.UnexpectedError;
                response.ErrorDetails = ex.Message;
            }
            return response;
        }

        private void SendPriceResetMessage(
            IEsbProducer producer,
            string sequenceId, 
            string patchFamilyId, 
            IEnumerable<PriceResetPrice> prices, 
            List<string> chosenSystems)
        {
            Guid messageId = Guid.NewGuid();
            var message = priceResetMessageBuilder.BuildMessage(new PriceResetMessageBuilderModel
            {
                PriceResetPrices = prices.ToList()
            });

            Dictionary<string, string> messageProperties = new Dictionary<string, string>
                            {
                                { EsbConstants.TransactionTypeKey, EsbConstants.PriceTransactionTypeValue },
                                { EsbConstants.TransactionIdKey, messageId.ToString() },
                                { EsbConstants.CorrelationIdKey, patchFamilyId },
                                { EsbConstants.SequenceIdKey, sequenceId },
                                { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName },
                                { EsbConstants.NonReceivingSystemsKey, GetNonReceivingSystems(chosenSystems) },
                                { EsbConstants.PriceResetKey, EsbConstants.PriceResetTrueValue }
                            };
            producer.Send(message, messageId.ToString(), messageProperties);
            saveSentMessageCommandHandler.Execute(new SaveSentMessageCommand
            {
                Message = message,
                MessageId = messageId,
                MessageProperties = messageProperties
            });
        }

        private string GetNonReceivingSystems(List<string> chosenSystems)
        {
            return string.Join(",", StaticData.DownstreamSystems.Except(chosenSystems));
        }
    }
}