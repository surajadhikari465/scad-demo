using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Icon.Web.DataAccess.Queries
{
    public class GetNewScanCodeUploadsQuery : IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>
    {
        private readonly IconContext context;
        private ILogger logger;

        public GetNewScanCodeUploadsQuery(ILogger logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public List<ScanCodeModel> Search(GetNewScanCodeUploadsParameters parameters)
        {
            SqlParameter scanCodeList = new SqlParameter("ScanCodes", SqlDbType.Structured);
            scanCodeList.TypeName = "app.ScanCodeListType";
            scanCodeList.Value = parameters.ScanCodes.ToDataTable();

            string sql = "EXEC app.GetNewScanCodesImportValidation @ScanCodes";

            var sqlOutput = new List<string>();

            try
            {
                var task = context.Database.SqlQuery<string>(sql, scanCodeList);
                sqlOutput = task.ToList();
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }

            var newScanCodes = new List<ScanCodeModel>();
            newScanCodes = parameters.ScanCodes.Where(scanCode => sqlOutput.Any(result => result == scanCode.ScanCode)).ToList();

            return newScanCodes;
        }
    }
}
