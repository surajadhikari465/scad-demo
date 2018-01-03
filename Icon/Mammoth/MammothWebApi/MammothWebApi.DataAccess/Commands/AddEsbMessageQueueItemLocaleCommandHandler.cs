using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Logging;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddEsbMessageQueueItemLocaleCommandHandler : ICommandHandler<AddEsbMessageQueueItemLocaleCommand>
    {
        private ILogger logger;
        private IDbProvider db;

        public AddEsbMessageQueueItemLocaleCommandHandler(
            ILogger logger,
            IDbProvider db)
        {
            this.logger = logger;
            this.db = db;
        }

        public void Execute(AddEsbMessageQueueItemLocaleCommand data)
        {
            logger.Info(string.Format("Adding ItemLocale ESB messages for Region {0} and Timestamp {1} and TransactionId {2}",
                data.Region, data.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fffffff"), data.TransactionId.ToString()));

            string sql = @"
                                insert into
	                                esb.MessageQueueItemLocale
                                select
	                                [MessageTypeId] = @MessageTypeId,
                                    [MessageStatusId] = @MessageStatusId,
                                    [MessageHistoryId] = null,
                                    [MessageActionId] = @MessageActionId,
                                    [InsertDate] = sysdatetime(),
	                                [RegionCode] = @RegionCode,
                                    [BusinessUnitId] = s.BusinessUnitID,
                                    [ItemId] = i.ItemID,
                                    [ItemTypeCode] = it.ItemTypeCode,
                                    [ItemTypeDesc] = it.ItemTypeDesc,
                                    [LocaleName] = l.StoreName,
                                    [ScanCode] = i.ScanCode,
                                    [CaseDiscount] = s.Discount_Case,
                                    [TmDiscount] = s.Discount_TM,
                                    [AgeRestriction] = s.Restriction_Age,
                                    [RestrictedHours] = s.Restriction_Hours,
                                    [Authorized] = s.Authorized,
                                    [Discontinued] = s.Discontinued,
                                    [LabelTypeDescription] = s.LabelTypeDesc,
                                    [LocalItem] = s.LocalItem,
                                    [ProductCode] = s.Product_Code,
                                    [RetailUnit] = s.RetailUnit,
                                    [SignDescription] = s.Sign_Desc,
                                    [Locality] = s.Locality,
                                    [SignRomanceLong] = s.Sign_RomanceText_Long,
                                    [SignRomanceShort] = s.Sign_RomanceText_Short,
                                    [ColorAdded] = ca.AttributeValue,
                                    [CountryOfProcessing] = cop.AttributeValue,
                                    [Origin] = o.AttributeValue,
                                    [ElectronicShelfTag] = est.AttributeValue,
                                    [Exclusive] = exc.AttributeValue,
                                    [NumberOfDigitsSentToScale] = num.AttributeValue,
                                    [ChicagoBaby] = cb.AttributeValue,
                                    [TagUom] = tag.AttributeValue,
                                    [LinkedItem] = lnk.AttributeValue,
                                    [ScaleExtraText] = sce.AttributeValue,
                                    [Msrp] = s.Msrp,
                                    [InProcessBy] = null,
                                    [ProcessedDate] = null,
                                    [SupplierName] = ils.SupplierName,
                                    [IrmaVendorKey] = ils.IrmaVendorKey ,
                                    [SupplierItemID] = ils.SupplierItemID ,
                                    [SupplierCaseSize] = ils.SupplierCaseSize, 
                                    [OrderedByInfor] = s.OrderedByInfor
                                from
	                                stage.ItemLocale s
	                                join dbo.Items i on s.ScanCode = i.ScanCode
	                                join dbo.ItemTypes it on i.ItemTypeID = it.ItemTypeID
	                                join dbo.Locales_{0} l on s.BusinessUnitID = l.BusinessUnitID

	                                join stage.ItemLocaleSupplier ils  on ils.BusinessUnitID = l.BusinessUnitID
                                                                          AND ils.ScanCode = i.ScanCode
                                                                          AND ils.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended ca	on  i.ScanCode = ca.ScanCode
												                                AND l.BusinessUnitID = ca.BusinessUnitId
												                                AND ca.AttributeID = @ColorAddedId
                                                                                AND ca.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended cop  on  i.ScanCode = cop.ScanCode
												                                AND l.BusinessUnitID = cop.BusinessUnitId
												                                AND cop.AttributeID = @CountryOfProcessingId
                                                                                AND cop.TransactionId = @TransactionId  
  
	                                left join stage.ItemLocaleExtended o    on  i.ScanCode = o.ScanCode
												                                AND l.BusinessUnitID = o.BusinessUnitId
												                                AND o.AttributeID = @OriginId
                                                                                AND o.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended est  on  i.ScanCode = est.ScanCode
												                                AND l.BusinessUnitID = est.BusinessUnitId
												                                AND est.AttributeID = @EstId
                                                                                AND est.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended exc  on  i.ScanCode = exc.ScanCode
												                                AND l.BusinessUnitID = exc.BusinessUnitId
												                                AND exc.AttributeID = @ExclusiveId
                                                                                AND exc.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended num  on  i.ScanCode = num.ScanCode
												                                AND l.BusinessUnitID = num.BusinessUnitId
												                                AND num.AttributeID = @NumDigitsToScaleId
                                                                                AND num.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended cb	on  i.ScanCode = cb.ScanCode
												                                AND l.BusinessUnitID = cb.BusinessUnitId
												                                AND cb.AttributeID = @ChicagoBabyId
                                                                                AND cb.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended tag  on  i.ScanCode = tag.ScanCode
												                                AND l.BusinessUnitID = tag.BusinessUnitId
												                                AND tag.AttributeID = @TagUomId
                                                                                AND tag.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended lnk  on  i.ScanCode = lnk.ScanCode
												                                AND l.BusinessUnitID = lnk.BusinessUnitId
												                                AND lnk.AttributeID = @LinkedScanCodeId
                                                                                AND lnk.TransactionId = @TransactionId

	                                left join stage.ItemLocaleExtended sce  on  i.ScanCode = sce.ScanCode
												                                AND l.BusinessUnitID = sce.BusinessUnitId
												                                AND sce.AttributeID = @ScaleExtraTextId
                                                                                AND sce.TransactionId = @TransactionId
                                where
	                                s.Region = @RegionCode and
	                                s.TransactionId = @TransactionId";

            sql = String.Format(sql, data.Region);

            object parameters = new
            {
                TransactionId = data.TransactionId,
                Timestamp = data.Timestamp,
                RegionCode = new DbString { Value = data.Region, Length = 2 },
                MessageTypeId = MessageTypes.ItemLocale,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageActionId = MessageActions.AddOrUpdate,
                ColorAddedId = Attributes.ColorAdded,
                CountryOfProcessingId = Attributes.CountryOfProcessing,
                OriginId = Attributes.Origin,
                EstId = Attributes.ElectronicShelfTag,
                ExclusiveId = Attributes.Exclusive,
                NumDigitsToScaleId = Attributes.NumberOfDigitsSentToScale,
                ChicagoBabyId = Attributes.ChicagoBaby,
                TagUomId = Attributes.TagUom,
                LinkedScanCodeId = Attributes.LinkedScanCode,
                ScaleExtraTextId = Attributes.ScaleExtraText
            };

            int affectedRows = db.Connection.Execute(sql, parameters, transaction: db.Transaction);

            logger.Info(string.Format("Added {0} ItemLocale ESB messages for Region {1} and Timestamp {2} and TransactionId {3}",
                affectedRows, data.Region, data.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fffffff"), data.TransactionId.ToString()));
        }
    }
}
