using Icon.Common;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetCheckPointMessageQuery : IQueryHandler<GetCheckPointMessageParameters, CheckPointMessageModel>
    {
        private MammothContext context;

        public GetCheckPointMessageQuery(MammothContext context)
        {
            this.context = context;
        }

        public CheckPointMessageModel Search(GetCheckPointMessageParameters parameters)
        {
            var businessUnitId = new SqlParameter("BusinessUnitId",Convert.ToInt32(parameters.BusinessUnitId));
            var scanCode = new SqlParameter("ScanCode",parameters.ScanCode);
            var region = new SqlParameter("Region",parameters.Region);

            return context.Database.SqlQuery<CheckPointMessageModel>(
                "EXEC gpm.GetCheckPointMessageData @Region, @BusinessUnitId, @ScanCode",
                region,
                businessUnitId,
                scanCode
                ).FirstOrDefault();
        }
    }
}