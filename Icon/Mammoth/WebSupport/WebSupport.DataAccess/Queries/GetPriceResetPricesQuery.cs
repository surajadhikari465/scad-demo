using Icon.Common;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetPriceResetPricesQuery : IQueryHandler<GetPriceResetPricesParameters, List<PriceResetPrice>>
    {
        private MammothContext context;

        public GetPriceResetPricesQuery(MammothContext context)
        {
            this.context = context;
        }

        public List<PriceResetPrice> Search(GetPriceResetPricesParameters parameters)
        {
            var businessUnitIds = parameters.BusinessUnitIds.Select(bu => new { BusinessUnitId = int.Parse(bu) }).ToList();
            var scanCodes = parameters.ScanCodes.Select(sc => new { ScanCode = sc }).ToList();

            return context.Database.SqlQuery<PriceResetPrice>(
                "EXEC gpm.GetPriceResetPrices @Region, @BusinessUnitIds, @ScanCodes",
                new SqlParameter("Region", SqlDbType.NVarChar) { Value = parameters.Region },
                businessUnitIds.ToTvp("BusinessUnitIds", "gpm.BusinessUnitIdsType"),
                scanCodes.ToTvp("ScanCodes", "gpm.ScanCodesType"))
                .ToList();
        }
    }
}
