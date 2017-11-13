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
using WebSupport.Models;
using WebSupport.ViewModels;
namespace WebSupport.Services
{
    public class WebSupportCheckPointRequestMessageService : IEsbService<CheckPointRequestBuilderModel>
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<CheckPointRequestBuilderModel> checkPointRequestMessageBuilder;
        private ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler;
        private IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>> getPriceResetPricesQuery;

        public EsbConnectionSettings Settings { get; set; }
        public WebSupportCheckPointRequestMessageService(
          IEsbConnectionFactory esbConnectionFactory,
          EsbConnectionSettings settings,
          IMessageBuilder<CheckPointRequestBuilderModel> checkPointRequestMessageBuilder,
          ICommandHandler<SaveSentMessageCommand> saveSentMessageCommandHandler,
          IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>> getPriceResetPricesQuery)
        {
            this.esbConnectionFactory = esbConnectionFactory;
            this.Settings = settings;
            this.checkPointRequestMessageBuilder = checkPointRequestMessageBuilder;
            this.saveSentMessageCommandHandler = saveSentMessageCommandHandler;
            this.getPriceResetPricesQuery = getPriceResetPricesQuery;
        }

        public EsbServiceResponse Send(CheckPointRequestBuilderModel request)
        {
            var getCurrentPriceInfo = getPriceResetPricesQuery.Search(new GetPriceResetPricesParameters
            {
                BusinessUnitIds = new List<string> { request.Store },
                Region = StaticData.WholeFoodsRegions.ElementAt(request.RegionIndex),
                ScanCodes = new List<string> { request.Item }
            });

            if (getCurrentPriceInfo.Any())
            {
                request.getCurrentPriceInfo = getCurrentPriceInfo.FirstOrDefault();
                using (var producer = esbConnectionFactory.CreateProducer(Settings))
                {

                    var message = checkPointRequestMessageBuilder.BuildMessage(request);

                    Guid messageId = Guid.NewGuid();
                    var sequenceId = request.getCurrentPriceInfo.SequenceId;
                    var patchFamilyId = request.getCurrentPriceInfo.PatchFamilyId;
                    Dictionary<string, string> messageProperties = new Dictionary<string, string>
                        {
                            { EsbConstants.TransactionTypeKey, EsbConstants.CheckPointRequestToGPM },
                            { EsbConstants.TransactionIdKey, messageId.ToString() },
                            { EsbConstants.CorrelationIdKey, patchFamilyId },
                            { EsbConstants.SequenceIdKey, sequenceId.HasValue ? sequenceId.ToString() : null },
                            { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName }
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
    }
}