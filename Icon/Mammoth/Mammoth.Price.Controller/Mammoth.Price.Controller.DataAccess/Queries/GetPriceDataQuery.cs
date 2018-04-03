using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Price.Controller.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Queries
{
    public class GetPriceDataQuery : IQueryHandler<GetPriceDataParameters, List<PriceEventModel>>
    {
        private IDbProvider provider;

        public GetPriceDataQuery(IDbProvider provider)
        {
            this.provider = provider;
        }

        public List<PriceEventModel> Search(GetPriceDataParameters parameters)
        {
            string sql = @" SET TRANSACTION ISOLATION LEVEL SNAPSHOT;

                            DECLARE @PriceEventTypeID INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = 'Price')
                            DECLARE @PriceRollbackEventTypeID INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = 'PriceRollback')

                            -- Main Query
                            -- For rows that have EventReferenceID populated with PriceBatchDetailID
                            SELECT	
                                q.QueueID				as QueueId,
	                            q.EventTypeID			as EventTypeId,
                                srm.Region_Code			as Region,
	                            ii.Identifier			as ScanCode,
	                            s.BusinessUnit_ID		as BusinessUnitId,
	                            pbd.Price           	as NewRegularPrice,
                                pbd.Multiple            as NewRegularMultiple,
	                            pbd.StartDate			as NewStartDate,
	                            pbd.Sale_End_Date		as NewSaleEndDate,
                                pbd.Sale_Price          as NewSalePrice,
                                pbd.Sale_Multiple       as NewSaleMultiple,
	                            pct.PriceChgTypeDesc	as NewPriceType,
	                            CASE
		                            WHEN ovu.Weight_Unit IS NOT NULL AND ovu.Weight_Unit = 1 THEN 'KG'
		                            WHEN iu.Weight_Unit = 1 THEN 'LB'
		                            ELSE 'EA'
	                            END						as PriceUom,
	                            c.CurrencyCode			as CurrencyCode,
                                p.Price					as CurrentRegularPrice,
                                p.Multiple              as CurrentRegularMultiple,
	                            p.Sale_Price			as CurrentSalePrice,
	                            p.Sale_Multiple			as CurrentSaleMultiple,
	                            p.Sale_Start_Date		as CurrentSaleStartDate,
	                            p.Sale_End_Date			as CurrentSaleEndDate,
	                            pt.PriceChgTypeDesc		as CurrentPriceType
                            FROM 
	                            mammoth.PriceChangeQueue	q
	                            JOIN PriceBatchDetail		pbd on	q.EventReferenceID = pbd.PriceBatchDetailID
	                            JOIN Price					p	on	pbd.Store_No = p.Store_No
										                            AND pbd.Item_Key = p.Item_Key
	                            JOIN PriceChgType			pct on	pbd.PriceChgTypeID = pct.PriceChgTypeID -- pbd Price Change Type
	                            JOIN PriceChgType			pt	on	p.PriceChgTypeId = pt.PriceChgTypeID -- p Price Change Type
	                            JOIN Store					s	on	q.Store_No = s.Store_No
	                            JOIN StoreRegionMapping		srm on  s.Store_No = srm.Store_No
	                            JOIN StoreJurisdiction		sj	on	s.StoreJurisdictionID = sj.StoreJurisdictionID
	                            JOIN Currency				c	on	sj.CurrencyID = c.CurrencyID
	                            JOIN ValidatedScanCode		v	on	q.Identifier = v.ScanCode
	                            JOIN ItemIdentifier			ii	on	pbd.Item_Key = ii.Item_Key
										                            AND ii.Deleted_Identifier = 0
										                            AND ii.Remove_Identifier = 0
	                            LEFT JOIN ItemUnit			iu	on	pbd.Retail_Unit_ID = iu.Unit_ID
	                            LEFT JOIN ItemOverride		ov	on	pbd.Item_Key = ov.Item_Key
										                            AND sj.StoreJurisdictionID = ov.StoreJurisdictionID
	                            LEFT JOIN ItemUnit			ovu	on	ov.Retail_Unit_ID = ovu.Unit_ID
                            WHERE
	                            q.InProcessBy = @Instance
	                            AND q.EventReferenceID IS NOT NULL
                                AND q.EventTypeID IN (@PriceEventTypeID, @PriceRollbackEventTypeID);";

            List<PriceEventModel> priceData = provider.Connection
                .Query<PriceEventModel>(sql,
                    new { Instance = parameters.Instance },
                    provider.Transaction)
                .ToList();

            return priceData;
        }
    }
}