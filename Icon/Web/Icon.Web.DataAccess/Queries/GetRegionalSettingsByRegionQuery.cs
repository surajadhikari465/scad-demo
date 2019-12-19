using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    class GetRegionalSettingsByRegionQuery : IQueryHandler<GetRegionalSettingsByRegionParameters, List<RegionalSettingsModel>>
    {
        private IconContext context;

        public GetRegionalSettingsByRegionQuery(IconContext context)
        {
            this.context = context;
        }

        public List<RegionalSettingsModel> Search(GetRegionalSettingsByRegionParameters parameters)
        {
            SqlParameter regionCode = new SqlParameter("regionCode", SqlDbType.NVarChar);
            regionCode.Value = parameters.RegionCode;

            string sql = "EXEC app.GetRegionalConfigurationByRegionCode @regionCode";

            List<RegionalSettingsModel> sqlOutput = new List<RegionalSettingsModel>();

            try
            {
                var task = context.Database.SqlQuery<RegionalSettingsModel>(sql, regionCode);
                sqlOutput = task.ToList();
            }
            catch
            {
                throw;
            }
            finally
            {
                this.context.Database.Connection.Close();
            }
            return sqlOutput;
        }
    }
}
