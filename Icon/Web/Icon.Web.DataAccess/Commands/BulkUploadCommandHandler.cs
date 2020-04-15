using Icon.Common.DataAccess;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class BulkUploadCommandHandler : ICommandHandler<BulkUploadCommand>
    {
        private IDbConnection connection;

        public BulkUploadCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(BulkUploadCommand data)
        {
            connection.Execute(sql: "dbo.AddBulkUpload",
                param: new
                {
                    fileName = data.BulkUploadModel.FileName,
                    bulkUploadDataTypeId = (int)data.BulkUploadModel.BulkUploadDataType,
                    fileModeTypeId = (int)data.BulkUploadModel.FileModeType,
                    fileContent = data.BulkUploadModel.FileContent,
                    uploadedBy = data.BulkUploadModel.UploadedBy
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}