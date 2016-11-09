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

namespace PushController.Controller.UdmEntityBuilders
{
    public class ItemPriceEntityBuilder : IUdmEntityBuilder<ItemPriceModel>
    {
        private ILogger<ItemPriceEntityBuilder> logger;
        private IEmailClient emailClient;
        private ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper;
        private ICacheHelper<int, Locale> localeCacheHelper;
        private IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler;
        private ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler;

        public ItemPriceEntityBuilder(
            ILogger<ItemPriceEntityBuilder> logger,
            IEmailClient emailClient,
            ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper,
            ICacheHelper<int, Locale> localeCacheHelper,
            IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler,
            ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.scanCodeCacheHelper = scanCodeCacheHelper;
            this.localeCacheHelper = localeCacheHelper;
            this.getLocaleQueryHandler = getLocaleQueryHandler;
            this.updateStagingTableDatesForUdmCommandHandler = updateStagingTableDatesForUdmCommandHandler;
        }

        public List<ItemPriceModel> BuildEntities(List<IRMAPush> posDataReadyForUdm)
        {
            var itemPriceEntities = new List<ItemPriceModel>();
            var failedRecords = new List<IRMAPush>();

            foreach (var posDataRecord in posDataReadyForUdm)
            {
                try
                {
                    switch (posDataRecord.ChangeType)
                    {
                        case Constants.IrmaPushChangeTypes.ScanCodeAdd:
                        case Constants.IrmaPushChangeTypes.ScanCodeAuthorization:
                            {
                                itemPriceEntities.Add(BuildItemPriceUpdate(posDataRecord, ItemPriceTypes.Reg));

                                if (posDataRecord.Sale_Price.HasValue)
                                {
                                    itemPriceEntities.Add(BuildItemPriceUpdate(posDataRecord, ItemPriceTypes.Tpr));
                                }

                                break;
                            }
                        case Constants.IrmaPushChangeTypes.RegularPriceChange:
                            {
                                itemPriceEntities.Add(BuildItemPriceUpdate(posDataRecord, ItemPriceTypes.Reg));
                                break;
                            }
                        case Constants.IrmaPushChangeTypes.NonRegularPriceChange:
                            {
                                if (posDataRecord.Sale_Price.HasValue)
                                {
                                    itemPriceEntities.Add(BuildItemPriceUpdate(posDataRecord, ItemPriceTypes.Reg));
                                    itemPriceEntities.Add(BuildItemPriceUpdate(posDataRecord, ItemPriceTypes.Tpr));
                                }
                                
                                break;
                            }
                        case Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange:
                        case Constants.IrmaPushChangeTypes.ScanCodeDeauthorization:
                        case Constants.IrmaPushChangeTypes.ScanCodeDelete:
                        case Constants.IrmaPushChangeTypes.CancelAllSales:
                            {
                                break;
                            }
                        default:
                            {
                                logger.Error(String.Format("ChangeType '{0}' is not a valid value.  IRMAPushID: {1}", posDataRecord.ChangeType, posDataRecord.IRMAPushID));
                                failedRecords.Add(posDataRecord);
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemPriceEntityBuilder>(this.logger);

                    string message = String.Format("A exception occurred when building the ItemPriceModel updates.  Region: {0}, Scan Code: {1}, IRMAPushID: {2}",
                        posDataRecord.RegionCode, posDataRecord.Identifier, posDataRecord.IRMAPushID);

                    exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    failedRecords.Add(posDataRecord);
                }
            }

            if (failedRecords.Count > 0)
            {
                string failureMessage = Resource.ItemPriceEntityBuildFailureEmailMessage;
                string emailSubject = Resource.ItemPriceEntityBuildFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForUdmBuildFailure(failureMessage, failedRecords);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemPriceEntityBuilder>(this.logger);
                    string message = "A failure occurred while attempting to send the alert email.";
                    exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());
                }

                var command = new UpdateStagingTableDatesForUdmCommand
                {
                    ProcessedSuccessfully = false,
                    StagedPosData = failedRecords,
                    Date = DateTime.Now
                };

                updateStagingTableDatesForUdmCommandHandler.Execute(command);
            }

            logger.Info(String.Format("Generated {0} ItemPrice entities for {1} staged POS record(s).",
                itemPriceEntities.Count, posDataReadyForUdm.Count));

            return itemPriceEntities;
        }

        private ItemPriceModel BuildItemPriceUpdate(IRMAPush posDataRecord, int priceTypeId)
        {
            var scanCodeModel = GetScanCode(posDataRecord.Identifier);
            Locale locale = GetLocale(posDataRecord.BusinessUnit_ID);

            int priceUomId;
            if (posDataRecord.Sold_By_Weight)
            {
                priceUomId = UOMs.Pound;
            }
            else
            {
                priceUomId = UOMs.Each;
            }

            if (priceTypeId == ItemPriceTypes.Reg)
            {
                return new ItemPriceModel
                {
                    IrmaPushId = posDataRecord.IRMAPushID,
                    ItemId = scanCodeModel.ItemId,
                    LocaleId = locale.localeID,
                    ItemPriceTypeId = priceTypeId,
                    ItemPriceAmount = posDataRecord.Price ?? 0m,
                    UomId = priceUomId,
                    CurrencyTypeId = CurrencyTypes.Usd,
                    BreakPointStartQuantity = posDataRecord.Multiple,
                    StartDate = DateTime.Now.Date,
                    EndDate = null
                };
            }
            else if (priceTypeId == ItemPriceTypes.Tpr)
            {
                return new ItemPriceModel
                {
                    IrmaPushId = posDataRecord.IRMAPushID,
                    ItemId = scanCodeModel.ItemId,
                    LocaleId = locale.localeID,
                    ItemPriceTypeId = priceTypeId,
                    ItemPriceAmount = posDataRecord.Sale_Price ?? 0m,
                    UomId = priceUomId,
                    CurrencyTypeId = CurrencyTypes.Usd,
                    BreakPointStartQuantity = posDataRecord.SaleMultiple,
                    StartDate = posDataRecord.Sale_Start_Date.Value.Date,
                    EndDate = posDataRecord.Sale_End_Date.Value.Date
                };
            }
            else
            {
                throw new ArgumentException(String.Format("Encountered an unknown itemPriceTypeID: {0}", priceTypeId));
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
