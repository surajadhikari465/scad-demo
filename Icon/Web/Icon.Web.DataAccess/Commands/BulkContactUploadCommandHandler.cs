using Icon.Common.DataAccess;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;

namespace Icon.Web.DataAccess.Commands
{
	public class BulkContactUploadCommandHandler : ICommandHandler<BulkContactUploadCommand>
	{
		private IDbProvider db;

		public BulkContactUploadCommandHandler(IDbProvider db)
		{
			this.db = db;
		}

		public void Execute(BulkContactUploadCommand data)
		{
			this.db.Connection.Query<int>(sql: "dbo.AddBulkContactUpload",
				param: new
				{
					fileName = data.BulkContactUploadModel.FileName,
					fileContent = data.BulkContactUploadModel.FileContent,
					uploadedBy = data.BulkContactUploadModel.UploadedBy,
                    @totalRecords = data.BulkContactUploadModel.TotalRecords
				},
				transaction: this.db.Transaction,
				commandType: System.Data.CommandType.StoredProcedure);
		}
	}
}