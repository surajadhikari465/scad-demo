using Dapper;
using Icon.Common.DataAccess;
using System.Data;
using System.Linq;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class DoesScanCodeExistQueryHandler : IQueryHandler<DoesScanCodeExistParameters, bool>
    {
        private IDbConnection connection;

        public DoesScanCodeExistQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public bool Search(DoesScanCodeExistParameters parameters)
        {
            return connection.Query<bool>($@"
                IF EXISTS (SELECT 1 FROM dbo.ScanCode WHERE ScanCode = @ScanCode)
                    SELECT CONVERT(BIT, 1)
                    SELECT CONVERT(BIT, 0)",
                    parameters)
                .Single();
        }
    }
}