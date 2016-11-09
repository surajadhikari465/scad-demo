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
    public class ItemPriceUpdateService : IUdmUpdateService<ItemPriceModel>
    {
        private ILogger<ItemPriceUpdateService> logger;
        private IEmailClient emailClient;
        private ICommandHandler<AddOrUpdateItemPriceBulkCommand> addOrUpdateItemPriceBulkCommandHandler;
        private ICommandHandler<AddOrUpdateItemPriceRowByRowCommand> addOrUpdateItemPriceRowByRowCommandHandler;
        private ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler;

        public ItemPriceUpdateService(
            ILogger<ItemPriceUpdateService> logger,
            IEmailClient emailClient,
            ICommandHandler<AddOrUpdateItemPriceBulkCommand> addOrUpdateItemPriceBulkCommandHandler,
            ICommandHandler<AddOrUpdateItemPriceRowByRowCommand> addOrUpdateItemPriceRowByRowCommandHandler,
            ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.addOrUpdateItemPriceBulkCommandHandler = addOrUpdateItemPriceBulkCommandHandler;
            this.addOrUpdateItemPriceRowByRowCommandHandler = addOrUpdateItemPriceRowByRowCommandHandler;
            this.updateStagingTableDatesForUdmCommandHandler = updateStagingTableDatesForUdmCommandHandler;
        }

        public void SaveEntitiesBulk(List<ItemPriceModel> entitiesToSave)
        {
            var itemPriceEntities = entitiesToSave.ConvertAll(e => new ItemPrice
                {
                    itemID = e.ItemId,
                    localeID = e.LocaleId,
                    itemPriceTypeID = e.ItemPriceTypeId,
                    uomID = e.UomId,
                    currencyTypeID = e.CurrencyTypeId,
                    itemPriceAmt = e.ItemPriceAmount,
                    breakPointStartQty = e.BreakPointStartQuantity,
                    startDate = e.StartDate,
                    endDate = e.EndDate
                });

            var command = new AddOrUpdateItemPriceBulkCommand
            {
                ItemPrices = itemPriceEntities
            };

            try
            {
                addOrUpdateItemPriceBulkCommandHandler.Execute(command);
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler<ItemPriceUpdateService>(this.logger);
                string errorMessage;

                if (ex.IsTransientError())
                {
                    errorMessage = "An error occurred when bulk inserting ItemPrice entities.  The error appears to be transient.  Attempting to retry...";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    int retryMax = 5;
                    int retryCount = 1;
                    while (retryCount <= retryMax)
                    {
                        try
                        {
                            Thread.Sleep(1000 * retryCount);
                            addOrUpdateItemPriceBulkCommandHandler.Execute(command);
                            logger.Info(String.Format("Retry attempt {0} of {1} has succeeded.  The entities have been inserted into ItemPrice.", retryCount, retryMax));
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
                    errorMessage = "An error occurred when bulk inserting ItemPrice entities.  The error appears to be intransient.  Falling back to row-by-row processing.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    string failureMessage = Resource.ItemPriceEntitySaveFallbackEmailMessage;
                    string emailSubject = Resource.ItemPriceEntitySaveFallbackEmailSubject;
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

            logger.Info(String.Format("Saved {0} ItemPrice entities generated from the staged POS data beginning with IRMAPushID {1}.",
                itemPriceEntities.Count, entitiesToSave[0].IrmaPushId));
        }

        public void SaveEntitiesRowByRow(List<ItemPriceModel> entitiesToSave)
        {
            var failedEntities = new List<ItemPriceModel>();
            var command = new AddOrUpdateItemPriceRowByRowCommand();

            foreach (var entity in entitiesToSave)
            {
                var itemPriceEntity = new ItemPrice
                {
                    itemID = entity.ItemId,
                    localeID = entity.LocaleId,
                    itemPriceTypeID = entity.ItemPriceTypeId,
                    uomID = entity.UomId,
                    currencyTypeID = entity.CurrencyTypeId,
                    itemPriceAmt = entity.ItemPriceAmount,
                    breakPointStartQty = entity.BreakPointStartQuantity,
                    startDate = entity.StartDate,
                    endDate = entity.EndDate
                };

                command.ItemPriceEntity = itemPriceEntity;

                try
                {
                    addOrUpdateItemPriceRowByRowCommandHandler.Execute(command);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemPriceUpdateService>(this.logger);
                    
                    string errorMessage = String.Format("Failed to insert the entity to ItemPrice:  IRMAPushID: {0}, itemID: {1}, localeID: {2}, itemPriceTypeID: {3}, itemPriceAmt: {4}",
                        entity.IrmaPushId, entity.ItemId, entity.LocaleId, entity.ItemPriceTypeId, entity.ItemPriceAmount);
                    
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    failedEntities.Add(entity);
                }
            }

            if (failedEntities.Count > 0)
            {
                string failureMessage = Resource.ItemPriceEntitySaveFailureEmailMessage;
                string emailSubject = Resource.ItemPriceEntitySaveFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForItemPriceInsertRowByRowFailure(failureMessage, failedEntities);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemPriceUpdateService>(this.logger);
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
