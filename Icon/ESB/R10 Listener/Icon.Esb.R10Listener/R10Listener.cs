using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.R10Listener.Models;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using TIBCO.EMS;

namespace Icon.Esb.R10Listener
{
    public class R10Listener : ListenerApplication<R10Listener, ListenerApplicationSettings>
    {
        private ICommandHandler<SaveR10MessageResponseCommand> saveR10MessageResponseCommandHandler;
        private IMessageParser<R10MessageResponseModel> messageParser;

        public R10Listener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            ICommandHandler<SaveR10MessageResponseCommand> saveR10MessageResponseCommandHandler,
            IMessageParser<R10MessageResponseModel> messageParser,
            IEmailClient emailClient,
            ILogger<R10Listener> logger)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.saveR10MessageResponseCommandHandler = saveR10MessageResponseCommandHandler;
            this.messageParser = messageParser;
            
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            R10MessageResponseModel r10MessageResponse = null;

            try
            {
                r10MessageResponse = messageParser.ParseMessage(args.Message);
            }
            catch (Exception ex)
            {
                LogUnsuccessfulParse(args, ex);
            }

            if (r10MessageResponse != null)
            {
                try
                {
                    if (!r10MessageResponse.RequestSuccess)
                    {
                        LogUnsuccessfulMessage(r10MessageResponse);
                    }

                    saveR10MessageResponseCommandHandler.Execute(new SaveR10MessageResponseCommand
                        {
                            R10MessageResponseModel = r10MessageResponse
                        });
                }
                catch (Exception ex)
                {
                    LogUnsuccessfulMessageProcessing(r10MessageResponse, ex);
                }
            }

            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }

        private void LogUnsuccessfulMessage(R10MessageResponseModel messageResponse)
        {
            logger.Error(
                JsonConvert.SerializeObject(
                    new
                    {
                        Message = NotificationConstants.UnsuccessfulRequest,
                        R10MessageResponseModel = messageResponse
                    }));
        }

        private void LogUnsuccessfulParse(EsbMessageEventArgs args, Exception ex)
        {
            logger.Error(
                JsonConvert.SerializeObject(
                    new
                    {
                        Message = NotificationConstants.UnsuccessfulParse,
                        EsbMessage = args.Message,
                        Exception = ex.ToString()
                    }));
        }

        private void LogUnsuccessfulMessageProcessing(R10MessageResponseModel messageResponse, Exception ex)
        {
            logger.Error(
                JsonConvert.SerializeObject(
                    new
                    {
                        Message = NotificationConstants.UnsuccessfulMessageProcessing,
                        R10MessageResponseModel = messageResponse,
                        Exception = ex.ToString()
                    }));
        }
    }
}
