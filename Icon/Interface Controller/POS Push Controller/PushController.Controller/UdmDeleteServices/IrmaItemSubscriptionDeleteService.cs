using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using PushController.Common;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace PushController.Controller.UdmDeleteServices
{
    public class IrmaItemSubscriptionDeleteService : IUdmDeleteService<IRMAItemSubscription>
    {
        private ILogger<IrmaItemSubscriptionDeleteService> logger;
        private IEmailClient emailClient;
        private ICommandHandler<DeleteItemSubscriptionCommand> deleteItemSubscriptionCommandHandler;
        
        public IrmaItemSubscriptionDeleteService(
            ILogger<IrmaItemSubscriptionDeleteService> logger,
            IEmailClient emailClient,
            ICommandHandler<DeleteItemSubscriptionCommand> deleteItemSubscriptionCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.deleteItemSubscriptionCommandHandler = deleteItemSubscriptionCommandHandler;
        }

        public void DeleteEntitiesBulk(List<IRMAItemSubscription> deleteSubscriptionsList)
        {
            var command = new DeleteItemSubscriptionCommand
            {
                Subscriptions = deleteSubscriptionsList
            };

            try
            {
                deleteItemSubscriptionCommandHandler.Execute(command);
            }
            catch (Exception ex)
            {
                string errorMessage;
                var exceptionHandler = new ExceptionHandler<IrmaItemSubscriptionDeleteService>(this.logger);

                if (ex.IsTransientError())
                {
                    errorMessage = "An error occurred when bulk deleting item subscriptions.  The error appears to be transient.  Attempting to retry...";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    int retryMax = 5;
                    int retryCount = 1;
                    while (retryCount <= retryMax)
                    {
                        try
                        {
                            Thread.Sleep(1000 * retryCount);
                            deleteItemSubscriptionCommandHandler.Execute(command);
                            logger.Info(String.Format("Retry attempt {0} of {1} has succeeded.  The item subscriptions have been deleted.", retryCount, retryMax));
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
                    throw new Exception("Unable to successfully complete the bulk item subscription delete.");
                }
                else
                {
                    errorMessage = "An error occurred when bulk deleting item subscriptions.  The error appears to be intransient.  Falling back to row-by-row processing.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    string failureMessage = Resource.ItemSubscriptionDeleteFailureEmailMessage;
                    string emailSubject = Resource.ItemSubscriptionDeleteFallbackEmailSubject;
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

                    throw new Exception("Unable to successfully complete the bulk item subscription delete.");
                }
            }
        }

        public void DeleteEntitiesRowByRow(List<IRMAItemSubscription> subscriptions)
        {
            var failedMessages = new List<IRMAPush>();
            var deleteItemSubscriptionCommand = new DeleteItemSubscriptionCommand();

            foreach (var subscription in subscriptions)
            {
                deleteItemSubscriptionCommand.Subscriptions = new List<IRMAItemSubscription> { subscription };

                try
                {
                    deleteItemSubscriptionCommandHandler.Execute(deleteItemSubscriptionCommand);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<IrmaItemSubscriptionDeleteService>(this.logger);

                    string errorMessage = String.Format("Failed to delete item subscritpion:  ScanCode: {0}, Region: {1}",
                       subscription.identifier, subscription.regioncode);
                    
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());
                }
            }
        }
    }
}
