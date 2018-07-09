using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;

namespace MammothWebApi.DataAccess.Commands
{
    public class CancelAllSalesCommandHandler : ICommandHandler<CancelAllSalesCommand>
    {
        private IDbProvider db;

        public CancelAllSalesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(CancelAllSalesCommand data)
        {
            db.Connection.Execute(
                 $@"CREATE TABLE #CancelAllSales
                    (  
                        ScanCode VARCHAR(13),
	                    BusinessUnitId INT,
                        EndDate DATETIME2(7),
                        EventCreatedDate DATETIME2(7)
                    )
                    INSERT INTO #CancelAllSales(
                        BusinessUnitId, 
                        ScanCode,
                        EndDate,
                        EventCreatedDate)
                    SELECT 
                        BusinessUnitId, 
                        ScanCode,
                        EndDate,
                        EventCreatedDate
                    FROM @CancelAllSales

                    SELECT     
                        p.PriceID           as PriceID,
	                    @MessageTypeId		as MessageTypeId,
	                    @MessageStatusId	as MessageStatusId,
	                    @MessageActionId	as MessageActionId,
	                    @Timestamp			as InsertDate,
	                    i.ItemID			as ItemId,
	                    t.ItemTypeCode		as ItemTypeCode,
	                    t.ItemTypeDesc		as ItemTypeDesc,
	                    l.BusinessUnitID	as BusinessUnitId,
	                    l.StoreName			as LocaleName,
	                    i.ScanCode			as ScanCode,
	                    p.PriceUom			as UomCode,
	                    c.CurrencyCode		as CurrencyCode,
                        'TPR'               as PriceTypeCode,
	                    p.PriceType         as SubPriceTypeCode,
	                    p.Price				as Price,
	                    p.Multiple			as Multiple,
	                    p.StartDate			as StartDate,
	                    tmp.EndDate			as EndDate
                    INTO #Tmp
                    FROM
                        #CancelAllSales                     tmp
                        JOIN dbo.Items		                i	ON	tmp.ScanCode	   = i.ScanCode
                        JOIN dbo.Price_{data.Region}        p   ON  tmp.BusinessUnitId = p.BusinessUnitId
                                                                AND p.ItemID           = i.ItemID
                        JOIN Currency                       c   ON  p.CurrencyID       = c.CurrencyID
	                    JOIN dbo.Locales_{data.Region}      l	ON	p.BusinessUnitId   = l.BusinessUnitID
	                    JOIN dbo.ItemTypes	                t	ON	i.ItemTypeID	   = t.ItemTypeID
                    WHERE
	                    p.EndDate IS NOT NULL 
                        AND p.EndDate > CONVERT(DATE,GetDate())
                        AND p.PriceType <> 'REG'
	                    AND p.Region = @Region
                        AND p.AddedDate < tmp.EventCreatedDate;

                    UPDATE P
                    SET EndDate = CONVERT(DATE, tmp.EndDate)
                    FROM Price_{data.Region} p WITH (UPDLOCK, ROWLOCK)
                    INNER JOIN #Tmp tmp ON tmp.PriceID = p.PriceID             
                    
                    INSERT INTO esb.MessageQueuePrice
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
                    FROM #tmp",
                   new
                   {
                       MessageTypeId = MessageTypes.Price,
                       MessageStatusId = MessageStatusTypes.Ready,
                       MessageActionId = data.MessageActionId,
                       Timestamp = data.Timestamp,
                       Region = new DbString { Value = data.Region, Length = 2 },
                       CancelAllSales = data.CancelAllSalesModelList
                                               .ToDataTable()
                                               .AsTableValuedParameter("app.CancelAllSalesType")
                   },
                   db.Transaction);
        }
    }
}