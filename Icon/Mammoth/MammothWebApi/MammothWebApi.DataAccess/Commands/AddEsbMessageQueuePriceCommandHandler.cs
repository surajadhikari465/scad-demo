using Mammoth.Common.DataAccess;
using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Logging;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddEsbMessageQueuePriceCommandHandler : ICommandHandler<AddEsbMessageQueuePriceCommand>
    {
        private IDbProvider db;
        private ILogger logger;

        public AddEsbMessageQueuePriceCommandHandler(ILogger logger, IDbProvider db)
        {
            this.db = db;
            this.logger = logger;
        }

        public void Execute(AddEsbMessageQueuePriceCommand data)
        {
            logger.Info(string.Format("Adding Price ESB messages for Region {0} and Timestamp {1} and TransactionId {2}",
                data.Region, data.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fffffff"), data.TransactionId.ToString()));

            string sql = @"INSERT INTO esb.MessageQueuePrice
                            (
	                            MessageTypeId,
	                            MessageStatusId,
	                            MessageActionId,
	                            InsertDate,
	                            ItemId,
	                            ItemTypeCode,
	                            ItemTypeDesc,
	                            BusinessUnitId,
	                            LocaleName,
	                            ScanCode,
	                            UomCode,
	                            CurrencyCode,
                                PriceTypeCode,
	                            SubPriceTypeCode,
	                            Price,
	                            Multiple,
	                            StartDate,
	                            EndDate
                            )
                            SELECT
	                            @MessageTypeId		as MessageTypeId,
	                            @MessageStatusId	as MessageStatusId,
	                            @MessageActionId	as MessageActionId,
	                            @Timestamp			as InsertDate,
	                            i.ItemID			as ItemId,
	                            t.ItemTypeCode		as ItemTypeCode,
	                            t.ItemTypeDesc		as ItemTypeDesc,
	                            l.BusinessUnitID	as BusinessUnitId,
	                            l.StoreName			as LocaleName,
	                            p.ScanCode			as ScanCode,
	                            p.PriceUom			as UomCode,
	                            p.CurrencyCode		as CurrencyCode,
                                CASE WHEN p.PriceType <> 'REG' THEN 'TPR' ELSE 'REG' END as PriceTypeCode,
	                            CASE WHEN p.PriceType <> 'REG' THEN p.PriceType END as SubPriceTypeCode,
	                            p.Price				as Price,
	                            p.Multiple			as Multiple,
	                            p.StartDate			as StartDate,
	                            p.EndDate			as EndDate
                            FROM
	                            stage.Price	            p
	                            JOIN dbo.Locales_{0}    l	on	p.BusinessUnitId	= l.BusinessUnitID
	                            JOIN dbo.Items		    i	on	p.ScanCode			= i.ScanCode
	                            JOIN dbo.ItemTypes	    t	on	i.ItemTypeID		= t.ItemTypeID
                            WHERE
	                            p.TransactionId = @TransactionId
	                            AND p.Region = @Region;";

            sql = string.Format(sql, data.Region);

            int affectedRows = this.db.Connection.Execute(sql,
                new
                {
                    MessageTypeId = MessageTypes.Price,
                    MessageStatusId = MessageStatusTypes.Ready,
                    MessageActionId = data.MessageActionId,
                    Timestamp = data.Timestamp,
                    Region = new DbString { Value = data.Region, Length = 2 },
                    TransactionId = data.TransactionId
                },
                this.db.Transaction);

            logger.Info(string.Format("Added {0} Price ESB messages for Region {1} and Timestamp {2} and TransactionId {3}",
                affectedRows, data.Region, data.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fffffff"), data.TransactionId.ToString()));
        }
    }
}
