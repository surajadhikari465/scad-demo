﻿using Icon.ApiController.Common;
using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.Common.Email;
using Icon.Logging;
using System;
using System.Reflection;
using System.Threading;
using Icon.Esb.Producer;

namespace Icon.ApiController.Controller
{
    public class ApiControllerBase
    {
        private ILogger<ApiControllerBase> logger;
        private IEmailClient emailClient;
        private IHistoryProcessor historyProcessor;
        private IQueueProcessor queueProcessor;
        private IEsbProducer producer;

        public ApiControllerBase(
            ILogger<ApiControllerBase> logger,
            IEmailClient emailClient,
            IHistoryProcessor historyProcessor, 
            IQueueProcessor queueProcessor,
            IEsbProducer producer)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.historyProcessor = historyProcessor;
            this.queueProcessor = queueProcessor;

            this.producer = producer;
        }

        public void Execute()
        {
            try
            {
                historyProcessor.ProcessMessageHistory();
            }
            catch (Exception ex)
            {
                ExceptionLogger<ApiControllerBase> exceptionLogger = new ExceptionLogger<ApiControllerBase>(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                string errorMessage = string.Format(Resource.HistoryProcessorUnhandledExceptionMessage, ControllerType.Type, ControllerType.Instance);
                string emailSubject = Resource.HistoryProcessorUnhandledExceptionEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForUnhandledException(errorMessage, ex.ToString());

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception mailEx)
                {
                    string message = "A failure occurred while attempting to send the alert email.";
                    exceptionLogger.LogException(message, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
                }

                // Sleep on unhandled exception to prevent email/logging floods.
                Thread.Sleep(30000);
                return;
            }

            try
            {
                queueProcessor.ProcessMessageQueue();
            }
            catch (Exception ex)
            {
                ExceptionLogger<ApiControllerBase> exceptionLogger = new ExceptionLogger<ApiControllerBase>(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                string errorMessage = string.Format(Resource.QueueProcessorUnhandledExceptionMessage, ControllerType.Type, ControllerType.Instance);
                string emailSubject = Resource.QueueProcessorUnhandledExceptionEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForUnhandledException(errorMessage, ex.ToString());

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception mailEx)
                {
                    string message = "A failure occurred while attempting to send the alert email.";
                    exceptionLogger.LogException(message, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
                }

                // Sleep on unhandled exception to prevent email/logging floods.
                Thread.Sleep(30000);

                return;
            }
        }
    }
}
