using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Queries
{
    public class GetScanCodesByIdentifierBulkQueryHandler : IQueryHandler<GetScanCodesByIdentifierBulkQuery, List<ScanCodeModel>>
    {
        private ILogger<GetScanCodesByIdentifierBulkQueryHandler> logger;
        private IRenewableContext<IconContext> context;

        public GetScanCodesByIdentifierBulkQueryHandler(
            ILogger<GetScanCodesByIdentifierBulkQueryHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public List<ScanCodeModel> Execute(GetScanCodesByIdentifierBulkQuery parameters)
        {
            if (parameters.Identifiers == null || parameters.Identifiers.Count == 0)
            {
                logger.Warn("GetScanCodesByIdentifierBulkQueryHandler was called with a null or empty list.  Check the calling code for errors.");
                return new List<ScanCodeModel>();
            }

            SqlParameter identifiersParameter = new SqlParameter("Identifiers", SqlDbType.Structured);
            identifiersParameter.TypeName = "app.ScanCodeListType";

            identifiersParameter.Value = parameters.Identifiers.ConvertAll(sc => new
                {
                    ScanCode = sc
                }).ToDataTable();

            string sql = "EXEC app.GetScanCodeModelsForMessageGeneration @Identifiers";

            var queryResults = context.Context.Database.SqlQuery<ScanCodeModel>(sql, identifiersParameter);

            return new List<ScanCodeModel>(queryResults);
        }
    }
}
