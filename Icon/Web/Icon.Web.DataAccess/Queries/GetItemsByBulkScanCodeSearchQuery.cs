using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsByBulkScanCodeSearchQuery : IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>
    {
        private readonly IconContext context;

        public GetItemsByBulkScanCodeSearchQuery(IconContext context)
        {
            this.context = context;
        }

        public List<ItemSearchModel> Search(GetItemsByBulkScanCodeSearchParameters parameters)
        {
            var scanCodes = new SqlParameter("SearchScanCodes", SqlDbType.Structured);
            scanCodes.TypeName = "app.ScanCodeListType";

            scanCodes.Value = parameters.ScanCodes.ConvertAll(sc => new 
                {
                    ScanCode = sc
                }).ToDataTable();
            
            string sql = "exec app.GetItemsByBulkScanCodeSearch @SearchScanCodes";

            var queryResults = context.Database.SqlQuery<ItemSearchModel>(sql, scanCodes);

            return new List<ItemSearchModel>(queryResults);
        }
    }
}
