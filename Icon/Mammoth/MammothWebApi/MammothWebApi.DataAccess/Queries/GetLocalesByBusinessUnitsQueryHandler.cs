using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetLocalesByBusinessUnitsQueryHandler : IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>
    {
        private IDbProvider db;

        public GetLocalesByBusinessUnitsQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<Locales> Search(GetLocalesByBusinessUnitsQuery parameters)
        {
            string sql = @"SELECT Region, LocaleID, BusinessUnitID, StoreName, StoreAbbrev, AddedDate, ModifiedDate 
                            FROM [dbo].[Locale]
                            WHERE BusinessUnitID IN @BusinessUnitIds";
            IEnumerable<Locales> locales = this.db.Connection.Query<Locales>(sql,
                new { BusinessUnitIds = parameters.BusinessUnits.Distinct().ToArray() },
                this.db.Transaction);
            return locales;
        }
    }
}
