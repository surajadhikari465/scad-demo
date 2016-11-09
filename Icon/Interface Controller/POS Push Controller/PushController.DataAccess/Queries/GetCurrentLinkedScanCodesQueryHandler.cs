using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetCurrentLinkedScanCodesQueryHandler : IQueryHandler<GetCurrentLinkedScanCodesQuery, List<CurrentLinkedScanCodeModel>>
    {
        private ILogger<GetCurrentLinkedScanCodesQueryHandler> logger;
        private IRenewableContext<IconContext> context;

        public GetCurrentLinkedScanCodesQueryHandler(
            ILogger<GetCurrentLinkedScanCodesQueryHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public List<CurrentLinkedScanCodeModel> Execute(GetCurrentLinkedScanCodesQuery parameters)
        {
            if (parameters.ScanCodesByBusinessUnit == null || parameters.ScanCodesByBusinessUnit.Count == 0)
            {
                logger.Warn("GetScanCodesByIdentifierBulkQueryHandler was called with a null or empty list.  Check the calling code for errors.");
                return new List<CurrentLinkedScanCodeModel>();
            }

            SqlParameter scanCodesByBusinessUnitsParameter = new SqlParameter("ScanCodesByBusinessUnit", SqlDbType.Structured);
            scanCodesByBusinessUnitsParameter.TypeName = "app.ScanCodesByBusinessUnitType";

            scanCodesByBusinessUnitsParameter.Value = parameters.ScanCodesByBusinessUnit.Select(p => new { ScanCode = p.Item1, BusinessUnitId = p.Item2 }).ToList().ToDataTable();

            string sql = "EXEC app.GetLinkedScanCodeModelsForMessageGeneration @ScanCodesByBusinessUnit";

            var queryResults = context.Context.Database.SqlQuery<CurrentLinkedScanCodeModel>(sql, scanCodesByBusinessUnitsParameter);

            return new List<CurrentLinkedScanCodeModel>(queryResults);
        }
    }
}
