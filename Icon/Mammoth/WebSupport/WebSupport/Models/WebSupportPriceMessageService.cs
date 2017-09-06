using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.ViewModels;

namespace WebSupport.Models
{
    public class WebSupportPriceMessageService : IEsbService<PriceResetRequestViewModel>
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<PriceResetMessageBuilderModel> priceResetMessageBuilder;
        private IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>> getPriceResetPricesQuery;
        private ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler;

        public EsbConnectionSettings Settings { get; set; }

        public WebSupportPriceMessageService(
            IEsbConnectionFactory esbConnectionFactory,
            EsbConnectionSettings settings,
            IMessageBuilder<PriceResetMessageBuilderModel> priceResetMessageBuilder,
            IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>> getPriceResetPricesQuery,
            ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler)
        {
            this.esbConnectionFactory = esbConnectionFactory;
            this.Settings = settings;
            this.priceResetMessageBuilder = priceResetMessageBuilder;
            this.getPriceResetPricesQuery = getPriceResetPricesQuery;
            this.saveSentMessageCommandHandler = saveSentMessageCommandHandler;
        }

        public EsbServiceResponse Send(PriceResetRequestViewModel request)
        {
            var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(request.RegionIndex);
            var chosenSystems = StaticData.DownstreamSystems
                .Where((s, i) => request.DownstreamSystems.Contains(i))
                .ToList();
            var chosenStores = request.Stores.ToList();
            var chosenScanCodes = request.Items
                .Split()
                .Where(s => !String.IsNullOrWhiteSpace(s))
                .ToList();

            var priceResetPrices = getPriceResetPricesQuery.Search(new GetPriceResetPricesParameters
            {
                BusinessUnitIds = chosenStores,
                Region = chosenRegion,
                ScanCodes = chosenScanCodes
            });

            if (priceResetPrices.Any())
            {
                using (var producer = esbConnectionFactory.CreateProducer(Settings))
                {
                    producer.OpenConnection();
                    foreach (var priceGroup in priceResetPrices.GroupBy(p => new { p.BusinessUnitId, p.ItemId }))
                    {
                        var message = priceResetMessageBuilder.BuildMessage(new PriceResetMessageBuilderModel
                        {
                            PriceResetPrices = priceGroup.ToList()
                        });

                        Guid messageId = Guid.NewGuid();
                        var sequenceId = priceGroup.First().SequenceId;
                        var patchFamilyId = priceGroup.First().PatchFamilyId;
                        Dictionary<string, string> messageProperties = new Dictionary<string, string>
                        {
                            { EsbConstants.TransactionTypeKey, EsbConstants.PriceResetValueName },
                            { EsbConstants.TransactionIdKey, messageId.ToString() },
                            { EsbConstants.CorrelationIdKey, patchFamilyId },
                            { EsbConstants.SequenceIdKey, sequenceId.HasValue ? sequenceId.ToString() : null },
                            { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName },
                            { EsbConstants.NonReceivingSystemsKey, GetNonReceivingSystems(chosenSystems) }
                        };
                        producer.Send(message, messageId.ToString(), messageProperties);
                        saveSentMessageCommandHandler.Execute(new SaveSentMessageCommand
                        {
                            Message = message,
                            MessageId = messageId,
                            MessageProperties = messageProperties
                        });
                    }

                    var response = new EsbServiceResponse
                    {
                        Status = EsbServiceResponseStatus.Sent,
                    };

                    return response;
                }
            }
            else
            {
                return new EsbServiceResponse
                {
                    Status = EsbServiceResponseStatus.Failed,
                    ErrorCode = ErrorConstants.Codes.NoPricesExist,
                    ErrorDetails = ErrorConstants.Details.NoPricesExist
                };
            }
        }

        private string GetNonReceivingSystems(List<string> chosenSystems)
        {
            return string.Join(",", StaticData.DownstreamSystems.Except(chosenSystems));
        }
    }
}