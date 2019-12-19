using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    class GetRegionalSettingsBySettingsKeyNameQuery : IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>>
    {
        private IconContext context;

        public GetRegionalSettingsBySettingsKeyNameQuery(IconContext context)
        {
            this.context = context;
        }

        public List<RegionalSettingsModel> Search(GetRegionalSettingsBySettingsKeyNameParameters parameters)
        {
            SqlParameter settingSectionKeyName = new SqlParameter("settingSectionKeyName", SqlDbType.NVarChar);
            settingSectionKeyName.Value = parameters.SettingsKeyName;

            string sql = "EXEC app.GetRegionalConfigurationForSettingName @settingSectionKeyName";

            List<RegionalSettingsModel> sqlOutput = new List<RegionalSettingsModel>();

            try
            {
                var query = context.Database.SqlQuery<RegionalSettingsModel>(sql, settingSectionKeyName);
                sqlOutput = query.ToList();
            }
            catch
            {
                throw;
            }

            return sqlOutput;
        }
    }
}
