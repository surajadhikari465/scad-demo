using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Logging;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using PushController.Common;
using PushController.Common.Models;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace PushController.Controller.PosDataConverters
{
    public class IrmaPushDataConverter : IPosDataConverter<IrmaPushModel>
    {
        private ILogger<IrmaPushDataConverter> logger;
        private IEmailClient emailClient;
        private IIrmaContextProvider contextProvider;
        private IRenewableContext<IrmaContext> globalContext;
        private IQueryHandler<GetAppConfigKeysQuery, List<GetAppConfigKeysResult>> getAppConfigKeysQueryHandler;
        private ICommandHandler<UpdatePublishTableDatesCommand> updatePublishTableDatesCommandHandler;


        public IrmaPushDataConverter(
            ILogger<IrmaPushDataConverter> logger,
            IEmailClient emailClient,
            IIrmaContextProvider contextProvider,
            IQueryHandler<GetAppConfigKeysQuery, List<GetAppConfigKeysResult>> getAppConfigKeysQueryHandler,
            ICommandHandler<UpdatePublishTableDatesCommand> updatePublishTableDatesCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.contextProvider = contextProvider;
            this.getAppConfigKeysQueryHandler = getAppConfigKeysQueryHandler;
            this.updatePublishTableDatesCommandHandler = updatePublishTableDatesCommandHandler;
        }

        public List<IrmaPushModel> ConvertPosData(List<IConPOSPushPublish> posDataReadyToConvert)
        {
            var posDataReadyToBeStaged = new List<IrmaPushModel>();
            var failedConversionRecords = new List<IConPOSPushPublish>();
            var invalidSaleDateRecords = new List<IConPOSPushPublish>();
            
            foreach (var posDataRecord in posDataReadyToConvert)
            {
                try
                {
                    if ((posDataRecord.Sale_Start_Date.HasValue && posDataRecord.Sale_End_Date.HasValue) &&
                        posDataRecord.Sale_Start_Date.Value > posDataRecord.Sale_End_Date.Value)
                    {
                        invalidSaleDateRecords.Add(posDataRecord);
                    }
                    else
                    {
                        if (posDataRecord.Sale_End_Date.HasValue && posDataRecord.Sale_End_Date.Value.Date < DateTime.Now.Date)
                        {
                            logger.Info(String.Format("An expired sale date has been found and will be discarded.  Region: {0}  IConPOSPushPublishID: {1}  Identifier: {2}  Sale End Date: {3}  Store Number: {4}",
                                posDataRecord.RegionCode, posDataRecord.IConPOSPushPublishID, posDataRecord.Identifier, posDataRecord.Sale_End_Date.Value.Date, posDataRecord.Store_No));

                            posDataRecord.Sale_Price = null;
                            posDataRecord.SaleMultiple = null;
                            posDataRecord.Sale_Start_Date = null;
                            posDataRecord.Sale_End_Date = null;
                        }

                        posDataReadyToBeStaged.Add(new IrmaPushModel
                        {
                            IconPosPushPublishId = posDataRecord.IConPOSPushPublishID,
                            RegionCode = posDataRecord.RegionCode,
                            BusinessUnitId = posDataRecord.BusinessUnit_ID,
                            Identifier = posDataRecord.Identifier,
                            ChangeType = posDataRecord.ChangeType,
                            InsertDate = default(DateTime),
                            RetailSize = posDataRecord.RetailSize.Value,
                            RetailPackageUom = posDataRecord.RetailPackageUom,
                            TmDiscountEligible = posDataRecord.TMDiscountEligible.Value,
                            CaseDiscount = posDataRecord.Case_Discount.Value,
                            AgeCode = posDataRecord.AgeCode,
                            Recall = posDataRecord.Recall_Flag.Value,
                            RestrictedHours = posDataRecord.Restricted_Hours.Value,
                            SoldByWeight = posDataRecord.Sold_By_Weight.Value,
                            ScaleForcedTare = posDataRecord.ScaleForcedTare.Value,
                            QuantityRequired = posDataRecord.Quantity_Required.Value,
                            PriceRequired = posDataRecord.Price_Required.Value,
                            QuantityProhibit = posDataRecord.QtyProhibit.Value,
                            VisualVerify = posDataRecord.VisualVerify.Value,
                            RestrictSale = posDataRecord.RestrictSale.Value,
                            PosScaleTare = posDataRecord.POSTare,
                            LinkedIdentifier = posDataRecord.LinkCode_ItemIdentifier,
                            Price = posDataRecord.Price,
                            RetailUom = posDataRecord.RetailUom,
                            Multiple = posDataRecord.Multiple,
                            SaleMultiple = posDataRecord.SaleMultiple,
                            SalePrice = posDataRecord.Sale_Price,
                            SaleStartDate = posDataRecord.Sale_Start_Date,
                            SaleEndDate = posDataRecord.Sale_End_Date,
                            InProcessBy = null,
                            InUdmDate = null,
                            EsbReadyDate = null,
                            UdmFailedDate = null,
                            EsbReadyFailedDate = null
                        });
                    }
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<IrmaPushDataConverter>(this.logger);
                    
                    string message = String.Format("A exception occurred when building the IRMAPush record.  Region: {0}, Scan Code: {1}, IConPOSPushPublishID: {2}",
                        posDataRecord.RegionCode, posDataRecord.Identifier, posDataRecord.IConPOSPushPublishID);

                    exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    failedConversionRecords.Add(posDataRecord);
                }
            }

            logger.Info(String.Format("Generated {0} IRMAPush entities for {1} published POS record(s).",
                posDataReadyToBeStaged.Count, posDataReadyToConvert.Count));

            if (failedConversionRecords.Count > 0)
            {
                ProcessFailedConversionRecords(failedConversionRecords);
            }

            if (invalidSaleDateRecords.Count > 0)
            {
                ProcessInvalidSaleDateRecords(invalidSaleDateRecords);
            }

            return posDataReadyToBeStaged;
        }

        private void ProcessInvalidSaleDateRecords(List<IConPOSPushPublish> invalidSaleDateRecords)
        {
            string regionalConnectionString = ConnectionBuilder.GetConnection(invalidSaleDateRecords[0].RegionCode);
            globalContext = new GlobalIrmaContext(contextProvider.GetRegionalContext(regionalConnectionString), regionalConnectionString);

            try
            {
                SendInvalidSaleDateRecordsNotification(invalidSaleDateRecords);
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler<IrmaPushDataConverter>(this.logger);

                string message = "A failure occurred while attempting to send the invalid sale dates alert email.";
                exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());
            }

            var command = new UpdatePublishTableDatesCommand
            {
                Context = globalContext.Context,
                ProcessedSuccessfully = false,
                PublishedPosData = invalidSaleDateRecords,
                Date = DateTime.Now
            };

            updatePublishTableDatesCommandHandler.Execute(command);
        }

        private void SendInvalidSaleDateRecordsNotification(List<IConPOSPushPublish> invalidSaleDateRecords)
        {
            string errorSubject = Resource.InvalidSaleDatesEmailSubject;
            string errorMessage = Resource.InvalidSaleDatesEmailMessage;
            string emailHost = ConfigurationManager.AppSettings["EmailHost"].ToString();
            string emailPort = ConfigurationManager.AppSettings["EmailPort"].ToString();
            string emailSender = ConfigurationManager.AppSettings["Sender"].ToString();
            string emailBody = EmailHelper.BuildMessageBodyForInvalidSaleDates(errorMessage, invalidSaleDateRecords);
            string[] emailRecipients = GetEmailRecipientsForInvalidSaleDateRecords();
            
            emailClient.Send(emailBody, errorSubject, emailRecipients);
        }

        private string[] GetEmailRecipientsForInvalidSaleDateRecords()
        {
            Dictionary<string, string> appConfigKeysList = new Dictionary<string, string>();
            var query = new GetAppConfigKeysQuery
            {
                Context = globalContext.Context,
                ApplicationName = "POS PUSH JOB"
            };

            var appCongfigKeysResult = getAppConfigKeysQueryHandler.Execute(query);

            foreach (var element in appCongfigKeysResult)
            {
                appConfigKeysList.Add(element.Key, element.Value);
            }

            return appConfigKeysList.ContainsKey("primaryErrorNotification") ? new string[] { appConfigKeysList["primaryErrorNotification"] } : ConfigurationManager.AppSettings["Recipients"].Split(',');
        }

        private void ProcessFailedConversionRecords(List<IConPOSPushPublish> failedConversionRecords)
        {
            string errorMessage = Resource.IrmaPushEntityBuildFailureEmailMessage;
            string emailSubject = Resource.IrmaPushEntityBuildFailureEmailSubject;
            string emailBody = EmailHelper.BuildMessageBodyForFailedIrmaPushConversion(errorMessage, failedConversionRecords);

            try
            {
                emailClient.Send(emailBody, emailSubject);
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler<IrmaPushDataConverter>(this.logger);

                string message = "A failure occurred while attempting to send the alert email.";
                exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());
            }

            string regionalConnectionString = ConnectionBuilder.GetConnection(failedConversionRecords[0].RegionCode);
            globalContext = new GlobalIrmaContext(contextProvider.GetRegionalContext(regionalConnectionString), regionalConnectionString);

            var updatePublishTableDatesCommand = new UpdatePublishTableDatesCommand
            {
                Context = globalContext.Context,
                ProcessedSuccessfully = false,
                PublishedPosData = failedConversionRecords,
                Date = DateTime.Now
            };

            updatePublishTableDatesCommandHandler.Execute(updatePublishTableDatesCommand);
        }
    }
}
