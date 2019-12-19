using Icon.Common.DataAccess;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;

namespace Icon.Web.DataAccess.Commands
{
    public class BulkItemUploadCommandHandler : ICommandHandler<BulkItemUploadCommand>
    {
        private IDbProvider db;

        public BulkItemUploadCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(BulkItemUploadCommand data)
        {
            this.db.Connection.Query<int>(sql: "dbo.AddBulkItemUpload",
                param: new
                {
                    fileName = data.BulkItemUploadModel.FileName,
                    fileModeType = data.BulkItemUploadModel.FileModeType,
                    fileContent = data.BulkItemUploadModel.FileContent,
                    uploadedBy = data.BulkItemUploadModel.UploadedBy
                },
                transaction: this.db.Transaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}