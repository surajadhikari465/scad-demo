using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Price.Controller.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Queries
{
    public class GetCancelAllSalesDataQuery : IQueryHandler<GetCancelAllSalesDataParameters, List<CancelAllSalesEventModel>>
    {
        private IDbProvider provider;

        public GetCancelAllSalesDataQuery(IDbProvider provider)
        {
            this.provider = provider;
        }

        public List<CancelAllSalesEventModel> Search(GetCancelAllSalesDataParameters parameters)
        {
            string sql = @" SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
                            
                            DECLARE @CancelAllSalesEventTypeID INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = 'CancelAllSales')
                            
                            SELECT	
                                q.QueueID				as QueueId,
	                            q.EventTypeID			as EventTypeId,
                                srm.Region_Code			as Region,
	                            q.Identifier			as ScanCode,
	                            s.BusinessUnit_ID		as BusinessUnitId,
                                pbd.StartDate           as EndDate
                            FROM 
	                            mammoth.PriceChangeQueue	        q
	                            JOIN Store					        s	    on	q.Store_No = s.Store_No
	                            JOIN StoreRegionMapping		        srm     on  s.Store_No = srm.Store_No
                                JOIN PriceBatchDetail               pbd     on  q.EventReferenceID = pbd.PriceBatchDetailID
                            WHERE
	                            q.InProcessBy = @Instance
                                AND q.EventTypeID = @CancelAllSalesEventTypeID";

            List<CancelAllSalesEventModel> priceData = provider.Connection
                .Query<CancelAllSalesEventModel>(sql,
                    new { Instance = parameters.Instance },
                    provider.Transaction)
                .ToList();

            return priceData;
        }
    }
}