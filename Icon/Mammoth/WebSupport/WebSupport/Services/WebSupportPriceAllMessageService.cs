using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupport.DataAccess.Commands;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Managers;
using WebSupport.Models;
using WebSupport.ViewModels;
using Newtonsoft.Json;
using WebSupport.Clients;
using WebSupport.Helpers;

namespace WebSupport.Services
{
    public class WebSupportPriceAllMessageService : IEsbService<PricesAllViewModel>
    {
        const string PriceReset = "PriceReset";
        private string[] targets;
        private string nonReceiving;
        private ILogger logger;
        private IClientIdManager clientIdManager;
        private IMessageBuilder<PriceResetMessageBuilderModel> messageBuilder;
        private IQueryHandler<GetPricesAllParameters, List<PriceResetPrice>> getPricesQuery;
        private ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler;
        private IDvsNearRealTimePriceClient dvsNearRealTimePriceClient;

        public EsbConnectionSettings Settings { get; set; }

        public WebSupportPriceAllMessageService(
            ILogger logger,
            IMessageBuilder<PriceResetMessageBuilderModel> messageBuilder,
            IQueryHandler<GetPricesAllParameters, List<PriceResetPrice>> getPricesQuery,
            ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler,
            IClientIdManager clientIdManager,
            IDvsNearRealTimePriceClient dvsNearRealTimePriceClient)
        {
            this.logger = logger;
            this.messageBuilder = messageBuilder;
            this.getPricesQuery = getPricesQuery;
            this.saveSentMessageCommandHandler = saveSentMessageCommandHandler;
            this.clientIdManager = clientIdManager;
            this.dvsNearRealTimePriceClient = dvsNearRealTimePriceClient;
        }

        public EsbServiceResponse Send(PricesAllViewModel viewModel)
        {
            var response = new EsbServiceResponse();

            try
            {
                var selectedRegion = StaticData.WholeFoodsRegions.Where(x => String.Compare(x, "TS", StringComparison.InvariantCultureIgnoreCase) != 0).ToArray()[viewModel.RegionIndex];
                this.targets = StaticData.DownstreamSystems
                    .Where((s, i) => viewModel.DownstreamSystems.Contains(i))
                    .ToArray();

                this.nonReceiving = String.Join(",", StaticData.DownstreamSystems.Except(this.targets));

                var prices = getPricesQuery.Search(new GetPricesAllParameters()
                    {
                        Region = selectedRegion,
                        BusinessUnitId = viewModel.SelectedStores.Select(x => int.Parse(x)).ToList()
                    });

                if(prices.Any())
                {
                    List<string> errors = new List<string>();

                   foreach(var grp in prices.GroupBy(p => new { p.BusinessUnitId, p.ItemId }))
                   {
                       var firstItem = grp.First();
                       var sequenceId = firstItem.SequenceId;
                       var patchFamilyId = firstItem.PatchFamilyId;

                       if (!string.IsNullOrWhiteSpace(sequenceId) && !string.IsNullOrWhiteSpace(patchFamilyId))
                       {
                           SendMessage(sequenceId, patchFamilyId, grp.ToList());
                       }
                   }

                   if (errors.Any())
                   {
                       response.Status = EsbServiceResponseStatus.Failed;
                       response.ErrorCode = ErrorConstants.Codes.SequenceIdOrPatchFamilyIdNotExist;
                       response.ErrorDetails = String.Join("|", errors);
                   }
                   else
                   {
                       response.Status = EsbServiceResponseStatus.Sent;
                   }
                   return response;
                }
                else
                {
                    response.Status = EsbServiceResponseStatus.Failed;
                    response.ErrorCode = ErrorConstants.Codes.NoPricesExist;
                    response.ErrorDetails = ErrorConstants.Details.NoPricesFound;
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

        private void SendMessage(string sequenceId, string patchFamilyId, List<PriceResetPrice> prices)
        {
            var messageId = Guid.NewGuid().ToString();
            var message = messageBuilder.BuildMessage(new PriceResetMessageBuilderModel { PriceResetPrices = prices });

            var messageProperties = new Dictionary<string, string>
                {
                    { EsbConstants.TransactionTypeKey, EsbConstants.PriceTransactionTypeValue },
                    { EsbConstants.TransactionIdKey, messageId.ToString() },
                    { EsbConstants.CorrelationIdKey, patchFamilyId },
                    { EsbConstants.SequenceIdKey, sequenceId },
                    { EsbConstants.SourceKey, Constants.Source.Infor },
                    { EsbConstants.NonReceivingSystemsKey, this.nonReceiving },
                    { EsbConstants.PriceResetKey,  EsbConstants.PriceResetFalseValue }
                };

            dvsNearRealTimePriceClient.Send(message, messageId, messageProperties);
            
            //There's lots of message (100K+). Should we log them at all?
            //The code below is working. Uncoment it when needed.
            /*saveSentMessageCommandHandler.Execute(new SaveSentMessageCommand
            {
                Message = message,
                MessageId = messageId,
                MessageProperties = messageProperties
            });*/
        }
    }
}