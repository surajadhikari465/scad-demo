using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Price.Controller.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Queries
{
    public class GetExistingPriceDataQuery : IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>>
    {
        private IDbProvider db;

        public GetExistingPriceDataQuery(IDbProvider db)
        {
            this.db = db;
        }

        public List<PriceEventModel> Search(GetExistingPriceDataParameters parameters)
        {
            // For rows without EventReferenceID (e.g. PriceBatchDetalID)
            // Need to get latest REG start date since it does not live in the Price table
            string sql = @" SET TRANSACTION ISOLATION LEVEL SNAPSHOT;

                            DECLARE @PriceEventTypeID INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = 'Price')
                            DECLARE @PriceRollbackEventTypeID INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = 'PriceRollback')

                            ;WITH QueueTable AS
                            (
	                            SELECT
		                            q.QueueID,
		                            q.Item_Key,
		                            q.Identifier,
		                            q.Store_No,
		                            q.EventTypeID
	                            FROM
		                            mammoth.PriceChangeQueue	q
	                            WHERE
		                            q.EventReferenceID IS NULL
		                            AND q.InProcessBy = @Instance
                                    AND (q.EventTypeID  = @PriceEventTypeID or q.EventTypeID = @PriceRollbackEventTypeID)
                            )

                            -- REGs
                            SELECT	
                                q.QueueID				as QueueId,
	                            q.EventTypeID			as EventTypeId,
                                srm.Region_Code			as Region,
	                            q.Identifier			as ScanCode,
	                            s.BusinessUnit_ID		as BusinessUnitId,
	                            p.Price           		as NewRegularPrice, -- current Price.Price is acting as the new Reg
                                p.Multiple				as NewRegularMultiple,
	                            CASE
                                    WHEN pt.PriceChgTypeDesc <> 'REG' THEN p.Sale_Start_Date
                                    ELSE @Today
                                END                     as NewStartDate,	-- defaulting to current date
	                            NULL					as NewSaleEndDate,
                                NULL					as NewSalePrice,
                                NULL    				as NewSaleMultiple,
	                            'REG'					as NewPriceType,
	                            CASE
		                            WHEN ovu.Weight_Unit IS NOT NULL AND ovu.Weight_Unit = 1 THEN 'KG'
		                            WHEN iu.Weight_Unit = 1 THEN 'LB'
		                            ELSE 'EA'
	                            END						as PriceUom,
	                            c.CurrencyCode			as CurrencyCode,
	                            p.Price					as CurrentRegularPrice,
                                p.Multiple              as CurrentRegularMultiple,
	                            NULL					as CurrentSalePrice,
	                            NULL					as CurrentSaleMultiple,
	                            NULL					as CurrentSaleStartDate,
	                            NULL					as CurrentSaleEndDate,
	                            NULL					as CurrentPriceType,
                                NULL                    as ItemChangeTypeID
                            FROM 
	                            QueueTable              	q
	                            JOIN Price					p	on	q.Item_Key = p.Item_Key
										                            AND q.Store_No = p.Store_No
                                JOIN PriceChgType           pt  on  p.PriceChgTypeID = pt.PriceChgTypeID
	                            JOIN Store					s	on	q.Store_No = s.Store_No
	                            JOIN StoreRegionMapping     srm on  s.Store_No = srm.Store_No
	                            JOIN StoreJurisdiction		sj	on	s.StoreJurisdictionID = sj.StoreJurisdictionID
	                            JOIN Currency				c	on	sj.CurrencyID = c.CurrencyID
	                            JOIN ValidatedScanCode		v	on	q.Identifier = v.ScanCode
	                            JOIN Item					i	on	q.Item_Key = i.Item_Key
	                            LEFT JOIN ItemUnit			iu	on	i.Retail_Unit_ID = iu.Unit_ID
	                            LEFT JOIN ItemOverride		ov	on	i.Item_Key = ov.Item_Key
										                            AND sj.StoreJurisdictionID = ov.StoreJurisdictionID
	                            LEFT JOIN ItemUnit			ovu	on	ov.Retail_Unit_ID = ovu.Unit_ID
                            UNION
                            -- Promos
                            SELECT	
                                q.QueueID				as QueueId,
	                            q.EventTypeID			as EventTypeId,
                                srm.Region_Code			as Region,
	                            q.Identifier			as ScanCode,
	                            s.BusinessUnit_ID		as BusinessUnitId,
	                            p.Price           		as NewRegularPrice,
                                p.Multiple				as NewRegularMultiple,
	                            p.Sale_Start_Date		as NewStartDate,
	                            p.Sale_End_Date			as NewSaleEndDate,
                                p.Sale_Price			as NewSalePrice,
                                p.Sale_Multiple			as NewSaleMultiple,
	                            pct.PriceChgTypeDesc	as NewPriceType,
	                            CASE
		                            WHEN ovu.Weight_Unit IS NOT NULL AND ovu.Weight_Unit = 1 THEN 'KG'
		                            WHEN iu.Weight_Unit = 1 THEN 'LB'
		                            ELSE 'EA'
	                            END						as PriceUom,
	                            c.CurrencyCode			as CurrencyCode,
	                            p.Price					as CurrentRegularPrice,
                                p.Multiple              as CurrentRegularMultiple,
	                            NULL					as CurrentSalePrice,
	                            NULL					as CurrentSaleMultiple,
	                            NULL					as CurrentSaleStartDate,
	                            NULL					as CurrentSaleEndDate,
	                            NULL					as CurrentPriceType,
                                NULL                    as ItemChangeTypeID
                            FROM 
	                            QueueTable                  q	
	                            JOIN Price					p	on	q.Item_Key = p.Item_Key
										                            AND q.Store_No = p.Store_No
	                            JOIN PriceChgType			pct on	p.PriceChgTypeId = pct.PriceChgTypeID
	                            JOIN Store					s	on	q.Store_No = s.Store_No
	                            JOIN StoreRegionMapping     srm on  s.Store_No = srm.Store_No
	                            JOIN StoreJurisdiction		sj	on	s.StoreJurisdictionID = sj.StoreJurisdictionID
	                            JOIN Currency				c	on	sj.CurrencyID = c.CurrencyID
	                            JOIN ValidatedScanCode		v	on	q.Identifier = v.ScanCode
	                            JOIN Item					i	on	q.Item_Key = i.Item_Key
	                            LEFT JOIN ItemUnit			iu	on	i.Retail_Unit_ID = iu.Unit_ID
	                            LEFT JOIN ItemOverride		ov	on	i.Item_Key = ov.Item_Key
										                            AND sj.StoreJurisdictionID = ov.StoreJurisdictionID
	                            LEFT JOIN ItemUnit			ovu	on	ov.Retail_Unit_ID = ovu.Unit_ID
                            WHERE
	                            p.Sale_End_Date >= @Today;";

            List<PriceEventModel> priceData = db.Connection
                .Query<PriceEventModel>(sql,
                    new { Region = parameters.Region, Instance = parameters.Instance, Today = DateTime.Today },
                    db.Transaction)
                .ToList();

            return priceData;
        }
    }
}
