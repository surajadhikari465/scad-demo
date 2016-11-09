using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using PushController.Common;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace PushController.Controller.MessageQueueServices
{
    public class ItemLocaleMessageQueueService : IMessageQueueService<MessageQueueItemLocale>
    {
        private ILogger<ItemLocaleMessageQueueService> logger;
        private IEmailClient emailClient;
        private ICommandHandler<AddItemLocaleMessagesBulkCommand> addItemLocaleMessagesBulkCommandHandler;
        private ICommandHandler<AddItemLocaleMessagesRowByRowCommand> addItemLocaleMessagesRowByRowCommandHandler;
        private ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler;

        public ItemLocaleMessageQueueService(
            ILogger<ItemLocaleMessageQueueService> logger,
            IEmailClient emailClient,
            ICommandHandler<AddItemLocaleMessagesBulkCommand> addItemLocaleMessagesBulkCommandHandler,
            ICommandHandler<AddItemLocaleMessagesRowByRowCommand> addItemLocaleMessagesRowByRowCommandHandler,
            ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.addItemLocaleMessagesBulkCommandHandler = addItemLocaleMessagesBulkCommandHandler;
            this.addItemLocaleMessagesRowByRowCommandHandler = addItemLocaleMessagesRowByRowCommandHandler;
            this.updateStagingTableDatesForEsbCommandHandler = updateStagingTableDatesForEsbCommandHandler;
        }

        public void SaveMessagesBulk(List<MessageQueueItemLocale> messagesToSave)
        {
            var command = new AddItemLocaleMessagesBulkCommand
            {
                Messages = messagesToSave
            };

            try
            {
                addItemLocaleMessagesBulkCommandHandler.Execute(command);
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler<ItemLocaleMessageQueueService>(this.logger);
                string errorMessage;

                if (ex.IsTransientError())
                {
                    errorMessage = "An error occurred when bulk inserting ItemLocale messages.  The error appears to be transient.  Attempting to retry...";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    int retryMax = 5;
                    int retryCount = 1;
                    while (retryCount <= retryMax)
                    {
                        try
                        {
                            Thread.Sleep(1000 * retryCount);
                            addItemLocaleMessagesBulkCommandHandler.Execute(command);
                            logger.Info(String.Format("Retry attempt {0} of {1} has succeeded.  The messages have been inserted into MessageQueueItemLocale.", retryCount, retryMax));
                            return;
                        }
                        catch (Exception)
                        {
                            errorMessage = String.Format("Retry attempt {0} of {1} has failed.", retryCount, retryMax);
                            exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());
                            retryCount++;
                        }
                    }

                    logger.Warn("All retry attempts have failed.  Falling back to row-by-row processing.");
                    throw new Exception("Unable to successfully complete the bulk message insert.");
                }
                else
                {
                    errorMessage = "An error occurred when bulk inserting ItemLocale messages.  The error appears to be intransient.  Falling back to row-by-row processing.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    string failureMessage = Resource.ItemLocaleMessageSaveFallbackEmailMessage;
                    string emailSubject = Resource.ItemLocaleMessageSaveFallbackEmailSubject;
                    string emailBody = EmailHelper.BuildMessageBodyForBulkInsertFailure(failureMessage, ex.ToString());

                    try
                    {
                        emailClient.Send(emailBody, emailSubject);
                    }
                    catch (Exception emailEx)
                    {
                        errorMessage = "A failure occurred while attempting to send the email alert.";
                        exceptionHandler.HandleException(errorMessage, emailEx, this.GetType(), MethodBase.GetCurrentMethod());
                    }

                    throw new Exception("Unable to successfully complete the bulk message insert.");
                }
            }
        }

        public void SaveMessagesRowByRow(List<MessageQueueItemLocale> messagesToSave)
        {
            var failedMessages = new List<MessageQueueItemLocale>();
            var addMessageCommand = new AddItemLocaleMessagesRowByRowCommand();

            foreach (var message in messagesToSave)
            {
                addMessageCommand.Message = message;

                try
                {
                    addItemLocaleMessagesRowByRowCommandHandler.Execute(addMessageCommand);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemLocaleMessageQueueService>(this.logger);

                    string errorMessage = String.Format("Failed to insert the message to the MessageQueueItemLocale table:  IRMAPushID: {0}, ScanCode: {1}, Region: {2}",
                        message.IRMAPushID, message.ScanCode, message.RegionCode);

                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    failedMessages.Add(message);
                }
            }

            if (failedMessages.Count > 0)
            {
                string failureMessage = Resource.ItemLocaleMessageSaveFailureEmailMessage;
                string emailSubject = Resource.ItemLocaleMessageSaveFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForItemLocaleMessageInsertRowByRowFailure(failureMessage, failedMessages);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemLocaleMessageQueueService>(this.logger);
                    string errorMessage = "A failure occurred while attempting to send the email alert.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());
                }

                var updateEsbFailedDateCommand = new UpdateStagingTableDatesForEsbCommand
                {
                    ProcessedSuccessfully = false,
                    StagedPosData = failedMessages.Select(m => new IRMAPush { IRMAPushID = m.IRMAPushID }).ToList(),
                    Date = DateTime.Now
                };

                updateStagingTableDatesForEsbCommandHandler.Execute(updateEsbFailedDateCommand);
            }
        }
    }
}
