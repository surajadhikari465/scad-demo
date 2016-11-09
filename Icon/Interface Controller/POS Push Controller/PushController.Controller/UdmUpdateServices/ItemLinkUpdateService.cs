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

namespace PushController.Controller.UdmUpdateServices
{
    public class ItemLinkUpdateService : IUdmUpdateService<ItemLinkModel>
    {
        private ILogger<ItemLinkUpdateService> logger;
        private IEmailClient emailClient;
        private ICommandHandler<AddOrUpdateItemLinkBulkCommand> addOrUpdateItemLinkBulkCommandHandler;
        private ICommandHandler<AddOrUpdateItemLinkRowByRowCommand> addOrUpdateItemLinkRowByRowCommandHandler;
        private ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler;

        public ItemLinkUpdateService(
            ILogger<ItemLinkUpdateService> logger,
            IEmailClient emailClient,
            ICommandHandler<AddOrUpdateItemLinkBulkCommand> addOrUpdateItemLinkBulkCommandHandler,
            ICommandHandler<AddOrUpdateItemLinkRowByRowCommand> addOrUpdateItemLinkRowByRowCommandHandler,
            ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.addOrUpdateItemLinkBulkCommandHandler = addOrUpdateItemLinkBulkCommandHandler;
            this.addOrUpdateItemLinkRowByRowCommandHandler = addOrUpdateItemLinkRowByRowCommandHandler;
            this.updateStagingTableDatesForUdmCommandHandler = updateStagingTableDatesForUdmCommandHandler;
        }

        public void SaveEntitiesBulk(List<ItemLinkModel> entitiesToSave)
        {
            var itemLinkEntities = entitiesToSave.ConvertAll(e => new ItemLink
            {
                parentItemID = e.ParentItemId,
                childItemID = e.ChildItemId,
                localeID = e.LocaleId
            });

            var command = new AddOrUpdateItemLinkBulkCommand
            {
                ItemLinks = itemLinkEntities
            };

            try
            {
                addOrUpdateItemLinkBulkCommandHandler.Execute(command);
            }
            catch (Exception ex)
            {
                string errorMessage;
                var exceptionHandler = new ExceptionHandler<ItemLinkUpdateService>(this.logger);

                if (ex.IsTransientError())
                {
                    errorMessage = "An error occurred when bulk inserting ItemLink entities.  The error appears to be transient.  Attempting to retry...";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    int retryMax = 5;
                    int retryCount = 1;
                    while (retryCount <= retryMax)
                    {
                        try
                        {
                            Thread.Sleep(1000 * retryCount);
                            addOrUpdateItemLinkBulkCommandHandler.Execute(command);
                            logger.Info(String.Format("Retry attempt {0} of {1} has succeeded.  The entities have been inserted into ItemLink.", retryCount, retryMax));
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
                    throw new Exception("Unable to successfully complete the bulk entity insert.");
                }
                else
                {
                    errorMessage = "An error occurred when bulk inserting ItemLink entities.  The error appears to be intransient.  Falling back to row-by-row processing.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    string failureMessage = Resource.ItemLinkEntitySaveFallbackEmailMessage;
                    string emailSubject = Resource.ItemLinkEntitySaveFallbackEmailSubject;
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

                    throw new Exception("Unable to successfully complete the bulk entity insert.");
                }
            }

            logger.Info(String.Format("Saved {0} ItemLink entities generated from the staged POS data beginning with IRMAPushID {1}.",
                itemLinkEntities.Count, entitiesToSave[0].IrmaPushId));
        }

        public void SaveEntitiesRowByRow(List<ItemLinkModel> entitiesToSave)
        {
            var failedEntities = new List<ItemLinkModel>();
            var command = new AddOrUpdateItemLinkRowByRowCommand();

            foreach (var entity in entitiesToSave)
            {
                var itemLinkEntity = new ItemLink
                {
                    parentItemID = entity.ParentItemId,
                    childItemID = entity.ChildItemId,
                    localeID = entity.LocaleId
                };

                command.ItemLinkEntity = itemLinkEntity;

                try
                {
                    addOrUpdateItemLinkRowByRowCommandHandler.Execute(command);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemLinkUpdateService>(this.logger);

                    string errorMessage = String.Format("Failed to insert the entity to ItemLink:  IRMAPushID: {0}, parentItemID: {1}, childItemID: {2}, localeID: {3}",
                        entity.IrmaPushId, entity.ParentItemId, entity.ChildItemId, entity.LocaleId);

                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    failedEntities.Add(entity);
                }
            }

            if (failedEntities.Count > 0)
            {
                string failureMessage = Resource.ItemLinkEntitySaveFailureEmailMessage;
                string emailSubject = Resource.ItemLinkEntitySaveFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForItemLinkInsertRowByRowFailure(failureMessage, failedEntities);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemLinkUpdateService>(this.logger);

                    string errorMessage = "A failure occurred while attempting to send the email alert.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());
                }

                var updateStagingTableCommand = new UpdateStagingTableDatesForUdmCommand
                {
                    ProcessedSuccessfully = false,
                    StagedPosData = failedEntities.Select(e => new IRMAPush { IRMAPushID = e.IrmaPushId }).ToList(),
                    Date = DateTime.Now
                };

                updateStagingTableDatesForUdmCommandHandler.Execute(updateStagingTableCommand);
            }
        }
    }
}
