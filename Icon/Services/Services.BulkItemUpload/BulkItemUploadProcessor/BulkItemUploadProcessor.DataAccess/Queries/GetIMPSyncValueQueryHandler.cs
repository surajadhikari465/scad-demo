using Icon.Common.DataAccess;
using System.Data;
using Dapper;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using BulkItemUploadProcessor.Common;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetIMPSyncValueQueryHandler : IQueryHandler<GetIMPSyncValueParameters, string>
    {
        private readonly IDbConnection dbConnection;

        public GetIMPSyncValueQueryHandler(IDbConnection connection)
        {
            this.dbConnection = connection;
        }

        public string Search(GetIMPSyncValueParameters parameters)
        {
            string sql = @"
                SELECT ItemAttributesJson
                FROM dbo.ItemView
                WHERE ScanCode = @ScanCode;";
            
            var results = dbConnection.QueryFirst<string>(sql, parameters);
            Dictionary<string, object> itemAttributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(results);

            if (itemAttributes.ContainsKey(Constants.Attributes.IMPSynchronized)) {
                return itemAttributes[Constants.Attributes.IMPSynchronized].ToString();
            }

            return null;
        }
    }
}
