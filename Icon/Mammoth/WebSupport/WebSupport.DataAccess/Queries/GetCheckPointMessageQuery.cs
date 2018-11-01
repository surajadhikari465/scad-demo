using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetCheckPointMessageQuery : IQueryHandler<GetCheckPointMessageParameters, IEnumerable<CheckPointMessageModel>>
    {
        private MammothContext context;

        public GetCheckPointMessageQuery(MammothContext context)
        {
            this.context = context;
        }

        public IEnumerable<CheckPointMessageModel> Search(GetCheckPointMessageParameters parameters)
        {
            var sql = "EXEC gpm.GetCheckPointMessageData @Region, @BusinessUnitIds, @ScanCodes";

            var regionParam = new SqlParameter("Region", SqlDbType.NVarChar) { Value = parameters.Region };
            var businessUnitsParam = parameters.BusinessUnitIds.Select(bu => new { BusinessUnitId = bu } )
                .ToTvp("BusinessUnitIds", "gpm.BusinessUnitIdsType");
            var scanCodesParam = parameters.ScanCodes.Select(sc => new { ScanCode = sc })
                .ToTvp("ScanCodes", "gpm.ScanCodesType");

            return context.Database.SqlQuery<CheckPointMessageModel>(
                sql, regionParam, businessUnitsParam, scanCodesParam)
            .ToList();
        }
    }
}