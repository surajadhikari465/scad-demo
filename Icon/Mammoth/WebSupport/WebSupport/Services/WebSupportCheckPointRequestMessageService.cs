using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.Models;
using WebSupport.ViewModels;

namespace WebSupport.Services
{
    public class WebSupportCheckPointRequestMessageService : IEsbService<CheckPointRequestViewModel>
    {
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<CheckPointRequestBuilderModel> checkPointRequestMessageBuilder;
        private IQueryHandler<GetCheckPointMessageParameters, CheckPointMessageModel> GetCheckPointMessageQuery;

        public EsbConnectionSettings Settings { get; set; }
        public WebSupportCheckPointRequestMessageService(
          IEsbConnectionFactory esbConnectionFactory,
          EsbConnectionSettings settings,
          IMessageBuilder<CheckPointRequestBuilderModel> checkPointRequestMessageBuilder,
          IQueryHandler<GetCheckPointMessageParameters, CheckPointMessageModel> GetCheckPointMessageQuery)
        {
            this.esbConnectionFactory = esbConnectionFactory;
            this.Settings = settings;
            this.checkPointRequestMessageBuilder = checkPointRequestMessageBuilder;
            this.GetCheckPointMessageQuery = GetCheckPointMessageQuery;
        }

        public EsbServiceResponse Send(CheckPointRequestViewModel request)
        {
            var getCurrentPriceInfo = GetCheckPointMessageQuery.Search(new GetCheckPointMessageParameters
            {
                BusinessUnitId = request.Store,
                Region = StaticData.WholeFoodsRegions.ElementAt(request.RegionIndex),
                ScanCode = request.ScanCode
            });

            CheckPointRequestBuilderModel checkPointRequestBuilderModel = new CheckPointRequestBuilderModel();
            checkPointRequestBuilderModel.CheckPointRequestViewModel = request;

            if (getCurrentPriceInfo != null)
            {
                checkPointRequestBuilderModel.getCurrentPriceInfo = getCurrentPriceInfo;

                using (var producer = esbConnectionFactory.CreateProducer(Settings))
                {
                    var message = checkPointRequestMessageBuilder.BuildMessage(checkPointRequestBuilderModel);

                    Guid messageId = Guid.NewGuid();
                    var sequenceId = checkPointRequestBuilderModel.getCurrentPriceInfo.SequenceId;
                    var patchFamilyId = checkPointRequestBuilderModel.getCurrentPriceInfo.PatchFamilyId;

                    Dictionary<string, string> messageProperties = new Dictionary<string, string>
                        {
                            { EsbConstants.TransactionTypeKey, EsbConstants.CheckPointRequest },
                            { EsbConstants.TransactionIdKey, messageId.ToString() },
                            { EsbConstants.CorrelationIdKey, patchFamilyId },
                            { EsbConstants.SequenceIdKey, sequenceId.HasValue ? sequenceId.ToString() : null },
                            { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName }
                        };

                    producer.OpenConnection();
                    producer.Send(message, messageId.ToString(), messageProperties);
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
                    ErrorCode = ErrorConstants.Codes.ItemDoesNotExist,
                    ErrorDetails = ErrorConstants.Details.ItemDoesNotExist
                };
            }
        }
    }
}