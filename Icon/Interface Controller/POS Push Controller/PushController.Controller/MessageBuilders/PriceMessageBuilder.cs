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
    public class PriceMessageBuilder : IMessageBuilder<MessageQueuePrice>
    {
        private ILogger<PriceMessageBuilder> logger;
        private IEmailClient emailClient;
        private ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper;
        private ICacheHelper<int, Locale> localeCacheHelper;
        private IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler;
        private IQueryHandler<GetPriceUomQuery, UOM> getPriceUomQueryHandler;
        private IQueryHandler<GetItemPriceQuery, ItemPrice> getItemPriceQueryHandler;
        private ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler;

        public PriceMessageBuilder(
            ILogger<PriceMessageBuilder> logger,
            IEmailClient emailClient,
            ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper,
            ICacheHelper<int, Locale> localeCacheHelper,
            IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler,
            IQueryHandler<GetPriceUomQuery, UOM> getPriceUomQueryHandler,
            IQueryHandler<GetItemPriceQuery, ItemPrice> getItemPriceQueryHandler,
            ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.scanCodeCacheHelper = scanCodeCacheHelper;
            this.localeCacheHelper = localeCacheHelper;
            this.getLocaleQueryHandler = getLocaleQueryHandler;
            this.getPriceUomQueryHandler = getPriceUomQueryHandler;
            this.getItemPriceQueryHandler = getItemPriceQueryHandler;
            this.updateStagingTableDatesForEsbCommandHandler = updateStagingTableDatesForEsbCommandHandler;
        }

        public List<MessageQueuePrice> BuildMessages(List<IRMAPush> posDataReadyForEsb)
        {
            var priceMessages = new List<MessageQueuePrice>();
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
                        case Constants.IrmaPushChangeTypes.ScanCodeAdd:
                        case Constants.IrmaPushChangeTypes.ScanCodeAuthorization:
                        case Constants.IrmaPushChangeTypes.RegularPriceChange:
                            {
                                priceMessages.Add(BuildPriceMessage(posDataRecord, scanCodeModel, checkForPreviousSale: false));
                                break;
                            }
                        case Constants.IrmaPushChangeTypes.CancelAllSales:
                            {
                                var cancelAllSalesPriceMessage = BuildPriceMessage(posDataRecord, scanCodeModel, checkForPreviousSale: true);

                                if (cancelAllSalesPriceMessage.PreviousSalePrice == null || cancelAllSalesPriceMessage.PreviousSaleStartDate == null || cancelAllSalesPriceMessage.PreviousSaleEndDate == null)
                                {
                                    logger.Error(String.Format("CancelAllSales message could not be generated because no previous sale was found.  IRMAPushID: {0}, Identifier {1}, BusinessUnit {2}",
                                        posDataRecord.IRMAPushID,
                                        posDataRecord.Identifier,
                                        posDataRecord.BusinessUnit_ID));
                                }
                                else if (cancelAllSalesPriceMessage.PreviousSaleEndDate.Value.Date < DateTime.Now.Date)
                                {
                                    logger.Error(String.Format("CancelAllSales message could not be generated because the sale has expired.  IRMAPushID: {0}, Identifier {1}, BusinessUnit {2}, PreviousSaleEndDate: {3}",
                                        posDataRecord.IRMAPushID,
                                        posDataRecord.Identifier,
                                        posDataRecord.BusinessUnit_ID,
                                        cancelAllSalesPriceMessage.PreviousSaleEndDate));
                                }
                                else
                                {
                                    priceMessages.Add(cancelAllSalesPriceMessage);
                                }

                                break;
                            }
                        case Constants.IrmaPushChangeTypes.NonRegularPriceChange:
                            {
                                priceMessages.Add(BuildPriceMessage(posDataRecord, scanCodeModel, checkForPreviousSale: true));
                                break;
                            }
                        case Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange:
                        case Constants.IrmaPushChangeTypes.ScanCodeDeauthorization:
                        case Constants.IrmaPushChangeTypes.ScanCodeDelete:
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
                    var exceptionHandler = new ExceptionHandler<PriceMessageBuilder>(this.logger);

                    string message = String.Format("A exception occurred when building the Price message.  Region: {0}, Scan Code: {1}, IRMAPushID: {2}",
                        posDataRecord.RegionCode, posDataRecord.Identifier, posDataRecord.IRMAPushID);

                    exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    var failedRecord = posDataRecord.ToModel();
                    failedRecord.MessageBuildError = ex.Message;

                    failedRecords.Add(failedRecord);
                }
            }

            if (failedRecords.Count > 0)
            {
                string failureMessage = Resource.PriceMessageBuildFailureEmailMessage;
                string emailSubject = Resource.PriceMessageBuildFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForMessageBuildFailure(failureMessage, failedRecords);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<PriceMessageBuilder>(this.logger);
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

            logger.Info(String.Format("Generated {0} Price message(s) for {1} staged POS record(s).",
                priceMessages.Count, posDataReadyForEsb.Count));

            return priceMessages;
        }

        private bool PreventMessageGeneration(ScanCodeModel scanCodeModel)
        {
            if (StartupOptions.UseItemTypeInsteadOfNonMerchTrait)
            {
                if (scanCodeModel.ItemTypeCode == ItemTypeCodes.Coupon)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} has item type {ItemTypes.Descriptions.Coupon}.  No Price message will be generated.");
                    return true;
                }

                if (scanCodeModel.ItemTypeCode == ItemTypeCodes.NonRetail)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} has item type {ItemTypes.Descriptions.NonRetail}.  No Price message will be generated.");
                    return true;
                }
            }
            else
            {
                if (scanCodeModel.NonMerchandiseTrait == NonMerchandiseTraits.Coupon)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} is associated to a sub-brick with the {NonMerchandiseTraits.Coupon} non-merchandise trait.  No Price message will be generated.");
                    return true;
                }

                if (scanCodeModel.NonMerchandiseTrait == NonMerchandiseTraits.LegacyPosOnly)
                {
                    logger.Info($"Scan code {scanCodeModel.ScanCode} is associated to a sub-brick with the {NonMerchandiseTraits.LegacyPosOnly} non-merchandise trait.  No Price message will be generated.");
                    return true;
                }
            }

            if (scanCodeModel.DepartmentSaleTrait == "1")
            {
                logger.Info(String.Format("Scan code {0} has the department sale trait, so no Price message will be generated.",
                    scanCodeModel.ScanCode));
                return true;
            }

            return false;
        }

        private MessageQueuePrice BuildPriceMessage(IRMAPush posData, ScanCodeModel scanCodeModel, bool checkForPreviousSale)
        {
            Locale localeEntity = GetLocale(posData.BusinessUnit_ID);
            int priceUomId = posData.Sold_By_Weight ? UOMs.Pound : UOMs.Each;
            UOM priceUom = GetPriceUom(priceUomId);

            var priceMessage = new MessageQueuePrice
            {
                MessageTypeId = MessageTypes.Price,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
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
                UomCode = priceUom.uomCode,
                UomName = priceUom.uomName,
                CurrencyCode = CurrencyCodes.Usd,
                Price = posData.Price,
                Multiple = posData.Multiple,
                SalePrice = posData.Sale_Price,
                SaleMultiple = posData.SaleMultiple,
                SaleStartDate = posData.Sale_Start_Date,
                SaleEndDate = posData.Sale_End_Date,
                InProcessBy = null,
                ProcessedDate = null
            };

            if (checkForPreviousSale)
            {
                var query = new GetItemPriceQuery
                {
                    ItemId = priceMessage.ItemId,
                    LocaleId = priceMessage.LocaleId,
                    PriceTypeId = ItemPriceTypes.Tpr
                };

                ItemPrice previousSale = getItemPriceQueryHandler.Execute(query);

                if (previousSale != null)
                {
                    priceMessage.PreviousSalePrice = previousSale.itemPriceAmt;
                    priceMessage.PreviousSaleMultiple = previousSale.breakPointStartQty;
                    priceMessage.PreviousSaleStartDate = previousSale.startDate;
                    priceMessage.PreviousSaleEndDate = previousSale.endDate;
                }
            }

            return priceMessage;
        }
        
        private UOM GetPriceUom(int priceUomId)
        {
            UOM uom;
            if (Cache.uomIdToUom.TryGetValue(priceUomId, out uom))
            {
                return uom;
            }
            else
            {
                var query = new GetPriceUomQuery
                {
                    PriceUomId = priceUomId
                };

                uom = getPriceUomQueryHandler.Execute(query);

                Cache.uomIdToUom.Add(priceUomId, uom);

                return uom;
            }
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
