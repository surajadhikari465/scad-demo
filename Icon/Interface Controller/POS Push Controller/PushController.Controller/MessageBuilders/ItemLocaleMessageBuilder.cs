using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PushController.Controller.MessageBuilders
{
    public class ItemLocaleMessageBuilder : IMessageBuilder<MessageQueueItemLocale>
    {
        private ILogger<ItemLocaleMessageBuilder> logger;
        private IEmailClient emailClient;
        private ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper;
        private ICacheHelper<Tuple<string, int>, string> linkedScanCodeCacheHelper;
        private ICacheHelper<int, Locale> localeCacheHelper;
        private IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler;
        private ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler;
        
        public ItemLocaleMessageBuilder(
            ILogger<ItemLocaleMessageBuilder> logger,
            IEmailClient emailClient,
            ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper,
            ICacheHelper<Tuple<string, int>, string> linkedScanCodeCacheHelper,
            ICacheHelper<int, Locale> localeCacheHelper,
            IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler,
            ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.scanCodeCacheHelper = scanCodeCacheHelper;
            this.linkedScanCodeCacheHelper = linkedScanCodeCacheHelper;
            this.localeCacheHelper = localeCacheHelper;
            this.getLocaleQueryHandler = getLocaleQueryHandler;
            this.updateStagingTableDatesForEsbCommandHandler = updateStagingTableDatesForEsbCommandHandler;
        }

        public List<MessageQueueItemLocale> BuildMessages(List<IRMAPush> posDataReadyForEsb)
        {
            var itemLocaleMessages = new List<MessageQueueItemLocale>();
            var failedRecords = new List<IrmaPushModel>();

            foreach (var posDataRecord in posDataReadyForEsb)
            {
                try
                {
                    var scanCodeModel = GetScanCode(posDataRecord.Identifier);

                    if (PreventMessageGeneration(scanCodeModel))
                    {
                        continue;
                    }

                    switch (posDataRecord.ChangeType)
                    {
                        case Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange:
                        case Constants.IrmaPushChangeTypes.ScanCodeAdd:
                        case Constants.IrmaPushChangeTypes.ScanCodeAuthorization:
                            {
                                itemLocaleMessages.Add(BuildItemLocaleMessage(posDataRecord, scanCodeModel, MessageActionTypes.AddOrUpdate));
                                break;
                            }
                        case Constants.IrmaPushChangeTypes.ScanCodeDeauthorization:
                        case Constants.IrmaPushChangeTypes.ScanCodeDelete:
                            {
                                itemLocaleMessages.Add(BuildItemLocaleMessage(posDataRecord, scanCodeModel, MessageActionTypes.Delete));
                                break;
                            }
                        case Constants.IrmaPushChangeTypes.RegularPriceChange:
                        case Constants.IrmaPushChangeTypes.NonRegularPriceChange:
                        case Constants.IrmaPushChangeTypes.CancelAllSales:
                            {
                                break;
                            }
                        default:
                            {
                                logger.Error(String.Format("ChangeType '{0}' is not a valid value.  IRMAPushID: {1}", posDataRecord.ChangeType, posDataRecord.IRMAPushID));

                                IrmaPushModel failedRecord = posDataRecord.ToModel();
                                failedRecord.MessageBuildError = "Invalid change type.";

                                failedRecords.Add(failedRecord);
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemLocaleMessageBuilder>(this.logger);

                    string message = String.Format("A exception occurred when building the ItemLocale message.  Region: {0}, Scan Code: {1}, IRMAPushID: {2}",
                        posDataRecord.RegionCode, posDataRecord.Identifier, posDataRecord.IRMAPushID);

                    exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    var failedRecord = posDataRecord.ToModel();
                    failedRecord.MessageBuildError = ex.Message;

                    failedRecords.Add(failedRecord);
                }
            }

            if (failedRecords.Count > 0)
            {
                string failureMessage = Resource.ItemLocaleMessageBuildFailureEmailMessage;
                string emailSubject = Resource.ItemLocaleMessageBuildFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForMessageBuildFailure(failureMessage, failedRecords);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemLocaleMessageBuilder>(this.logger);

                    string message = "A failure occurred while attempting to send the alert email.";

                    exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());
                }

                var command = new UpdateStagingTableDatesForEsbCommand
                {
                    ProcessedSuccessfully = false,
                    StagedPosData = failedRecords.ToEntities(),
                    Date = DateTime.Now
                };

                updateStagingTableDatesForEsbCommandHandler.Execute(command);
            }

            logger.Info(String.Format("Generated {0} ItemLocale message(s) for {1} staged POS record(s).",
                itemLocaleMessages.Count, posDataReadyForEsb.Count));

            return itemLocaleMessages;
        }

        private bool PreventMessageGeneration(ScanCodeModel scanCodeModel)
        {
            if (StartupOptions.UseItemTypeInsteadOfNonMerchTrait)
            {
                if (scanCodeModel.ItemTypeCode == ItemTypeCodes.Coupon)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} has item type {ItemTypes.Descriptions.Coupon}.  No ItemLocale message will be generated.");
                    return true;
                }

                if (scanCodeModel.ItemTypeCode == ItemTypeCodes.NonRetail)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} has item type {ItemTypes.Descriptions.NonRetail}.  No ItemLocale message will be generated.");
                    return true;
                }
            }
            else
            {
                if (scanCodeModel.NonMerchandiseTrait == NonMerchandiseTraits.Coupon)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} is associated to a sub-brick with the {NonMerchandiseTraits.Coupon} non-merchandise trait.  No ItemLocale message will be generated.");
                    return true;
                }

                if (scanCodeModel.NonMerchandiseTrait == NonMerchandiseTraits.LegacyPosOnly)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} is associated to a sub-brick with the {NonMerchandiseTraits.LegacyPosOnly} non-merchandise trait.  No ItemLocale message will be generated.");
                    return true;
                }
            }

            if (scanCodeModel.DepartmentSaleTrait == "1")
            {
                logger.Info(String.Format("Scan code {0} has the department sale trait, so no ItemLocale message will be generated.",
                    scanCodeModel.ScanCode));
                return true;
            }

            return false;
        }

        private MessageQueueItemLocale BuildItemLocaleMessage(IRMAPush posData, ScanCodeModel scanCodeModel, int messageActionId)
        {
            Locale localeEntity = GetLocale(posData.BusinessUnit_ID);
            string currentLinkedScanCode = GetCurrentLinkedScanCode(posData.Identifier, posData.BusinessUnit_ID);
            string newLinkedScanCode = GetNewLinkedScanCode(posData.LinkedIdentifier);

            var message = new MessageQueueItemLocale
            {
                MessageTypeId = MessageTypes.ItemLocale,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                MessageActionId = messageActionId,
                IRMAPushID = posData.IRMAPushID,
                InsertDate = DateTime.Now,
                RegionCode = posData.RegionCode,
                BusinessUnit_ID = posData.BusinessUnit_ID,
                ItemId = scanCodeModel.ItemId,
                ItemTypeCode = scanCodeModel.ItemTypeCode,
                ItemTypeDesc = scanCodeModel.ItemTypeDesc,
                LocaleId = localeEntity.localeID,
                LocaleName = localeEntity.localeName,
                ScanCodeId = scanCodeModel.ScanCodeId,
                ScanCode = posData.Identifier,
                ScanCodeTypeId = scanCodeModel.ScanCodeTypeId,
                ScanCodeTypeDesc = scanCodeModel.ScanCodeTypeDesc,
                ChangeType = posData.ChangeType,
                LockedForSale = (posData.Recall_Flag || posData.RestrictSale),
                Recall = posData.Recall_Flag,
                TMDiscountEligible = posData.TMDiscountEligible,
                Case_Discount = posData.Case_Discount,
                AgeCode = posData.AgeCode,
                Restricted_Hours = posData.Restricted_Hours,
                Sold_By_Weight = posData.Sold_By_Weight,
                ScaleForcedTare = posData.ScaleForcedTare,
                Quantity_Required = posData.Quantity_Required,
                Price_Required = posData.Price_Required,
                QtyProhibit = posData.QtyProhibit,
                VisualVerify = posData.VisualVerify,
                LinkedItemScanCode = newLinkedScanCode,
                PreviousLinkedItemScanCode = currentLinkedScanCode,
                PosScaleTare = posData.PosScaleTare,
                InProcessBy = null,
                ProcessedDate = null
            };

            return message;
        }

        private string GetCurrentLinkedScanCode(string identifier, int businessUnitId)
        {
            return linkedScanCodeCacheHelper.Retrieve(new Tuple<string, int>(identifier, businessUnitId));
        }

        private string GetNewLinkedScanCode(string linkedIdentifier)
        {
            if (String.IsNullOrEmpty(linkedIdentifier))
            {
                return null;
            }

            var linkedScanCode = scanCodeCacheHelper.Retrieve(linkedIdentifier);

            if (linkedScanCode.ItemIsBottleDepositOrCrv() && IsValidated(linkedScanCode))
            {
                return linkedScanCode.ScanCode;
            }
            else
            {
                return null;
            }
        }

        private bool IsValidated(ScanCodeModel linkedScanCode)
        {
            return !String.IsNullOrEmpty(linkedScanCode.ValidationDate);
        }

        private Locale GetLocale(int businessUnitId)
        {
            return localeCacheHelper.Retrieve(businessUnitId);
        }

        private ScanCodeModel GetScanCode(string identifier)
        {
            return scanCodeCacheHelper.Retrieve(identifier);
        }
    }
}
