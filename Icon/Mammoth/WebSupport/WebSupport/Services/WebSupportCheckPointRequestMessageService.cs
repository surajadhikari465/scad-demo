﻿using Esb.Core.EsbServices;
using Esb.Core.MessageBuilders;
using Icon.Common.DataAccess;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Newtonsoft.Json;
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
    public class WebSupportCheckPointRequestMessageService : IEsbMultipleMessageService<CheckPointRequestViewModel>
    {
        private ILogger logger;
        private IEsbConnectionFactory esbConnectionFactory;
        private IMessageBuilder<CheckPointRequestBuilderModel> checkPointRequestMessageBuilder;
        private IQueryHandler<GetCheckPointMessageParameters, IEnumerable<CheckPointMessageModel>> GetCheckPointMessageQuery;
        private ICommandHandler<ArchiveCheckpointMessageCommandParameters> archiveCheckpointMessageCommandHandler;

        public WebSupportCheckPointRequestMessageService(
            ILogger logger,
            IEsbConnectionFactory esbConnectionFactory,
            EsbConnectionSettings settings,
            IMessageBuilder<CheckPointRequestBuilderModel> checkPointRequestMessageBuilder,
            IQueryHandler<GetCheckPointMessageParameters, IEnumerable<CheckPointMessageModel>> GetCheckPointMessageQuery,
            ICommandHandler<ArchiveCheckpointMessageCommandParameters> archiveCheckpointMessageCommandHandler)
        {
            this.logger = logger;
            this.esbConnectionFactory = esbConnectionFactory;
            this.Settings = settings;
            this.checkPointRequestMessageBuilder = checkPointRequestMessageBuilder;
            this.GetCheckPointMessageQuery = GetCheckPointMessageQuery;
            this.archiveCheckpointMessageCommandHandler = archiveCheckpointMessageCommandHandler;
        }

        public EsbConnectionSettings Settings { get; set; }

        public List<EsbServiceResponse> Send(CheckPointRequestViewModel viewModel)
        {
            var esbResponses = new List<EsbServiceResponse>(viewModel.Stores.Length * viewModel.ScanCodesList.Count);
            int businessUnit = 0;
            var esbResponse = new EsbServiceResponse();

            var checkPointMessages = GetCheckPointMessageQuery.Search(
                new GetCheckPointMessageParameters
                {
                    Region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex),
                    BusinessUnitIds = viewModel.StoresAsIntList,
                    ScanCodes = viewModel.ScanCodesList
                });

            if (checkPointMessages != null)
            {
                foreach (var store in viewModel.Stores)
                {
                    if (int.TryParse(store, out businessUnit))
                    {
                        foreach (var scanCode in viewModel.ScanCodesList)
                        {
                            var checkPointMessage = checkPointMessages.FirstOrDefault(m =>
                                m.BusinessUnitID == businessUnit &&
                                string.Equals(m.ScanCode, scanCode, StringComparison.CurrentCultureIgnoreCase));

                            LogCheckpointRequest("Log CheckpointRequest Start", store, scanCode);

                            if (checkPointMessage != default(CheckPointMessageModel))
                            {
                                var checkPointRequestBuilderModel = new CheckPointRequestBuilderModel()
                                {
                                    CheckpointMessage = checkPointMessage
                                };
                                var message = checkPointRequestMessageBuilder.BuildMessage(checkPointRequestBuilderModel);

                                Guid messageId = Guid.NewGuid();
                                var sequenceId = checkPointRequestBuilderModel.CheckpointMessage.SequenceId;
                                var patchFamilyId = checkPointRequestBuilderModel.CheckpointMessage.PatchFamilyId;

                                Dictionary<string, string> messageProperties = new Dictionary<string, string>
                                    {
                                        { EsbConstants.TransactionTypeKey, EsbConstants.CheckPointRequest },
                                        { EsbConstants.TransactionIdKey, messageId.ToString() },
                                        { EsbConstants.CorrelationIdKey, patchFamilyId },
                                        { EsbConstants.SequenceIdKey, sequenceId.HasValue ? sequenceId.ToString() : null },
                                        { EsbConstants.SourceKey, EsbConstants.MammothSourceValueName }
                                    };

                                using (var esbProducer = esbConnectionFactory.CreateProducer(Settings))
                                {
                                    esbProducer.OpenConnection();
                                    esbProducer.Send(message, messageId.ToString(), messageProperties);
                                }

                                esbResponse = new EsbServiceResponse
                                {
                                    Status = EsbServiceResponseStatus.Sent,
                                    ErrorDetails = $"Store: '{store}' ScanCode: '{scanCode}' sent for checkpoint request."
                                };

                                archiveCheckpointMessageCommandHandler.Execute(
                                    new ArchiveCheckpointMessageCommandParameters
                                    {
                                        MessageId = messageId,
                                        MessageStatusId = MessageStatusTypes.Sent,
                                        Message = message,
                                        MessageHeadersJson = JsonConvert.SerializeObject(messageProperties),
                                    });
                            }
                            else
                            {
                                esbResponse = new EsbServiceResponse
                                {
                                    Status = EsbServiceResponseStatus.Failed,
                                    ErrorCode = ErrorConstants.Codes.ItemDoesNotExist,
                                    ErrorDetails = $"Store: '{store}' ScanCode: '{scanCode}' not found for checkpoint request."
                                };
                            }

                            esbResponses.Add(esbResponse);
                            LogCheckpointRequest("Log CheckpointRequest Result", store, scanCode, esbResponse);
                        }
                    }
                }
            }

            return esbResponses;
        }

        internal void LogCheckpointRequest(string action, string businessUnit, string scanCode, EsbServiceResponse response = null)
        {
            if (response != null)
            {
                if (response.Status == EsbServiceResponseStatus.Sent)
                {
                    logger.Info(JsonConvert.SerializeObject(
                        new
                        {
                            Action = action,
                            Store = businessUnit,
                            ScanCode = scanCode,
                            Response = response
                        }));
                }
                else
                {
                    logger.Error(JsonConvert.SerializeObject(
                        new
                        {
                            Action = action,
                            Store = businessUnit,
                            ScanCode = scanCode,
                            Response = response
                        }));
                }
            }
            else
            {
                logger.Info(JsonConvert.SerializeObject(
                    new
                    {
                        Action = action,
                        Store = businessUnit,
                        ScanCode = scanCode
                    }));
            }
        }
    }
}