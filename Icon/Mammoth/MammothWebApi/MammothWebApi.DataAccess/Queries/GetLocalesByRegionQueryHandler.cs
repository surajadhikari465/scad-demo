using Dapper;
using Mammoth.Common;
using Mammoth.Common.DataAccess;
using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetLocalesByRegionQueryHandler : IQueryHandler<GetLocalesByRegionQuery, List<Locales>>
    {
        private IDbProvider db;

        public GetLocalesByRegionQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public List<Locales> Search(GetLocalesByRegionQuery parameters)
        {
            string sql = @" SELECT 
                                Region,
                                LocaleID,
	                            BusinessUnitID,
	                            StoreName,
	                            StoreAbbrev,
	                            CurrencyID,
	                            AddedDate,
	                            ModifiedDate
                            FROM 
                                dbo.Locales_FL";

            List<Locales> locales = this.db.Connection
                .Query<Locales>(String.Format(sql, parameters.Region))
                .ToList();

            return locales;
        }
    }
}
