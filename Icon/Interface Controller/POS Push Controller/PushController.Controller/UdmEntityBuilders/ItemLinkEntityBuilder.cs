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
using System.Linq;
using System.Reflection;

namespace PushController.Controller.UdmEntityBuilders
{
    public class ItemLinkEntityBuilder : IUdmEntityBuilder<ItemLinkModel>
    {
        private ILogger<ItemLinkEntityBuilder> logger;
        private IEmailClient emailClient;
        private ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper;
        private ICacheHelper<int, Locale> localeCacheHelper;
        private IQueryHandler<GetLocalesByBusinessUnitIdQuery, List<Locale>> getLocaleQueryHandler;
        private ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler;

        public ItemLinkEntityBuilder(
            ILogger<ItemLinkEntityBuilder> logger,
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

        public List<ItemLinkModel> BuildEntities(List<IRMAPush> posDataReadyForUdm)
        {
            var itemLinkEntities = new List<ItemLinkModel>();
            var failedRecords = new List<IRMAPush>();

            foreach (var posDataRecord in posDataReadyForUdm)
            {
                try
                {
                    switch (posDataRecord.ChangeType)
                    {
                        case Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange:
                        case Constants.IrmaPushChangeTypes.ScanCodeAdd:
                        case Constants.IrmaPushChangeTypes.ScanCodeAuthorization:
                            {
                                if (string.IsNullOrEmpty(posDataRecord.LinkedIdentifier))
                                {
                                    var childLinkScanCode = GetScanCode(posDataRecord.Identifier);
                                    itemLinkEntities.Add(BuildItemLinkDelete(posDataRecord, childLinkScanCode));
                                }
                                else
                                {
                                    var childLinkScanCode = GetScanCode(posDataRecord.Identifier);
                                    var parentLinkScanCode = GetScanCode(posDataRecord.LinkedIdentifier);

                                    if (parentLinkScanCode.ItemIsBottleDepositOrCrv())
                                    {
                                        itemLinkEntities.Add(BuildItemLinkUpdate(posDataRecord, childLinkScanCode, parentLinkScanCode));
                                    }
                                }

                                break;
                            }
                        case Constants.IrmaPushChangeTypes.ScanCodeDeauthorization:
                        case Constants.IrmaPushChangeTypes.ScanCodeDelete:
                        case Constants.IrmaPushChangeTypes.RegularPriceChange:
                        case Constants.IrmaPushChangeTypes.NonRegularPriceChange:
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
                    var exceptionHandler = new ExceptionHandler<ItemLinkEntityBuilder>(this.logger);

                    string message = String.Format("A exception occurred when building the ItemLink updates.  Region: {0}, Scan Code: {1}, IRMAPushID: {2}",
                        posDataRecord.RegionCode, posDataRecord.Identifier, posDataRecord.IRMAPushID);

                    exceptionHandler.HandleException(message, ex, this.GetType(), MethodBase.GetCurrentMethod());

                    failedRecords.Add(posDataRecord);
                }
            }

            if (failedRecords.Count > 0)
            {
                string failureMessage = Resource.ItemLinkEntityBuildFailureEmailMessage;
                string emailSubject = Resource.ItemLinkEntityBuildFailureEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForUdmBuildFailure(failureMessage, failedRecords);

                try
                {
                    emailClient.Send(emailBody, emailSubject);
                }
                catch (Exception ex)
                {
                    var exceptionHandler = new ExceptionHandler<ItemLinkEntityBuilder>(this.logger);

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

            logger.Info(String.Format("Generated {0} ItemLink entities for {1} staged POS record(s).",
                itemLinkEntities.Count, posDataReadyForUdm.Count));

            return itemLinkEntities;
        }

        private ItemLinkModel BuildItemLinkUpdate(IRMAPush posDataRecord, ScanCodeModel childLinkScanCode, ScanCodeModel parentLinkScanCode)
        {
            int localeId = GetLocale(posDataRecord.BusinessUnit_ID).localeID;

            var itemLink = new ItemLinkModel
            {
                IrmaPushId = posDataRecord.IRMAPushID,
                ParentItemId = parentLinkScanCode.ItemId,
                ChildItemId = childLinkScanCode.ItemId,
                LocaleId = localeId
            };

            return itemLink;
        }

        private ItemLinkModel BuildItemLinkDelete(IRMAPush posDataRecord, ScanCodeModel childLinkScanCode)
        {
            int localeId = GetLocale(posDataRecord.BusinessUnit_ID).localeID;

            var itemLink = new ItemLinkModel
            {
                IrmaPushId = posDataRecord.IRMAPushID,
                ParentItemId = 0,
                ChildItemId = childLinkScanCode.ItemId,
                LocaleId = localeId,
                IsDelete = true
            };

            return itemLink;
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
