using Dapper;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.DataAccess.Queries
{
    public class GetLocalesByBusinessUnitsQuery : IQueryHandler<GetLocalesByBusinessUnitsParameters, IEnumerable<Locale>>
    {
        private IDbProvider db;

        public GetLocalesByBusinessUnitsQuery(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<Locale> Search(GetLocalesByBusinessUnitsParameters parameters)
        {
            string sql = $"SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev FROM [dbo].[Locale] WHERE BusinessUnitID IN @BusinessUnits";
            List<Locale> locales = this.db.Connection.Query<Locale>(sql, new { BusinessUnits = parameters.BusinessUnitIDs }, this.db.Transaction).ToList();
            return locales;
        }
    }
}
