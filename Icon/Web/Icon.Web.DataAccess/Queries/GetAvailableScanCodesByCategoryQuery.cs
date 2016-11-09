using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAvailableScanCodesByCategoryQuery : IQueryHandler<GetAvailableScanCodesByCategoryParameters, List<IRMAItem>>
    {
        private IconContext context;

        public GetAvailableScanCodesByCategoryQuery(IconContext context)
        {
            this.context = context;
        }

        public List<IRMAItem> Search(GetAvailableScanCodesByCategoryParameters parameters)
        {
            SqlParameter CategoryId = new SqlParameter("@CategoryId", SqlDbType.Int);
            CategoryId.Value = parameters.CategoryId;

            SqlParameter MaxScanCodes = new SqlParameter("@MaxScanCodes", SqlDbType.Int);
            MaxScanCodes.Value = parameters.MaxScanCodes;

            string sql = @"app.GetAvailableScanCodesByCategory @CategoryId, @MaxScanCodes";

            var dbResult = this.context.Database.SqlQuery<IRMAItem>(sql, CategoryId, MaxScanCodes);

            return dbResult.ToList();
        }
    }
}