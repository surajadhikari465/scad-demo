using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetAllBusinessUnitsQueryHandler : IQueryHandler<GetAllBusinessUnitsQuery, List<int>>
    {
        private IDbProvider db;

        public GetAllBusinessUnitsQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public List<int> Search(GetAllBusinessUnitsQuery parameters)
        {
            List<int> businessUnits = db.Connection.Query<int>(
                @"SELECT BusinessUnitID from Locales_FL
                    UNION
                    SELECT BusinessUnitID from Locales_MA
                    UNION
                    SELECT BusinessUnitID from Locales_MW
                    UNION
                    SELECT BusinessUnitID from Locales_NA
                    UNION
                    SELECT BusinessUnitID from Locales_NC
                    UNION
                    SELECT BusinessUnitID from Locales_NE
                    UNION
                    SELECT BusinessUnitID from Locales_PN
                    UNION
                    SELECT BusinessUnitID from Locales_RM
                    UNION
                    SELECT BusinessUnitID from Locales_SO
                    UNION
                    SELECT BusinessUnitID from Locales_SP
                    UNION
                    SELECT BusinessUnitID from Locales_SW
                    UNION
                    SELECT BusinessUnitID from Locales_TS
                    UNION
                    SELECT BusinessUnitID from Locales_UK", 
                null, 
                db.Transaction).ToList();

            return businessUnits;
        }
    }
}
