using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace GlobalEventController.DataAccess.Queries
{
    public class GetRegionalSettingsBySettingsKeyNameQuery : IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>>
    {
        private readonly IconContext context;

        public GetRegionalSettingsBySettingsKeyNameQuery(IconContext context)
        {
            this.context = context;
        }

        public List<RegionalSettingsModel> Handle(GetRegionalSettingsBySettingsKeyNameParameters parameters)
        {
            SqlParameter settingSectionKeyName = new SqlParameter("settingSectionKeyName", SqlDbType.NVarChar);
            settingSectionKeyName.Value = parameters.SettingsKeyName;

            string sql = "EXEC app.GetRegionalConfigurationForSettingName @settingSectionKeyName";

            List<RegionalSettingsModel> sqlOutput = new List<RegionalSettingsModel>();

            var query = context.Database.SqlQuery<RegionalSettingsModel>(sql, settingSectionKeyName);
            sqlOutput = query.ToList();            

            return sqlOutput;
        }
    }
}
