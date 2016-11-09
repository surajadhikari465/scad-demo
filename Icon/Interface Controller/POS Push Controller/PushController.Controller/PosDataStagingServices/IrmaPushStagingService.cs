using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using PushController.Common;
using PushController.Common.Models;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace PushController.Controller.PosDataStagingServices
{
    public class IrmaPushStagingService : IPosDataStagingService<IrmaPushModel>
    {
        private ILogger<IrmaPushStagingService> logger;
        private IIrmaContextProvider contextProvider;
        private IRenewableContext<IrmaContext> globalContext;
        private IEmailClient emailClient;
        private ICommandHandler<StagePosDataBulkCommand> stagePosDataBulkCommandHandler;
        private ICommandHandler<StagePosDataRowByRowCommand> stagePosDataRowByRowCommandHandler;
        private ICommandHandler<UpdatePublishTableDatesCommand> updatePublishTableDatesCommandHandler;

        public IrmaPushStagingService(
            ILogger<IrmaPushStagingService> logger,
            IIrmaContextProvider contextProvider,
            IEmailClient emailClient,
            ICommandHandler<StagePosDataBulkCommand> stagePosDataBulkCommandHandler,
            ICommandHandler<StagePosDataRowByRowCommand> stagePosDataRowByRowCommandHandler,
            ICommandHandler<UpdatePublishTableDatesCommand> updatePublishTableDatesCommandHandler)
        {
            this.logger = logger;
            this.contextProvider = contextProvider;
            this.emailClient = emailClient;
            this.stagePosDataBulkCommandHandler = stagePosDataBulkCommandHandler;
            this.stagePosDataRowByRowCommandHandler = stagePosDataRowByRowCommandHandler;
            this.updatePublishTableDatesCommandHandler = updatePublishTableDatesCommandHandler;
        }

        public void StagePosDataBulk(List<IrmaPushModel> posDataReadyToStage)
        {
            var irmaPushEntities = posDataReadyToStage.ConvertAll(pos => new IRMAPush
                {
                    RegionCode = pos.RegionCode,
                    BusinessUnit_ID = pos.BusinessUnitId,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = pos.InsertDate,
                    RetailSize = pos.RetailSize,
                    RetailPackageUom = pos.RetailPackageUom,
                    TMDiscountEligible = pos.TmDiscountEligible,
                    Case_Discount = pos.CaseDiscount,
                    AgeCode = pos.AgeCode,
                    Recall_Flag = pos.Recall,
                    Restricted_Hours = pos.RestrictedHours,
                    Sold_By_Weight = pos.SoldByWeight,
                    ScaleForcedTare = pos.ScaleForcedTare,
                    Quantity_Required = pos.QuantityRequired,
                    Price_Required = pos.PriceRequired,
                    QtyProhibit = pos.QuantityProhibit,
                    VisualVerify = pos.VisualVerify,
                    RestrictSale = pos.RestrictSale,
                    PosScaleTare = pos.PosScaleTare,
                    LinkedIdentifier = pos.LinkedIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    Sale_Price = pos.SalePrice,
                    Sale_Start_Date = pos.SaleStartDate,
                    Sale_End_Date = pos.SaleEndDate,
                    InProcessBy = pos.InProcessBy,
                    InUdmDate = pos.InUdmDate,
                    EsbReadyDate = pos.EsbReadyDate,
                    UdmFailedDate = pos.UdmFailedDate,
                    EsbReadyFailedDate = pos.EsbReadyFailedDate
                });

            var command = new StagePosDataBulkCommand
            {
                PosData = irmaPushEntities
            };

            try
            {
                stagePosDataBulkCommandHandler.Execute(command);
            }
            catch (Exception ex)
            {
                string errorMessage;
                var exceptionHandler = new ExceptionHandler<IrmaPushStagingService>(this.logger);

                if (ex.IsTransientError())
                {
                    errorMessage = "An error occurred when bulk inserting IRMAPush entities.  The error appears to be transient.  Attempting to retry...";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    int retryMax = 5;
                    int retryCount = 1;
                    while (retryCount <= retryMax)
                    {
                        try
                        {
                            Thread.Sleep(1000 * retryCount);
                            stagePosDataBulkCommandHandler.Execute(command);
                            logger.Info(String.Format("Retry attempt {0} of {1} has succeeded.  The entities have been inserted into IRMAPush.", retryCount, retryMax));
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
                    errorMessage = "An error occurred when bulk inserting IRMAPush entities.  The error appears to be intransient.  Falling back to row-by-row processing.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    string failureMessage = Resource.IrmaPushEntitySaveFallbackEmailMessage;
                    string emailSubject = Resource.IrmaPushEntitySaveFallbackEmailSubject;
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
        }

        public void StagePosDataRowByRow(List<IrmaPushModel> posDataReadyToStage)
        {
            var failedEntities = new List<IrmaPushModel>();
            var command = new StagePosDataRowByRowCommand();

            foreach (var posDataRecord in posDataReadyToStage)
            {
                var irmaPushEntity = new IRMAPush
                {
                    RegionCode = posDataRecord.RegionCode,
                    BusinessUnit_ID = posDataRecord.BusinessUnitId,
                    Identifier = posDataRecord.Identifier,
                    ChangeType = posDataRecord.ChangeType,
                    InsertDate = DateTime.Now,
                    RetailSize = posDataRecord.RetailSize,
                    RetailPackageUom = posDataRecord.RetailPackageUom,
                    TMDiscountEligible = posDataRecord.TmDiscountEligible,
                    Case_Discount = posDataRecord.CaseDiscount,
                    AgeCode = posDataRecord.AgeCode,
                    Recall_Flag = posDataRecord.Recall,
                    Restricted_Hours = posDataRecord.RestrictedHours,
                    Sold_By_Weight = posDataRecord.SoldByWeight,
                    ScaleForcedTare = posDataRecord.ScaleForcedTare,
                    Quantity_Required = posDataRecord.QuantityRequired,
                    Price_Required = posDataRecord.PriceRequired,
                    QtyProhibit = posDataRecord.QuantityProhibit,
                    VisualVerify = posDataRecord.VisualVerify,
                    RestrictSale = posDataRecord.RestrictSale,
                    PosScaleTare = posDataRecord.PosScaleTare,
                    LinkedIdentifier = posDataRecord.LinkedIdentifier,
                    Price = posDataRecord.Price,
                    RetailUom = posDataRecord.RetailUom,
                    Multiple = posDataRecord.Multiple,
                    SaleMultiple = posDataRecord.SaleMultiple,
                    Sale_Price = posDataRecord.SalePrice,
                    Sale_Start_Date = posDataRecord.SaleStartDate,
                    Sale_End_Date = posDataRecord.SaleEndDate,
                    InProcessBy = posDataRecord.InProcessBy,
                    InUdmDate = posDataRecord.InUdmDate,
                    EsbReadyDate = posDataRecord.EsbReadyDate,
                    UdmFailedDate = posDataRecord.UdmFailedDate,
                    EsbReadyFailedDate = posDataRecord.EsbReadyFailedDate
                };

                command.PosDataEntity = irmaPushEntity;

                try
                {
                    stagePosDataRowByRowCommandHandler.Execute(command);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<IrmaPushStagingService>(this.logger);

                    string errorMessage = String.Format("Failed to insert the entity to IRMAPush:  IconPosPushPublish ID: {0}, Region: {1}, Scan Code: {2}",
                        posDataRecord.IconPosPushPublishId, posDataRecord.RegionCode, posDataRecord.Identifier);
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    failedEntities.Add(posDataRecord);
                }
            }

            if (failedEntities.Count > 0)
            {
                string failureMessage = Resource.IrmaPushEntitySaveFailureEmailMessage;
                string emailSubject = Resource.IrmaPushEntitySaveFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForIrmaPushRowByRowFailure(failureMessage, failedEntities);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<IrmaPushStagingService>(this.logger);

                    string errorMessage = "A failure occurred while attempting to send the email alert.";
                    exceptionHandler.HandleException(errorMessage, ex, this.GetType(), MethodBase.GetCurrentMethod());
                }

                string regionalConnectionString = ConnectionBuilder.GetConnection(failedEntities[0].RegionCode);
                globalContext = new GlobalIrmaContext(contextProvider.GetRegionalContext(regionalConnectionString), regionalConnectionString);

                var failedPublishedPosData = failedEntities.Select(fe => new IConPOSPushPublish { IConPOSPushPublishID = fe.IconPosPushPublishId }).ToList();

                var updatePublishTableDatesCommand = new UpdatePublishTableDatesCommand
                {
                    Context = globalContext.Context,
                    ProcessedSuccessfully = false,
                    PublishedPosData = failedPublishedPosData,
                    Date = DateTime.Now
                };

                updatePublishTableDatesCommandHandler.Execute(updatePublishTableDatesCommand);
            }
        }
    }
}
