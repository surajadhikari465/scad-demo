﻿using System.Data;
using Dapper;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class ClearErrorsCommandHandler : ICommandHandler<ClearErrorsCommand>
    {
        private readonly IDbConnection DbConnection;

        public ClearErrorsCommandHandler(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Execute(ClearErrorsCommand data)
        {
            var sql = "DELETE FROM BulkUploadErrors WHERE BulkUploadId = @BulkUploadId ";
            DbConnection.Execute(sql, new { data.BulkUploadId });
        }
    }
}