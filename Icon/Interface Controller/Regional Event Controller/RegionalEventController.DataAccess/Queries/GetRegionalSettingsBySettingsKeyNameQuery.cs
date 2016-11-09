using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetRegionalSettingsBySettingsKeyNameQuery : IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>>
    {
        private IconContext context;

        public GetRegionalSettingsBySettingsKeyNameQuery(IconContext context)
        {
            this.context = context;
        }

        public List<RegionalSettingsModel> Execute(GetRegionalSettingsBySettingsKeyNameParameters parameters)
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
